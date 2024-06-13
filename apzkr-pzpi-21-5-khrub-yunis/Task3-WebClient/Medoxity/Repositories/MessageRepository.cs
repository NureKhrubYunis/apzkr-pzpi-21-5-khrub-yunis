using Medoxity.Models;
using Microsoft.EntityFrameworkCore;

namespace Medoxity.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MedicalPlatformContext _context;

        public MessageRepository(MedicalPlatformContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(string senderUsername, string receiverUsername)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            var messages = await _context.Messages
                .Where(m => (m.SenderUsername == senderUsername && m.ReceiverUsername == receiverUsername) ||
                            (m.SenderUsername == receiverUsername && m.ReceiverUsername == senderUsername))
                .OrderBy(m => m.SentDate)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.SentDate = TimeZoneInfo.ConvertTimeFromUtc(message.SentDate, localTimeZone);
            }

            return messages;
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<IEnumerable<(string User, Message LastMessage)>> GetUsersWithMessagesAsync(string username)
        {
            var messages = await _context.Messages
                .Where(m => m.SenderUsername == username || m.ReceiverUsername == username)
                .GroupBy(m => m.SenderUsername == username ? m.ReceiverUsername : m.SenderUsername)
                .Select(g => new
                {
                    User = g.Key,
                    LastMessage = g.OrderByDescending(m => m.SentDate).FirstOrDefault()
                })
                .ToListAsync();

            return messages.Select(m => (m.User, m.LastMessage));
        }
    }
}
