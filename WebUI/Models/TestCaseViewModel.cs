using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    [NotMapped]
    public class TestCaseViewModel
    {
        public int TestCaseId { get; set; }
        public int CaseId { get; set; }
        public string CaseSummary { get; set; }
        public string CasePreconditions { get; set; }
        public List<CaseStepViewModel> CaseSteps { get; set; }
    }
    [NotMapped]
    public class CaseStepViewModel
    {
        public int CaseStepNumber { get; set; }
        public string CaseStepDescription { get; set; }
        public string CaseStepExpectedResult { get; set; }
    }
}