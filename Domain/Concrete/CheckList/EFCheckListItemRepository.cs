using System.Collections.Generic;
using Domain.Abstract.CheckListAbstract;
using Domain.Entities.CheckLists;

namespace Domain.Concrete.CheckList
{
    public class EFCheckListItemRepository :ICheckListItemRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<CheckListItem> CheckListItems => context.CheckListItems;
    }
}