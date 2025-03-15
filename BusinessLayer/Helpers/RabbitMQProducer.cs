using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;  // RabbitMQ Client library for .NET
using System.Text;       // Required for encoding messages into byte arrays

public class RabbitMQProducer
{
    // ConnectionFactory is used to establish a connection with RabbitMQ Server
    private readonly ConnectionFactory _factory;

    // Constructor initializes the connection factory with RabbitMQ server details
    public RabbitMQProducer()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost", // RabbitMQ server running on local machine
            UserName = "guest",     // Default RabbitMQ username
            Password = "guest"      // Default RabbitMQ password
        };
    }

    // Method to send a message to RabbitMQ
    public void SendMessage(string message)
    {
        // Establishing a connection to RabbitMQ (using 'using' ensures disposal)
        using (var connection = _factory.CreateConnection())

        // Creating a channel for communication
        using (var channel = connection.CreateModel())
        {
            // Declaring a queue to ensure it exists before sending messages
            channel.QueueDeclare(
                queue: "helloQueue",   // Queue name (must match the consumer's queue)
                durable: false,        // If true, the queue persists after a server restart
                exclusive: false,      // If true, only this connection can use the queue
                autoDelete: false,     // If true, queue gets deleted if no consumers are connected
                arguments: null        // Additional settings (null means default)
            );

            // Encoding the message into a byte array (RabbitMQ only sends byte data)
            var body = Encoding.UTF8.GetBytes(message);

            // Publishing the message to RabbitMQ
            channel.BasicPublish(
                exchange: "",         // Default exchange (direct message routing)
                routingKey: "helloQueue", // The queue name to send the message to
                basicProperties: null, // Default message properties
                body: body             // Message data (byte array)
            );

            // Logging the sent message
            Console.WriteLine($" [x] Sent {message}");
        } // Connection and channel automatically close when 'using' block ends
    }
}