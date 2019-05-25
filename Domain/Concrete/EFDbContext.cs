using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;
using Domain.Entities.CheckLists;
using Domain.Entities.TestCases;

namespace Domain.Concrete
{
    public class EFDbContext :DbContext
    {
        public EFDbContext() : base("name=EFDbContext")
        {

        }
        public DbSet<Component> Components { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CheckListEntity> CheckLists { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }
        public DbSet<TestCaseEntity> TestCases { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseStep> CaseSteps { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}
