using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    public class LayoutController : Controller
    {
        public PartialViewResult PartialSideBar()
        {
            return PartialView();
        
        }

        public PartialViewResult PartialFooter()
        {
            return PartialView();
        }

        public PartialViewResult PartialHeader()
        {
            return PartialView();
        }
        public PartialViewResult PartialScripts()
        {
            return PartialView();
        }
        public PartialViewResult PartialNavbar()
        {
            return PartialView();
        }

        public PartialViewResult PartialRightBar()
        {
            return PartialView();
        }

        public PartialViewResult PartialThemeSettings()
        {
            return PartialView();
        }
    }
}
