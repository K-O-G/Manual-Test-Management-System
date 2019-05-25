using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFUserRepository:IUserRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<User> Users => context.Users;
    }
}