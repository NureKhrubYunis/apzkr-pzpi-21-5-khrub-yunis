using Medoxity.Models;

namespace Medoxity.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(string senderUsername, string receiverUsername);
        Task<List<Message>> GetAllMessagesAsync();
        Task<IEnumerable<(string User, Message LastMessage)>> GetUsersWithMessagesAsync(string username);
    }
}
