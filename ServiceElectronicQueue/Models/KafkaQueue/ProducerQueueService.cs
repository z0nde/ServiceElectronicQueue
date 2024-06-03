using Confluent.Kafka;

namespace ServiceElectronicQueue.Models.KafkaQueue
{
    public class ProducerQueueService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public ProducerQueueService(IProducer<Null, string> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }
        
        public async void PostMessage(string message)
        {
            try
            {
                // Отправка сообщения
                await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });

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