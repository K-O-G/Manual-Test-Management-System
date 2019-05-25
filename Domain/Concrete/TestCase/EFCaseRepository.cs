using System.Collections.Generic;
using Domain.Abstract.TestCaseAbstract;
using Domain.Entities.TestCases;

namespace Domain.Concrete.TestCase
{
    public class EFCaseRepository:ICaseRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Case> Cases => context.Cases;
    }
}