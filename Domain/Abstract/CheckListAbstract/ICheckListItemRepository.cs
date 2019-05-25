using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.CheckLists;

namespace Domain.Abstract.CheckListAbstract
{
    public interface ICheckListItemRepository
    {
        IEnumerable<CheckListItem> CheckListItems { get; }
    }
}
