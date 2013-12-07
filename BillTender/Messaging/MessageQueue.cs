using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace BillTender.Messaging
{
    public class MessageQueue
    {
        private MessageList _messageList;
        private Dictionary<Type, IMessageHandler> _handlers =
            new Dictionary<Type, IMessageHandler>();
        private bool _running = false;

        private static XmlSerializer MessageListSerializer =
            new XmlSerializer(typeof(MessageList));

        public MessageQueue Register<T>(MessageHandler<T> handler)
            where T: Message
        {
            _handlers.Add(typeof(T), handler);
            return this;
        }

        public async Task InitializeAsync()
        {
            if (_messageList == null)
            {
                try
                {
                    var folder = ApplicationData.Current.LocalFolder;
                    var file = await folder.GetFileAsync("messages.xml");
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        _messageList = (MessageList)MessageListSerializer
                            .Deserialize(stream);
                        if (_messageList.Messages == null)
                            _messageList.Messages = new List<Message>();
                    }
                }
                catch (FileNotFoundException)
                {
                    // At first the file will not exist.
                    _messageList = new MessageList()
                    {
                        Messages = new List<Message>()
                    };
                }
            }

            await RunAsync();
        }

        public async Task PushAsync(Message message)
        {
            _messageList.Messages.Add(message);
            await SaveMessagesAsync();

            await RunAsync();
        }

        private async Task RunAsync()
        {
            if (!_running)
            {
                _running = true;

                try
                {
                    while (_messageList.Messages.Any())
                    {
                        var message = _messageList.Messages.First();
                        await HandleMessageAsync(message);
                        _messageList.Messages.RemoveAt(0);
                        await SaveMessagesAsync();
                    }
                }
                finally
                {
                    _running = false;
                }
            }
        }

        private async Task HandleMessageAsync(Message message)
        {
            var handler = _handlers[message.GetType()];
            await handler.HandleInternal(message);
        }

        private async Task SaveMessagesAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(
                "messages.xml",
                CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                MessageListSerializer.Serialize(stream, _messageList);
            }
        }
    }
}
