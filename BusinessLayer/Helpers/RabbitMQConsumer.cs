using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

public class RabbitMQConsumer
{
    private readonly ConnectionFactory _factory;

    // Constructor: Initializes the RabbitMQ connection factory with hostname, username, and password
    public RabbitMQConsumer()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",  // RabbitMQ server address
            UserName = "guest",      // Default username
            Password = "guest"       // Default password
        };
    }

    public void StartListening()
    {
        // Establishes connection to RabbitMQ
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        // Declares a queue named "helloQueue" to ensure it exists
        channel.QueueDeclare(queue: "helloQueue",
                             durable: false,  // Messages are not persisted
                             exclusive: false, // Can be accessed by multiple consumers
                             autoDelete: false, // Queue remains even if consumers disconnect
                             arguments: null);

        // Creates a consumer to listen for messages
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();  // Extracts message body
            var message = Encoding.UTF8.GetString(body);  // Converts byte array to string
            Console.WriteLine($" [x] Received {message}");  // Prints received message
        };

        // Starts consuming messages from the queue
        channel.BasicConsume(queue: "helloQueue",
                             autoAck: true,  // Automatically acknowledges message receipt
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Thread.Sleep(Timeout.Infinite);  // Keeps the consumer running indefinitely
    }
}