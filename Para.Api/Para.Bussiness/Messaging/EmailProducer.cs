using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para.Bussiness.Messaging
{
    public class EmailProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public EmailProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "emailQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendEmailToQueue(string subject, string email, string content)
        {
            var message = $"{subject}|{email}|{content}";
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                  routingKey: "emailQueue",
                                  basicProperties: null,
                                  body: body);
        }
    }
}
