using System.Collections.Generic;
using Domain.Entities.TestCases;

namespace Domain.Abstract.TestCaseAbstract
{
    public interface ICaseStepRepository
    {
        IEnumerable<CaseStep> CaseSteps { get; }
    }
}
