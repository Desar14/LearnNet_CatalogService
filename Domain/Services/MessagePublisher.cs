using Azure.Core.Extensions;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearnNet_CatalogService.Domain.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;

        public MessagePublisher(ILogger<ProductService> logger, IConfiguration configuration, IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _client = serviceBusClientFactory.CreateClient("ProductUpdates");
        }

        public async Task PublishUpdateMessage(ProductDTO dto)
        {
            ServiceBusSender sender = _client.CreateSender(_configuration.GetSection("ServiceBus")["Topic"]);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            var data = JsonSerializer.Serialize(dto);

            _logger.LogDebug("Trying to add message to queue");

            _logger.LogTrace($"Message content: {data}");

            if (!messageBatch.TryAddMessage(new ServiceBusMessage(data)))
            {
                // if it is too large for the batch
                _logger.LogError("The message is too large to fit in the batch.");
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
                _logger.LogDebug("The message has been published to the topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sending message error");
            }
        }
    }
}
