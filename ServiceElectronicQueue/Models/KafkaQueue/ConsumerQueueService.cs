using Confluent.Kafka;
using Newtonsoft.Json;
using ServiceElectronicQueue.Models.JsonModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServiceElectronicQueue.Models.KafkaQueue;

public class ConsumerQueueService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;
    private readonly List<KafkaMessageClientToBranchOffice> _allClients = new();


    public ConsumerQueueService(IConsumer<string, string> consumer, string topic)
    {
        _consumer = consumer;
        _topic = topic;
    }

    public List<KafkaMessageClientToBranchOffice> GetAllMessage(string uniqueKey)
    {
        // Подписка на топик
        _consumer.Subscribe(ConfigKafka.Topic);
        while (true)
        {
            var consumeResult = _consumer.Consume(CancellationToken.None);
            if (consumeResult != null)
            {
                // Распарсить Json, сделать проверку на первичный ключ
                var message = JsonConvert.DeserializeObject<KafkaMessageClientToBranchOffice>(consumeResult.Message.Value);
                if (!_allClients.Any(c => c.IdClient == message.IdClient))
                {
                    _allClients.Add(message);
                }
                // Подтверждаем получение сообщения
                _consumer.Commit(consumeResult); 
            }
            else
            {
                // Выход из цикла, если больше нет сообщений
                break; 
            }
        }
        return _allClients;
    }
    
    public List<KafkaQuery> GetMessagesAsync()
    {
        DateTime startTime = DateTime.Now;
        TimeSpan duration = TimeSpan.FromSeconds(5);
        var messages = new List<KafkaQuery>();

        _consumer.Subscribe(ConfigKafka.Topic);
        while (DateTime.Now - startTime < duration)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
                if (consumeResult != null)
                {
                    messages.Add(JsonSerializer.Deserialize<KafkaQuery>(consumeResult.Message.Value)!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении из Kafka: {ex.Message}");
            }
        }
        return messages;
    }
}