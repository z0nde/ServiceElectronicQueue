using Confluent.Kafka;

namespace ServiceElectronicQueue.Models.KafkaQueue;

public class KafkaFactory
{
    public static IProducer<Null, string> CreateProducer(ConfigProducer producer)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = ConfigKafka.BootstrapServers,
            ClientId = producer.ClientId,
            EnableIdempotence = true
        };
        return new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public static IConsumer<Null, string> CreateConsumer(ConfigConsumer consumer)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = ConfigKafka.BootstrapServers,
            GroupId = consumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        return new ConsumerBuilder<Null, string>(consumerConfig).Build();
    }
}