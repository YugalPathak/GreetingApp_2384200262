using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;

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

         /// <summary>
        /// Returns a greeting message string.
        /// </summary>
        /// <returns>String containing "Hello, World!".</returns>
        public string GetGreetMessage(string? firstName, string? lastName)
        {
            if(firstName!=null && lastName!=null)
            {
                return $"{firstName}  {lastName}";
            }
            else if(firstName!=null)
            {
                return $"{firstName}";

            }
            else if (lastName != null)
            {
                return $"{lastName}";

            }
            else
            {
                return "Hello, World!";
            }
        }

        public void SaveGreeting(string message)
        {
            var greeting = new HelloGreetingEntity { message = message };
            _greetingRL.SaveGreeting(greeting);
        }

        public HelloGreetingEntity GetMessageById(int id)
        {
            return _greetingRL.GetMessageById(id);
        }

        public List<HelloGreetingEntity> GetMessages()
        {
            return _greetingRL.GetMessages();
        }

        public bool UpdateMessage(int id, string updatedMessage)
        {
            return _greetingRL.UpdateMessage(id, updatedMessage);
        }

        public bool DeleteMessage(int id)
        {
            return _greetingRL.DeleteMessage(id);
        }
    }
}