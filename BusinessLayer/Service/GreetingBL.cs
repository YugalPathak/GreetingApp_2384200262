using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
namespace BusinessLayer.Service
{
    /// <summary>
    /// Implementing Service of Business Layer in Greeting Application.
    /// </summary>
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }
        public async Task<bool> SaveGreeting(int userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty");

            return await _greetingRL.SaveGreeting(userId, message); // Pass userId and message to repository layer
        }

        public async Task<GreetingEntity> CreateGreeting(string message, int userId)
        {
            var greeting = new GreetingEntity
            {
                message = message,
                id = userId
            };

            return await _greetingRL.CreateGreeting(greeting);
        }

        public async Task<List<GreetingEntity>> GetGreetingsByUserId(int userId)
        {
            return await _greetingRL.GetGreetingsByUserId(userId);
        }

        public async Task<List<GreetingEntity>> GetAllGreetings()
        {
            return await _greetingRL.GetAllGreetings();
        }

        public async Task<bool> UpdateGreeting(int id, string newMessage)
        {
            return await _greetingRL.UpdateGreeting(id, newMessage);
        }

        public async Task<bool> DeleteGreeting(int id)
        {
            return await _greetingRL.DeleteGreeting(id);
        }

        public string GetGreeting(string? firstName = null, string? lastName = null)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return $"Hello, {firstName} {lastName}!";
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                return $"Hello, {firstName}!";
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                return $"Hello, {lastName}!";
            }
            return "Hello World!";
        }
    }
}