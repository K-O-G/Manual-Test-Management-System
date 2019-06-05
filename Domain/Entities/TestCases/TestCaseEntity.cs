using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.TestCases
{
    [Table("TestCases")]
    public class TestCaseEntity
    {

        [Column("TestCaseId")]
        public int TestCaseEntityId { get; set; }
        public string TestCaseName { get; set; }
        public string TestCaseItemsIdSuffix { get; set; }
        public string TestCaseDescription { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual List<Case> Cases { get; set; }
        public virtual List<Component> Components { get; set; }
        public virtual User CreatorCaseUser { get; set; }
        public virtual User LastEditorCaseUser { get; set; }
        public DateTime LastEditionDateTime { get; set; }
        public TestCaseEntity()
        {
            this.Components = new List<Component>();
        }
    }
}
