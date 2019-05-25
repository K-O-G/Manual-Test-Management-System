using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class ComponentController : Controller
    {
        // GET: Component
        private IComponentRepository componentRepository;
        public ComponentController(IComponentRepository repo)
        {
            componentRepository = repo;
        }

        public ViewResult List()
        {
            return View(componentRepository.Components);
        }
    }
}
