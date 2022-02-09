using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmsTracker.Data;
using SmsTracker.Models;

namespace SmsTracker.Pages;

public class AccountOverview : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public AccountOverview(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public List<Account> Accounts { get; set; }
    
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _dbContext.Users.FirstAsync(x => User.Identity != null && x.Email == User.Identity.Name);

        Accounts = await _dbContext.Accounts.Where(x => x.OwnedByUserId == user.Id).ToListAsync();
        
        return Page();
    }
}