namespace ServiceElectronicQueue.Models.JsonModels.TransmittingUrl
{
    public class OrganizationUrl
    {
        public string Email { get; set; }
        public string Title { get; set; }

        public OrganizationUrl(string email, string title) =>
            (Email, Title) = (email, title);
    }
}