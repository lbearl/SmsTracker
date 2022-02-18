using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using SmsTracker.Constants;
using SmsTracker.Data;
using SmsTracker.Models;
using SmsTracker.Validation;

namespace SmsTracker.Pages.Tracker;

public class AccountManagement : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public AccountManagement(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [FromRoute] public int? AccountId { get; set; }

    public bool IsNew { get; set; }

    [BindProperty] public AccountViewModel Account { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            User.Identity != null && x.Email == User.Identity.Name);
        if (user is null) throw new InvalidOperationException("Please log in!");

        var account = await _dbContext.Accounts.Include(x => x.AssociatedNumbers)
            .FirstOrDefaultAsync(x => x.Id == AccountId);

        if (account is null)
        {
            IsNew = true;
            account = new Account();
        }

        Account = new AccountViewModel
        {
            IsPrimary = account.IsPrimary,
            Prefix = account.Prefix,
            AccountName = account.AccountName,
            AssociatedNumbers = account.AssociatedNumbers.ToList(),
            Id = account.Id,
        };

        Account.AssociatedNumbers = PrettyPrintPhoneNumbers(Account.AssociatedNumbers).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Glue account information together 
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            User.Identity != null && x.Email == User.Identity.Name);
        if (user is null) throw new InvalidOperationException("User is not logged in");

        var accounts = _dbContext.Accounts.Include(x => x.AssociatedNumbers).Where( x=> x.OwnedByUserId == user.Id);

        // If we have something else that is marked as primary with a different AccountId, we have an issue.
        if (await accounts.AnyAsync(x => x.IsPrimary && Account.IsPrimary && x.Id != AccountId))
        {
            ModelState.AddModelError("Account.IsPrimary", "Can't have multiple primary accounts.");
            return Page();
        }

        var account = await accounts.FirstOrDefaultAsync(x => x.Id == AccountId);

        if (account is null)
        {
            IsNew = true;
            account = new Account();
        }

        await TryUpdateModelAsync(account, nameof(Account));
        // Make sure that the account's primary phone is in e164 format.
        account.OwnedByUser = user;
        account.OwnedByUserId = user.Id;

        if (IsNew)
            _dbContext.Accounts.Add(account);
        else
            _dbContext.Accounts.Update(account);

        await _dbContext.SaveChangesAsync();

        return RedirectToPage(NavigationConstants.TrackerPages.AccountManagement, new { AccountId = account.Id });
    }


    private IEnumerable<Number> PrettyPrintPhoneNumbers(List<Number> numbers)
    {
        var util = PhoneNumberUtil.GetInstance();
        foreach (var n in numbers)
        {
            var parsed = util.Parse(n.PhoneNumber, "US");
            n.PhoneNumber = util.Format(parsed, PhoneNumberFormat.NATIONAL);

            yield return n;
        }
    }

    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100), Display(Name = "Account Name")]
        public string AccountName { get; set; } = null!;

        public virtual List<Number> AssociatedNumbers { get; set; } = new List<Number>();

        [Display(Name = "Is Primary Account?")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Account Prefix (for inbound SMS)")]
        [MaxLength(5),
         RequiredIfFalse(nameof(IsPrimary), ErrorMessage = "Prefix must be set for non-primary accounts.")]
        public string? Prefix { get; set; }
    }
}