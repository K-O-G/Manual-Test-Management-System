using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFComponentRepository:IComponentRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Component> Components => context.Components;
    }
}