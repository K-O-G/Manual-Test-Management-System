using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.UI;
using System.Web.ModelBinding;
using System.IO;
using Domain.Concrete;
using Domain.Entities;

namespace WebUI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                Component component = new Component();

                // Получить данные из формы с помощью средств
                // привязки моделей ASP.NET
                IValueProvider provider =
                    new FormValueProvider(ModelBindingExecutionContext);
                if (TryUpdateModel<Component>(component, provider))
                {
                    
                    
                    // В этой точке непосредственно начинается работа с Entity Framework

                    // Создать объект контекста
                    EFDbContext context = new EFDbContext();

                    // Вставить данные в таблицу Customers с помощью LINQ
                    context.Components.Add(component);

                    // Сохранить изменения в БД
                    context.SaveChanges();
                }
            }
        }
    }
    
}