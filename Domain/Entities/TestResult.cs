using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TestResult
    {
        //not executed,passed,failed,blocked
        public int TestResultId { get; set; }
        public string TestResultValue { get; set; }
        public string TestResultDescription { get; set; }
        public string TestResultColor { get; set; }
        //not executed - test case cannot be executed
        //passed, failed, blocked, error
        //blocked - A test case that cannot be executed because the preconditions for its execution are not fulfilled.
        //The result of error is used for situations where it is not clear whether the problem is in the test object.

    }
}
