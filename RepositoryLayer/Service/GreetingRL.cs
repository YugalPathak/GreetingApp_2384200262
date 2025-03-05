using RepositoryLayer.Interface;
using System;
using RepositoryLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Handles database operations related to greeting messages within the Repository Layer.
    /// </summary>
    public class GreetingRL : IGreetingRL
    {

        private readonly HelloGreetingContext _context;

        /// <summary>
        /// Initializes the repository with the specified database context for data access.
        /// </summary>
        /// <param name="context">The database context instance used to interact with the database.</param>
        public GreetingRL(HelloGreetingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserts a new greeting message into the database.
        /// </summary>
        /// <param name="helloGreetingEntity">The greeting entity containing the message to be stored.</param>
        public void SaveGreeting(HelloGreetingEntity helloGreetingEntity)
        {
            try
            {
                // Adding the greeting message to the database
                _context.Greetings.Add(helloGreetingEntity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Throwing an exception if there is an error during the saving process
                throw new Exception("An error occurred while saving the greeting message", ex);
            }
        }

        /// <summary>
        /// Retrieving message by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The greeting message</returns>
        /// <exception cref="Exception"></exception>
        public HelloGreetingEntity GetMessageById(int id)
        {
            try
            {
                return _context.Greetings.FirstOrDefault(g => g.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
