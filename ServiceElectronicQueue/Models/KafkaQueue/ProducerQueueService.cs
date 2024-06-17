using Confluent.Kafka;
using Newtonsoft.Json;

namespace ServiceElectronicQueue.Models.KafkaQueue
{
    public class ProducerQueueService
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public ProducerQueueService(IProducer<string, string> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }
        
        public async void PostMessage(string message, string uniqueKey)
        {
            try
            {
                // Отправка сообщения
                await _producer.ProduceAsync(_topic, new Message<string, string> { Key = uniqueKey, Value = message });

                // Закрытие продюсера
                _producer.Flush();

                Console.WriteLine("Message POST successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}