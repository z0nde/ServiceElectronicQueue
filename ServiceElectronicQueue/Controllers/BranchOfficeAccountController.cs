using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers
{
    public class BranchOfficeAccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        private readonly BranchOfficeManager _branchOfficeManager;
        private BranchOffice _branchOffice;
        private User _user;
        
        public BranchOfficeAccountController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
            _branchOfficeManager = new BranchOfficeManager(_unitOfWork);
            _branchOffice = new BranchOffice();
            _user = new User();
        }
        
        [HttpGet]
        public IActionResult BranchOfficeAccount(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            var containerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);

            var model = new BranchOfficeAccountForView
            {
                TitleOrganization = _unitOfWork.OrganizationsRep.GetAll()
                    .Where(s => s.IdOrganization == _branchOffice.IdOrganization)
                    .Select(s => s.Title).FirstOrDefault(),
                Addres = _branchOffice.Addres,
                Surname = _user.Surname,
                Name = _user.Name,
                Patronymic = _user.Patronymic
            };
            
            containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            
            return View(model);
        }

        public IActionResult Services()
        {
            return RedirectToAction();
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}