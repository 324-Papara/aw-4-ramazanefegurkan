using Para.Bussiness.Notification;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para.Bussiness.Messaging
{
    public class EmailConsumer
    {
        private readonly INotificationService _notificationService;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EmailConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "emailQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void ProcessQueue()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var parts = message.Split('|');
                if (parts.Length == 3)
                {
                    var subject = parts[0];
                    var email = parts[1];
                    var content = parts[2];
                    _notificationService.SendEmail(subject, email, content);
                }
            };
            _channel.BasicConsume(queue: "emailQueue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}