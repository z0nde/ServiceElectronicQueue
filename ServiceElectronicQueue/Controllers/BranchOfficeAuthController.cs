using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers
{
    public class BranchOfficeAuthController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        private readonly BranchOfficeManager _branchOfficeManager;
        private BranchOffice _branchOffice;
        private User _user;
        
        public BranchOfficeAuthController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
            _branchOfficeManager = new BranchOfficeManager(_unitOfWork);
            _branchOffice = new BranchOffice();
            _user = new User();
        }

        [HttpGet]
        public IActionResult BranchOfficeRegister(string jsonUserUrl)
        {
            ParserTransmittingGetDataContainer container = new ParserTransmittingGetDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize(jsonUserUrl);
            
            container.ParseSerialize(userAuthStatusPost, _user);
            return View();
        }

        [HttpPost]
        public IActionResult BranchOfficeRegister(BranchOfficeRegisterForView branchOfficeRegisterForView)
        {
            if (!ModelState.IsValid)
                return View();
            if (_branchOfficeManager.CheckRegisterModel(branchOfficeRegisterForView) != null)
            {
                Guid? orgId = _unitOfWork.OrganizationsRep.GetAll()
                    .Where(s => s.UniqueKey == branchOfficeRegisterForView.UniqueKeyOrganization)
                    .Select(s => s.IdOrganization).FirstOrDefault();
                
                if (orgId != null && orgId != Guid.Empty && _unitOfWork.BranchesRep.GetAll()
                        .Where(s => s.Email == branchOfficeRegisterForView.Email)
                        .Select(s => s).FirstOrDefault() == null)
                {
                    ParserTransmittingPostDataContainer container =
                        new ParserTransmittingPostDataContainer(_httpContextAccessor);
                    (DataComeFrom userAuthStatus, _user) = container.ParseDeserialize();
                    
                    _branchOffice.SetProperties(
                        Guid.NewGuid(),
                        branchOfficeRegisterForView.Email!,
                        branchOfficeRegisterForView.Password!,
                        branchOfficeRegisterForView.Addres!,
                        null,
                        (Guid)orgId
                    );
                    
                    _unitOfWork.BranchesRep.Create(_branchOffice);
                    _unitOfWork.Save();
                    
                    ParserTransmittingPostDataContainerWithBranchOffice containerWithBrOffice =
                        new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
                    (string jsonUserUrl, string jsonBrOfficeUrl) = containerWithBrOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
                    
                    return RedirectToAction("BranchOfficeAccount", "BranchOfficeAccount", new 
                        {jsonUserUrl, jsonBrOfficeUrl});
                }
            }
            return View();
        }
        
        
        
        [HttpGet]
        public IActionResult BranchOfficeLogin(string jsonUserUrl)
        {
            ParserTransmittingGetDataContainer container = new ParserTransmittingGetDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize(jsonUserUrl);
            
            container.ParseSerialize(userAuthStatusPost, _user);
            return View();
        }

        [HttpPost]
        public IActionResult BranchOfficeLogin(BranchOfficeLoginForView branchOfficeLoginForView)
        {
            if (!ModelState.IsValid)
                return View();
            if (_branchOfficeManager.CheckLoginModel(branchOfficeLoginForView) != null)
            {
                Guid? brOfficeId = _unitOfWork.BranchesRep.GetAll()
                    .Where(s => s.Email == branchOfficeLoginForView.Email && s.Password == branchOfficeLoginForView.Password)
                    .Select(s => s.IdBranchOffice).FirstOrDefault();
                if (brOfficeId != null && brOfficeId != Guid.Empty)
                {
                    ParserTransmittingPostDataContainer container =
                        new ParserTransmittingPostDataContainer(_httpContextAccessor);
                    (DataComeFrom userAuthStatus, _user) = container.ParseDeserialize();
                    
                    _branchOffice = _unitOfWork.BranchesRep.GetByIndex((Guid)brOfficeId);

                    ParserTransmittingPostDataContainerWithBranchOffice containerWithBrOffice =
                        new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
                    (string jsonUserUrl, string jsonBrOfficeUrl) = containerWithBrOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
                    
                    return RedirectToAction("BranchOfficeAccount", "BranchOfficeAccount", new 
                        {jsonUserUrl, jsonBrOfficeUrl});
                }
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}