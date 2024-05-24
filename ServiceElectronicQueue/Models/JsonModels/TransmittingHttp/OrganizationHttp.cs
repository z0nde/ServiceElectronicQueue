namespace ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;

public class OrganizationHttp
{
    public Guid IdOrganization { get; set; }
    public string Password { get; set; }
    public string? UniqueKey { get; set; }
    public byte[]? Logo { get; set; }

    public OrganizationHttp(Guid idOrganization, string password, string? uniqueKey, byte[]? logo) =>
        (IdOrganization, Password, UniqueKey, Logo) = (idOrganization, password, uniqueKey, logo);
    
    public void SetProperties(Guid idOrganization, string password, string? uniqueKey, byte[]? logo) =>
        (IdOrganization, Password, UniqueKey, Logo) = (idOrganization, password, uniqueKey, logo);
}