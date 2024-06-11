namespace ServiceElectronicQueue.Models.KafkaQueue;

public class ConfigConsumer
{
    public string GroupId { get; set; }

    public ConfigConsumer(string groupId) => GroupId = groupId;
}