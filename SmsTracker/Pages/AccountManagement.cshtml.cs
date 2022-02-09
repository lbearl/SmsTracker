using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using SmsTracker.Data;
using SmsTracker.Models;

namespace SmsTracker.Pages;

public class AccountManagement : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public AccountManagement(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [FromRoute] public int? AccountId { get; set; }

    public bool IsNew { get; set; } = false;

    [BindProperty] public AccountViewModel Account { get; set; } = new AccountViewModel();

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
            
            AccountName = account.AccountName,
            AssociatedNumbers = account.AssociatedNumbers.ToList(),
            Id = account.Id,
        };

        PrettyPrintPhoneNumbers(Account.AssociatedNumbers);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Glue account information together 
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            User.Identity != null && x.Email == User.Identity.Name);
        if (user is null) throw new InvalidOperationException("User is not logged in");

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.OwnedByUserId == user.Id) ?? new Account();

        await TryUpdateModelAsync(account, nameof(Account));
        // Make sure that the account's primary phone is in e164 format.
        account.OwnedByUser = user;
        account.OwnedByUserId = user.Id;

        if(IsNew)
            _dbContext.Accounts.Add(account);
        else
            _dbContext.Accounts.Update(account);
        
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(nameof(AccountManagement), new { AccountId = account.Id });
    }


    private List<Number> PrettyPrintPhoneNumbers(List<Number> numbers)
    {
        var util = PhoneNumberUtil.GetInstance();
        foreach (var n in numbers)
        {
            var parsed = util.Parse(n.PhoneNumber, "US");
            n.PhoneNumber = util.Format(parsed, PhoneNumberFormat.NATIONAL);
        }

        return numbers;
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(100)] public string AccountName { get; set; }

        public virtual List<Number> AssociatedNumbers { get; set; } = new List<Number>();
    }
}