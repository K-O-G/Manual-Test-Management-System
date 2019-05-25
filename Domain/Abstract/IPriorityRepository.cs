using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IPriorityRepository
    {
        IEnumerable<Priority> Priorities { get; }
    }
}