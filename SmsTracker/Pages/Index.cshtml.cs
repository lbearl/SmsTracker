using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmsTracker.Constants;
using SmsTracker.Services;

namespace SmsTracker.Pages;

public class IndexModel : PageModel
{


    public IndexModel()
    {

    }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToPage(NavigationConstants.TrackerPages.Calendar);
        }

        return Page();
    }
}