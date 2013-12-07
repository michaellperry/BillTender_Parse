using System.Threading.Tasks;

namespace BillTender.Messaging
{
    public abstract class MessageHandler<T> : IMessageHandler
            where T : Message
    {
        protected abstract Task Handle(T message);

        public Task HandleInternal(Message message)
        {
            return Handle((T)message);
        }
    }
}
