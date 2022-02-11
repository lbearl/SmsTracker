using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using SmsTracker.Data;
using SmsTracker.Exceptions;
using SmsTracker.Models;

namespace SmsTracker.Pages.Tracker;

public class PhoneNumbers : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneNumbers(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [BindProperty] public NumberViewModel Number { get; set; } = null!;

    [FromRoute] public int AccountId { get; set; }


    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var number = new Number
            {
                PublicName = Number.Name,
                PhoneNumber = GetE164FormattedNumber(Number.PhoneNumber),
                Notes = Number.Notes,
                AccountId = AccountId
            };

            var duplicate = await _dbContext.Numbers.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.PhoneNumber == number.PhoneNumber);
            if (duplicate is not null)
            {
                var user = await _dbContext.Users.FirstAsync(x => x.UserName == User.Identity!.Name);
                // Need to handle the scenario where this number is registered by someone else (by disallowing)
                if (duplicate.Account.OwnedByUserId != user.Id)
                    throw new DuplicateNumberException();
            }

            _dbContext.Numbers.Add(number);
            await _dbContext.SaveChangesAsync();
        }
        catch (NumberParseException npe)
        {
            ModelState.AddModelError(nameof(PhoneNumber), npe.Message);
            return Page();
        }
        catch (DuplicateNumberException)
        {
            ModelState.AddModelError(nameof(PhoneNumber), "This number is already registered by someone else.");
            return Page();
        }

        return RedirectToPage(nameof(AccountManagement), new {AccountId});
    }

    private string GetE164FormattedNumber(string input)
    {
        var util = PhoneNumberUtil.GetInstance();

        var normalized = PhoneNumberUtil.Normalize(input);
        var converted = util.Parse(normalized, "US");

        return util.Format(converted, PhoneNumberFormat.E164);
    }

    public class NumberViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;
        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Notes"), DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
    }
}