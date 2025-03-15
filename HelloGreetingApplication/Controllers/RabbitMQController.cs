using Microsoft.AspNetCore.Mvc; // Required for building an API controller

// Defines the route for the controller as "api/RabbitMQ"
[Route("api/[controller]")]
[ApiController]
public class RabbitMQController : ControllerBase
{
    private readonly RabbitMQProducer _producer; // Instance of RabbitMQProducer for sending messages

    // Constructor: Injects the RabbitMQProducer dependency
    public RabbitMQController(RabbitMQProducer producer)
    {
        _producer = producer;
    }

    /// <summary>
    /// API endpoint to send a message to RabbitMQ.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    /// <returns>Returns HTTP 200 with confirmation.</returns>
    [HttpPost("send")]
    public IActionResult SendMessage([FromBody] string message)
    {
        _producer.SendMessage(message); // Calls the producer to send the message
        return Ok($"Message sent: {message}"); // Returns a success response
    }
}