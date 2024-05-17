﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ServiceElectronicQueue.Models.ForViews;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Organization
    {
        [Key] 
        public Guid IdOrganization { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string? UniqueKey { get; set; }
        public byte[]? Logo { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<BranchOffice> Branches { get; set; }
        

        public Organization()
        { }

        public Organization(Guid idOrganization, string email, string password, string title) =>
            (IdOrganization, Email, Password, Title) = (idOrganization, email, password, title);

        public Organization(string email, string password, string title, string? uniqueKey, byte[]? logo) =>
            (IdOrganization, Email, Password, Title, UniqueKey, Logo) = (Guid.NewGuid(), email, password, title, uniqueKey, logo);

        public Organization(Guid idOrganization, string email, string password, string title, string? uniqueKey, byte[]? logo) =>
            (IdOrganization, Email, Password, Title, UniqueKey, Logo) = (idOrganization, email, password, title, uniqueKey, logo);
        
        /*public Organization ToDb(OrganizationRegisterForView view)
        {
            return new Organization(view.Email!, view.Password!, view.Title!, null);
        }*/
    }
}