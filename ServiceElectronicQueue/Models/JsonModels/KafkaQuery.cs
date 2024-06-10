using ServiceElectronicQueue.Models.KafkaQueue;

namespace ServiceElectronicQueue.Models.JsonModels;

public class KafkaQuery
{
    public string NumberQueue { get; set; }
    public int NumberService { get; set; }
    public string Service { get; set; }

    public KafkaQuery(string numberQueue, int numberService, string service) =>
        (NumberQueue, NumberService, Service) = (numberQueue, numberService, service);
}