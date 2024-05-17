namespace ServiceElectronicQueue.Models
{
    public struct DataComeFrom
    {
        public DataComeFrom(byte status) => 
            AuthStatus = status;
        
        public byte AuthStatus { get; set; }
    }
}