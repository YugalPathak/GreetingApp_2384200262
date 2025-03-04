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
    }
}