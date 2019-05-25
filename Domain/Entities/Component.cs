using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Component
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDescription { get; set; }
    }
}
