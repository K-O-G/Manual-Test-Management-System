using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        // User rights -admin, test creator, test executor
        // может иметь одновременно несколько ролей, поэтому делается через bool
        public bool UserAdmin { get; set; }
        public bool UserTestCreator { get; set; }
        public bool UserTestExecutor { get; set; }
    }
}
