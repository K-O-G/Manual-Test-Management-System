using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Concrete;
using Domain.Entities;

namespace Domain
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var ctx = new EFDbContext())
            {
                var component = new Component() { ComponentName = "VerificationStation" };

                ctx.Components.Add(component);
                ctx.SaveChanges();
            }
        }
    }
}

