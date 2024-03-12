using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class UserRepository : IRepository<User>
    {
        private readonly CompanyDbContext _db;

        public UserRepository(CompanyDbContext context) => _db = context;

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public User GetByIndex(int id)
        {
            return _db.Users.Find(id);
        }

        public void Create(User item)
        {
            _db.Users.Add(item);
        }

        public void Update(User item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = _db.Users.Find(id);
            if (user != null)
                _db.Users.Remove(user);
        }
    }
}