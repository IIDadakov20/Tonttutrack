using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tonttutrack.Web.Controllers;

[Authorize]
public class MapController : Controller
{
    public IActionResult MapTrackerLayout()
    {
        return View("Views/Map/_MapTrackerLayout.cshtml");
    }
}
