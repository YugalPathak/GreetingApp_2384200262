using RepositoryLayer.Entity;
using System;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Greeting Business Logic Layer Interface.
    /// Contains Abstract Methods of Greeting Application Business Layer. 
    /// </summary>
    public interface IGreetingBL
    {
        /// <summary>
        /// Get a greeting message.
        /// </summary>
        /// <returns>A greeting message string.</returns>
        string GetGreetMessage(string? firstName , string? lastName);
        void SaveGreeting(string message);
        HelloGreetingEntity GetMessageById(int id);

        List<HelloGreetingEntity> GetMessages();
        bool UpdateMessage(int id, string updatedMessage);
        bool DeleteMessage(int id);
    }
}