using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ITestResultRepository
    {
        IEnumerable<TestResult> TestResults { get; }
    }
}