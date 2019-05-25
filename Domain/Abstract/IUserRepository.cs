using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
    }
}
