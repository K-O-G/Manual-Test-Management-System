using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.CheckLists
{
    public class CheckListItem
    {
        public int CheckListItemId { get; set; }
        public int? CheckListId { get; set; }
        public CheckListEntity CheckListEntity { get; set; }
        public string CheckListItemIdPublic { get; set; }
        public string Procedure { get; set; }
        public string ExpectedResult { get; set; }
        public virtual TestResult CheckListTestResult { get; set; }
        public virtual string CheckListComment { get; set; }
        public virtual User LastExecutorCheckListUser { get; set; }// будет заполняться самостоятельно, после выполнения
        public virtual DateTime? LastExecutionDateTime { get; set; }
    }
}
