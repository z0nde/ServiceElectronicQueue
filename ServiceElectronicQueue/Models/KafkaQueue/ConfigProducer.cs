namespace ServiceElectronicQueue.Models.KafkaQueue;

public class ConfigProducer
{
    public string ClientId { get; set; }
    
    public ConfigProducer(string id) => ClientId = id;
}