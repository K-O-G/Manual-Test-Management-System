using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Concrete;
using Domain.Entities;
using Domain.Entities.CheckLists;
using Domain.Entities.TestCases;
using Domain.Helpers;

namespace Domain.Concrete
{
    public class EFDbContext : DbContext
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<CheckListEntity>()
                .HasMany<Component>(s => s.Components)
                .WithMany(c => c.CheckLists)
                .Map(cs =>
                {
                    cs.MapLeftKey("CheckListId");
                    cs.MapRightKey("ComponentId");
                    cs.ToTable("CheckList_Component");
                });

            modelBuilder.Entity<TestCaseEntity>()
                .HasMany<Component>(s => s.Components)
                .WithMany(c => c.TestCases)
                .Map(cs =>
                {
                    cs.MapLeftKey("TestCaseId");
                    cs.MapRightKey("ComponentId");
                    cs.ToTable("TestCase_Component");
                });

            modelBuilder
                .Entity<Case>()
                .HasOptional<User>(u => u.LastExecutorCaseUser)
                .WithOptionalPrincipal();

            modelBuilder
                .Entity<CheckListItem>()
                .HasOptional<User>(u => u.LastExecutorCheckListUser)
                .WithOptionalPrincipal();

            modelBuilder.Entity<Case>()
                .HasRequired<TestCaseEntity>(s => s.TestCase)
                .WithMany(g => g.Cases)
                .HasForeignKey<int>(s => s.TestCaseId);

            modelBuilder.Entity<CheckListItem>()
                .HasRequired<CheckListEntity>(s => s.CheckListEntity)
                .WithMany(g => g.CheckListItems)
                .HasForeignKey<int>(s => s.CheckListId);
        }
    }
}

public class TestInitializerDb : DropCreateDatabaseIfModelChanges<EFDbContext>
{
    protected override void Seed(EFDbContext context)
    {
        TestResult testResult = new TestResult
        {
            TestResultId = 1, TestResultValue = "Not Running",
            TestResultDescription = "Test was not running", TestResultColor = "#454545"
        };
        Priority priority = new Priority
        {
            PriorityId = 1, PriorityName = "Minor",
            PriorityDescription =
                "In this category, all the suggestions, and small UI changes or product improvements will be included. They will not affect the software usage in anyway and can be avoided if there is tight deadline.",
            PriorityColor = "#00ac00"
        };
        string pass = new UserSecurity().CalculateMD5Hash("12345");
        User user = new User
        {
            UserId = 1, UserName = "Admin", UserEmail = "akatyryna@ukr.net", UserPassword = pass, UserAdmin = true,
            UserTestCreator = true, UserTestExecutor = true
        };
        User user1 = new User
        {
            UserId = 2, UserName = "V_I_I", UserEmail = "irina.vitkovska@livenau.net", UserPassword = pass,
            UserAdmin = true, UserTestCreator = true, UserTestExecutor = true
        };
        context.TestResults.Add(testResult);
        context.Priorities.Add(priority);
        context.Users.Add(user);
        context.Users.Add(user1);
    }
}
