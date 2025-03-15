using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {

        private readonly HelloGreetingContext _context;

        public GreetingRL(HelloGreetingContext context)
        {
            _context = context;

        }

        public async Task<bool> SaveGreeting(int userId, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false; // User not found

            var greeting = new GreetingEntity
            {
                id = userId,
                message = message
            };

            await _context.Greetings.AddAsync(greeting);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GreetingEntity> CreateGreeting(GreetingEntity greeting)
        {
            var userExists = await _context.Users.AnyAsync(u => u.id == greeting.id);
            if (!userExists)
            {
                throw new Exception("User not found.");
            }

            _context.Greetings.Add(greeting);
            await _context.SaveChangesAsync();
            return greeting;
        }

        public async Task<List<GreetingEntity>> GetGreetingsByUserId(int userId)
        {
            return _context.Greetings.Where(g => g.id == userId).ToList();
        }

        public async Task<List<GreetingEntity>> GetAllGreetings()
        {
            return await _context.Greetings.Include(g => g.User).ToListAsync();
        }

        public async Task<bool> UpdateGreeting(int id, string newMessage)
        {
            var greeting = await _context.Greetings.FirstOrDefaultAsync(g => g.GreetingId == id);
            if (greeting == null) return false;

            greeting.message = newMessage;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGreeting(int id)
        {
            var greeting = await _context.Greetings.FirstOrDefaultAsync(g => g.GreetingId == id);
            if (greeting == null) return false;

            _context.Greetings.Remove(greeting);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}