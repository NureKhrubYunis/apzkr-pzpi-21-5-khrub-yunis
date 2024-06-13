using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string senderUsername, string receiverUsername, string text);
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(string senderUsername, string receiverUsername);
        Task<List<Message>> GetAllMessagesAsync();
    }
}
