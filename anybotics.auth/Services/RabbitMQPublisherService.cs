using RabbitMQ.Client;
using System.Text;

namespace anybotics.auth.Services;

public class RabbitMQPublisherService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQPublisherService(string rabbitMqUrl, string queueName)
    {
        var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqUrl) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void PublishUserCreatedEvent(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "",
                              routingKey: "user.created",
                              basicProperties: null,
                              body: body);
    }
}

