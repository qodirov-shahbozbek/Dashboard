using Dashboard.Application.Abstractions;
using Dashboard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Dashboard.Application.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationSection _configurationSection;
        public string queueName = String.Empty;

        public ConsumerService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configurationSection = configuration.GetSection("MessageBroker");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = int.Parse(_configurationSection["Port"]),
                UserName = _configurationSection["Username"],
                Password = _configurationSection["Password"],
            };

            var connection = factory.CreateConnection();

            _channel = connection.CreateModel();
            queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: _configurationSection["Exchange"], routingKey: _configurationSection["RoutingKey"]);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ReceiveHandle;
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private async void ReceiveHandle(object sender, BasicDeliverEventArgs delievedArgs)
        {
            var json = Encoding.UTF8.GetString(delievedArgs.Body.ToArray());
            var requestDelievier = JsonConvert.DeserializeObject <Client>(json);

            using var scope = _serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var client = new Client()
            {
                Id = requestDelievier.Id,
                Password = requestDelievier.Password,
                UserName = requestDelievier.UserName,
                Balance = int.Parse(null)
            };

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }
    }
}
