using System.Collections.Generic;
using Domain.Abstract.TestCaseAbstract;
using Domain.Entities.TestCases;

namespace Domain.Concrete.TestCase
{
    public class EFCaseStepRepository:ICaseStepRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<CaseStep> CaseSteps => context.CaseSteps;
    }
}