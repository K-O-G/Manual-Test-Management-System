using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFPriorityRepository :IPriorityRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Priority> Priorities => context.Priorities;
    }
}