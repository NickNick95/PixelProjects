using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Storage.Data.Model;
using Storage.Data.Repositories;
using System.Text;

namespace StorageService.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IFileRepository _fileRepository;
        private readonly IConfiguration _config;

        public RabbitMqListener(IFileRepository fileRepository, IConfiguration config)
        {
            _config = config;
            _fileRepository = fileRepository;

            var factory = new ConnectionFactory()
            {
                HostName = _config.GetValue<string>("RabbitMQ:Host"),
                UserName = _config.GetValue<string>("RabbitMQ:UserName"),
                Password = _config.GetValue<string>("RabbitMQ:Password")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "track", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                if (content != null) {
                    var track = JsonConvert.DeserializeObject<TrackModel>(content);

                    if (track != null)
                        _fileRepository.SaveToTmpFile(_config.GetValue<string>("LogFileName"), track);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("track", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
