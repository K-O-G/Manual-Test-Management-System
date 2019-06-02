using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Entities.CheckLists;
using Domain.Entities.TestCases;

namespace Domain.Entities
{
    public class Component
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDescription { get; set; }
        public virtual List<CheckListEntity> CheckLists { get; set; }
        public virtual List<TestCaseEntity> TestCases { get; set; }

        public Component()
        {
            this.TestCases = new List<TestCaseEntity>();
            this.CheckLists = new List<CheckListEntity>();
        }
    }
}
