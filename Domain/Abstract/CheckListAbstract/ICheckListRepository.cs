using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.CheckLists;

namespace Domain.Abstract.CheckListAbstract
{
    public interface ICheckListRepository
    {
        IEnumerable<CheckListEntity> CheckLists { get; }
    }
}
