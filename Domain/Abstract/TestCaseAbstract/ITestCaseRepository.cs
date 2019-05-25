using System.Collections.Generic;
using Domain.Entities.TestCases;

namespace Domain.Abstract.TestCaseAbstract
{
    public interface ITestCaseRepository
    {
        IEnumerable<TestCaseEntity> TestCases { get; }
    }
}
