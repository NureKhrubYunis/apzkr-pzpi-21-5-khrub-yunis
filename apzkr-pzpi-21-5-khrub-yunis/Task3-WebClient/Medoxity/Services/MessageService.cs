using Medoxity.Models;
using Medoxity.Repositories;

namespace Medoxity.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessageAsync(string senderUsername, string receiverUsername, string text)
        {
            var message = new Message
            {
                SenderUsername = senderUsername,
                ReceiverUsername = receiverUsername,
                Text = text,
                SentDate = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
        }

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(string senderUsername, string receiverUsername)
        {
            return await _messageRepository.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername);
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            return messages.Select(message => new Message
            {
                MessageID = message.MessageID,
                SenderUsername = message.SenderUsername,
                ReceiverUsername = message.ReceiverUsername,
                Text = message.Text,
                SentDate = message.SentDate
            }).ToList();
        }

        public async Task<IEnumerable<(string User, Message LastMessage)>> GetUsersWithMessagesAsync(string username)
        {
            return await _messageRepository.GetUsersWithMessagesAsync(username);
        }
    }
}
