using Newtonsoft.Json;
using PixelService.Services.Interface;
using RabbitMQ.Client;
using System.Text;

namespace PixelService.Services
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IConfiguration _config;
        private readonly ConnectionFactory _factory;
        public RabbitMQProducer(IConfiguration config)
        {
            _config = config;
            _factory = new ConnectionFactory()
            {
                HostName = _config.GetValue<String>("RabbitMQ:Host"),
                UserName = _config.GetValue<String>("RabbitMQ:UserName"),
                Password = _config.GetValue<String>("RabbitMQ:Password"),
                Port = 5672
            };
        }

        public void SendMessage<T>(T message)
        {
            if (message == null)
                return;
            try
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "track",
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
                    var jsonMessage = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: "",
                                   routingKey: "track",
                                   basicProperties: null,
                                   body: body);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
    }
}
