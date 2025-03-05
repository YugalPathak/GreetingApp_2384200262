using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        void SaveGreeting(HelloGreetingEntity helloGreetingEntity);
        HelloGreetingEntity GetMessageById(int id);
        List<HelloGreetingEntity> GetMessages();
        bool UpdateMessage(int id, string updatedMessage);
        bool DeleteMessage(int id);
    }
}
