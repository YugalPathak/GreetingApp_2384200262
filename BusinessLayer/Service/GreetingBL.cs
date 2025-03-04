using BusinessLayer.Interface;
using System;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Implementing Service of Business Layer in Greeting Application.
    /// </summary>
    public class GreetingBL : IGreetingBL
    {
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
    }
}