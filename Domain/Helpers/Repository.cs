using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Tokenizer;
using System.Web.WebPages;
using Domain.Entities;

namespace Domain.Helpers
{
    public static class Repository
    {
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session["CurrentUser"] != null)
                    return HttpContext.Current.Session["CurrentUser"] as User;
                throw new UnauthorizedAccessException();
            }
            set { HttpContext.Current.Session["CurrentUser"] = value; }
        }
    }
}
