namespace ServiceElectronicQueue.Models.KafkaQueue;

public class KafkaConfig
{
    public string BootstrapServers { get; set; }
    public string Topic { get; set; }
    public string GroupId { get; set; }
}