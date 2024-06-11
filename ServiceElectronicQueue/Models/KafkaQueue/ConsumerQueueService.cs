using Confluent.Kafka;

namespace ServiceElectronicQueue.Models.KafkaQueue;

public class ConsumerQueueService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly string _topic;

    public ConsumerQueueService(IConsumer<Null, string> consumer, string topic)
    {
        _consumer = consumer;
        _topic = topic;
    }
    
    public string? GetMessage()
    {
        try
        {
            var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
            if (consumeResult != null)
            {
                var message = consumeResult.Message.Value;
                return message;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            _consumer.Unsubscribe();
            _consumer.Close();
        }
    }
}