using Microsoft.AspNetCore.Mvc;
using SmsTracker.Constants;
using SmsTracker.Data;

namespace SmsTracker.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public AccountController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteNumber([FromQuery]int numberId)
    {
        var number = await _dbContext.Numbers.FindAsync(numberId);
        
        if (number is null) return NotFound("Number with specified Id could not be found.");
        var accountId = number.AccountId;
        _dbContext.Numbers.Remove(number);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(NavigationConstants.TrackerPages.AccountManagement, new {accountId});
    }
}