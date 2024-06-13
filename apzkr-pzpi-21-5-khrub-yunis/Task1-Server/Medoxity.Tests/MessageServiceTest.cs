using Medoxity.Models;
using Medoxity.Repositories;
using Medoxity.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medoxity.Tests
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly MessageService _messageService;

        public MessageServiceTests()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _messageService = new MessageService(_mockMessageRepository.Object);
        }

        [Fact]
        public async Task SendMessageAsync_ValidData_MessageSent()
        {
            // Arrange
            var senderUsername = "sender";
            var receiverUsername = "receiver";
            var text = "Hello!";

            // Act
            await _messageService.SendMessageAsync(senderUsername, receiverUsername, text);

            // Assert
            _mockMessageRepository.Verify(repo => repo.AddMessageAsync(It.Is<Message>(
                m => m.SenderUsername == senderUsername &&
                     m.ReceiverUsername == receiverUsername &&
                     m.Text == text)), Times.Once);
        }

        [Fact]
        public async Task GetMessagesBetweenUsersAsync_MessagesExist_ReturnsMessages()
        {
            // Arrange
            var senderUsername = "sender";
            var receiverUsername = "receiver";
            var messages = new List<Message>
        {
            new Message { SenderUsername = senderUsername, ReceiverUsername = receiverUsername, Text = "Hello!" },
            new Message { SenderUsername = receiverUsername, ReceiverUsername = senderUsername, Text = "Hi!" }
        };

            _mockMessageRepository.Setup(repo => repo.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername))
                .ReturnsAsync(messages);

            // Act
            var result = await _messageService.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Text == "Hello!");
            Assert.Contains(result, m => m.Text == "Hi!");
        }

        [Fact]
        public async Task GetMessagesBetweenUsersAsync_NoMessagesExist_ReturnsEmptyList()
        {
            // Arrange
            var senderUsername = "sender";
            var receiverUsername = "receiver";

            _mockMessageRepository.Setup(repo => repo.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername))
                .ReturnsAsync(new List<Message>());

            // Act
            var result = await _messageService.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

}
