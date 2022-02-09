using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhoneNumbers;
using SmsTracker.Data;
using SmsTracker.Models;

namespace SmsTracker.Pages;

public class PhoneNumbers : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneNumbers(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [BindProperty] public NumberViewModel Number { get; set; }

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

            // TODO make sure changing the URL doesn't allow enumeration attacks.

            _dbContext.Numbers.Add(number);
            await _dbContext.SaveChangesAsync();
        }
        catch (NumberParseException npe)
        {
            ModelState.AddModelError(nameof(PhoneNumber), npe.Message);
            return Page();
        }

        return RedirectToPage(nameof(AccountManagement));
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
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? Notes { get; set; }
    }
}