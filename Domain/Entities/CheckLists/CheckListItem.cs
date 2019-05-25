using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.CheckLists
{
    public class CheckListItem
    {
        public int CheckListItemId { get; set; }
        public int CheckListId { get; set; }
        public CheckListEntity CheckListEntity { get; set; }
        public string Procedure { get; set; }
        public string ExpectedResult { get; set; }
        public TestResult CheckListTestResult { get; set; }
        public string CheckListComment { get; set; }
        public User LastExecutorCheckListUser { get; set; }// будет заполняться самостоятельно, после выполнения
        public DateTime LastExecutionDateTime { get; set; }
    }
}
