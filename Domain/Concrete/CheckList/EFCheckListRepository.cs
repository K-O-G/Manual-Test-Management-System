using System.Collections.Generic;
using Domain.Abstract.CheckListAbstract;
using Domain.Entities.CheckLists;

namespace Domain.Concrete.CheckList
{
    public class EFCheckListRepository : ICheckListRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<CheckListEntity> CheckLists => context.CheckLists;
    }
}