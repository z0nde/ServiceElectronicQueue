namespace ServiceElectronicQueue.Models.KafkaQueue;

public static class ConfigKafka
{
    public static string Topic { get; } = "TopicElectronicQueue";
    public static string BootstrapServers { get; } = "localhost:9092";
}