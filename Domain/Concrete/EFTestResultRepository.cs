using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFTestResultRepository:ITestResultRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<TestResult> TestResults => context.TestResults;
    }
}