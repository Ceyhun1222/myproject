using Microsoft.AspNetCore.Mvc;

namespace ObstacleManagementSystem.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        public FooterViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
