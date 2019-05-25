using System.Collections.Generic;
using Domain.Abstract.TestCaseAbstract;
using Domain.Entities.TestCases;

namespace Domain.Concrete.TestCase
{
    public class EFTestCaseRepository :ITestCaseRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<TestCaseEntity> TestCases => context.TestCases;
    }
}