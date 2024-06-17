namespace ServiceElectronicQueue.Models.KafkaQueue
{
    public class KafkaMessageClientToBranchOffice
    {
        public Guid IdClient { get; set; }
        public string NumberQueue { get; set; }
        public int NumberService { get; set; }
        public string Service { get; set; }

        public KafkaMessageClientToBranchOffice()
        { }
    
        public KafkaMessageClientToBranchOffice(Guid idClient, string numberQueue, int numberService, string service) =>
            (IdClient, NumberQueue, NumberService, Service) = (idClient, numberQueue, numberService, service);
    }
}