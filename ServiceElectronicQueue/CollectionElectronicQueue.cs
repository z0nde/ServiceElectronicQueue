using ServiceElectronicQueue.Models.KafkaQueue;

namespace ServiceElectronicQueue;

public static class CollectionElectronicQueue
{
    public static List<KafkaMessageClientToBranchOffice> _allClients { get; set; } = new();
}