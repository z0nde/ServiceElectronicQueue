namespace ServiceElectronicQueue.Models.JsonModels.TransmittingHttp
{
    public class BranchOfficeHttp
    {
        public Guid IdBranchOffice { get; set; }
        public string Password { get; set; }
        public string? UniqueLink { get; set; }
        public Guid IdOrganization { get; set; }

        public BranchOfficeHttp(Guid idBranchOffice, string password, string? uniqueLink, Guid idOrganization) =>
            (IdBranchOffice, Password, UniqueLink, IdOrganization) = 
            (idBranchOffice, password, uniqueLink, idOrganization);
        
        public void SetProperties(Guid idBranchOffice, string password, string? uniqueLink, Guid idOrganization) =>
            (IdBranchOffice, Password, UniqueLink, IdOrganization) = 
            (idBranchOffice, password, uniqueLink, idOrganization);
    }
}