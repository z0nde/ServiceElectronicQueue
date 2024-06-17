using Confluent.Kafka;

namespace ServiceElectronicQueue.Models.KafkaQueue;

public class KafkaFactory
{
    public static IProducer<string, string> CreateProducer()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = ConfigKafka.BootstrapServers,
            EnableIdempotence = true,
            Partitioner = Partitioner.Murmur2
        };
        return new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public static IConsumer<string, string> CreateConsumer(ConfigConsumer consumer)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = ConfigKafka.BootstrapServers,
            GroupId = consumer.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        return new ConsumerBuilder<string, string>(consumerConfig).Build();
    }
}