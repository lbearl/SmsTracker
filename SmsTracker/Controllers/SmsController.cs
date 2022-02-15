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

        var account = await _dbContext.Numbers.Include(x=>x.Account).FirstOrDefaultAsync(x => x.PhoneNumber == from);

        if (account is null)
        {
            response.Message("This number isn't registered. Please register to proceed.");
        }
        else
        {
            // get the prefix (split on string and get first component, normalized to upper invariant case).
            var prefix = incomingMessage.Body.Split(' ')[0].ToUpperInvariant();
            var scopedAccount = await _dbContext.Accounts.Include(x => x.AssociatedNumbers)
                .FirstOrDefaultAsync(x => x.Prefix == prefix && x.OwnedByUserId == account.Account.OwnedByUserId);
            // we are in the primary account case if we can't resolve a prefix back to an account.
            if (scopedAccount is null)
            {
                response.Message("Got it! Added to tracking.");
                var trackedItem = new TrackedItem
                {
                    OwnedByAccount = account.Account,
                    OwnedByAccountId = account.Account.Id,
                    CreatedOn = DateTime.Now,
                    Text = incomingMessage.Body
                };

                _dbContext.TrackedItems.Add(trackedItem);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                response.Message("Got it! Added to tracking.");
                var trackedItem = new TrackedItem
                {
                    OwnedByAccount = scopedAccount,
                    OwnedByAccountId = scopedAccount.Id,
                    CreatedOn = DateTime.Now,
                    // Remove the prefix from the persisted message.
                    Text = incomingMessage.Body[prefix.Length..]
                };

                _dbContext.TrackedItems.Add(trackedItem);
                await _dbContext.SaveChangesAsync();
                
            }
        }

        return TwiML(response);
    }
}