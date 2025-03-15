using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        Task<GreetingEntity> CreateGreeting(GreetingEntity greeting);
        Task<List<GreetingEntity>> GetGreetingsByUserId(int userId);
        Task<List<GreetingEntity>> GetAllGreetings();
        Task<bool> UpdateGreeting(int id, string newMessage);
        Task<bool> DeleteGreeting(int id);
        Task<bool> SaveGreeting(int userId, string message);
    }
}