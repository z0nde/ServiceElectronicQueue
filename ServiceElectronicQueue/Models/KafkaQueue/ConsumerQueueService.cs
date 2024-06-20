using Confluent.Kafka;
using Newtonsoft.Json;
using ServiceElectronicQueue.Models.JsonModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServiceElectronicQueue.Models.KafkaQueue;

public class ConsumerQueueService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;

    public ConsumerQueueService(IConsumer<string, string> consumer, string topic)
    {
        _consumer = consumer;
        _topic = topic;
    }

    public void GetAllMessage(string idBrOffice)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        // Подписка на топик
        _consumer.Subscribe(_topic);
        while (!cts.IsCancellationRequested)
        {
            try
            {
                // Получение сообщения
                ConsumeResult<string, string>? consumeResult = _consumer.Consume(cts.Token);
                if (consumeResult != null)
                {
                    if (consumeResult.Message.Key == idBrOffice)
                    {
                        var message = JsonConvert.DeserializeObject<KafkaMessageClientToBranchOffice>(consumeResult.Message.Value);
                        if (!CollectionElectronicQueue._allClients.Any(c => c.IdClient == message.IdClient))
                        {
                            CollectionElectronicQueue._allClients.Add(message);
                        }
                        // Подтверждение получения сообщения
                        //_consumer.Commit(consumeResult); 
                    }
                }
                else
                {
                    // Выход из цикла, если больше нет сообщений
                    break; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении сообщения: {ex.Message}");
                break;
            }
        }
    }
}