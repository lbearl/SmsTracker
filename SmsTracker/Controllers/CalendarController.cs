using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsTracker.Data;
using SmsTracker.Models;

namespace SmsTracker.Controllers;

public class CalendarController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public CalendarController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IActionResult> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end,
        [FromQuery] int accountId)
    {
        List<CalendarJson> allEvents;
        var user = await _dbContext.Users.FirstAsync(x => x.UserName == User.Identity!.Name);
        var events = _dbContext.TrackedItems.Include(x=>x.OwnedByAccount).Where(x=>x.OwnedByAccount.OwnedByUserId == user.Id);
        if (accountId > 0)
        {
            allEvents = await events.Where(x => x.OwnedByAccountId == accountId ).Where(x => x.CreatedOn >= start && x.CreatedOn <= end)
                .Select(x => new CalendarJson
                {
                    Title = x.Text,
                    Start = x.CreatedOn.ToString("O")
                }).ToListAsync();
        }
        else
        {
            allEvents = await events.Where(x => x.CreatedOn >= start && x.CreatedOn <= end)
                .Select(x => new CalendarJson
                {
                    Title = x.Text,
                    Start = x.CreatedOn.ToString("O")
                }).ToListAsync();
        }
        
            
        return Json(allEvents);
    }

    private class CalendarJson
    {
        public string Title { get; set; }
        public string Start { get; set; }
    }
}