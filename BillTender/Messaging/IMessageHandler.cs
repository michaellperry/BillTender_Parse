using System.Threading.Tasks;

namespace BillTender.Messaging
{
    public interface IMessageHandler
    {
        Task HandleInternal(Message message);
    }
}