namespace ServiceElectronicQueue.Models.JsonModels.TransmittingUrl
{
    public class BranchOfficeUrl
    {
        public string Email { get; set; }
        public string Addres { get; set; }

        public BranchOfficeUrl(string email, string addres) =>
            (Email, Addres) = (email, addres);
        
        public void SetProperties(string email, string addres) =>
            (Email, Addres) = (email, addres);
    }
}