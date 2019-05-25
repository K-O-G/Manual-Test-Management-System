using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.TestCases
{
    public class Case
    {
        public int CaseId { get; set; }
        public int TestCaseId { get; set; }
        public TestCaseEntity TestCase { get; set; }
        public string CaseSummary { get; set; }
        public string CasePreconditions { get; set; }
        public ICollection<CaseStep> CaseSteps { get; set; }
        public string CaseComment { get; set; }
        public User LastExecutorCaseUser { get; set; }
        public DateTime LastExecutionDateTime { get; set; }
    }
}
