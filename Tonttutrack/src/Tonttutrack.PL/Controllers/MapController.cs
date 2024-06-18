using Microsoft.AspNetCore.Mvc;

namespace Tonttutrack.PL.Controllers;

public class MapController : Controller
{
    public IActionResult MapTrackerLayout()
    {
        return View("Views/Map/_MapTrackerLayout.cshtml");
    }
}
