using Microsoft.EntityFrameworkCore;
using SmsTracker.Data;
using SmsTracker.Models;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace SmsTracker.Controllers;

public class SmsController : TwilioController
{
    private readonly ApplicationDbContext _dbContext;

    public SmsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    // GET
    public async Task<TwiMLResult> Index(SmsRequest incomingMessage)
    {
        var response = new MessagingResponse();
        
        // this is an e.164 number
        var from = incomingMessage.From;

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.PrimaryPhone == from);

        if (account is null)
        {
            response.Message("This number isn't registered. Please register to proceed.");
        }
        else
        {
            response.Message("Got it! Added to tracking.");
            var trackedItem = new TrackedItem
            {
                OwnedByAccount = account,
                OwnedByAccountId = account.Id,
                CreatedOn = DateTime.Now,
                Text = incomingMessage.Body
            };

            _dbContext.TrackedItems.Add(trackedItem);
            await _dbContext.SaveChangesAsync();
        }

        return TwiML(response);
    }
}