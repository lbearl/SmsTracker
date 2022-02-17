using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmsTracker.Data;

namespace SmsTracker.Pages.Tracker;

public class Calendar : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public Calendar(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public List<SelectListItem> Accounts { get; set; } = new();

    [FromRoute] public int AccountId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _dbContext.Users.FirstAsync(x => x.UserName == User.Identity!.Name);
        Accounts = await _dbContext.Accounts.Where(x => x.OwnedByUserId == user.Id).Select(x => new SelectListItem
        {
            Text = x.AccountName,
            Value = x.Id.ToString()
        }).ToListAsync();

        Accounts.Insert(0, new SelectListItem("All", 0.ToString()));

        return Page();
    }
}