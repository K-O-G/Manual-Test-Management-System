using System.Collections.Generic;
using Domain.Entities.TestCases;

namespace Domain.Abstract.TestCaseAbstract
{
    public class ICaseRepository
    {
        IEnumerable<Case> Cases { get; }
    }
}
