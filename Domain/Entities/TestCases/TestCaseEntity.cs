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
        public string TestCaseDescription { get; set; }
        public Priority Priority { get; set; }
        public List<Case> Cases { get; set; }
        public List<Component> Components { get; set; }
        public User CreatorCaseUser { get; set; }
        public User LastEditorCaseUser { get; set; }
        public DateTime LastEditionDateTime { get; set; }
    }
}
