using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.TestCases
{
    public class CaseStep
    {

        public int CaseStepId { get; set; }
        public int? CaseId { get; set; }
        public Case Case { get; set; }
        public int CaseStepNumber { get; set; }
        public string CaseStepDescription { get; set; }
        public string CaseStepExpectedResult { get; set; }
        public  TestResult CaseStepResult { get; set; }
    }
}
