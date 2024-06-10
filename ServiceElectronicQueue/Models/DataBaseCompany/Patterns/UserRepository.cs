using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class UserRepository : IRepository<User>
    {
        private readonly CompanyDbContext _db;

        public UserRepository(CompanyDbContext context) => _db = context;

        /*public UserRepository()
        { }*/

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public User GetByIndex(Guid id)
        {
            return _db.Users
                .Where(s => s.IdUser == id)
                .Select(s => s)
                .First();
        }

        public void Create(User item)
        {
            _db.Users.Add(item);
        }

        public void Update(Guid id, User newItem)
        {
            var unitOfWork = new UnitOfWorkCompany(_db);
            var oldUser = unitOfWork.UsersRep.GetByIndex(id);
            oldUser.Email = newItem.Email;
            oldUser.Password = newItem.Password;
            oldUser.Surname = newItem.Surname;
            oldUser.Name = newItem.Name;
            oldUser.Patronymic = oldUser.Patronymic;
            oldUser.PhoneNumber = newItem.PhoneNumber;
        }

        public void Delete(Guid id)
        {
            User user = _db.Users.Find(id);
            if (user != null)
                _db.Users.Remove(user);
        }
    }
}