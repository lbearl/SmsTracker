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

    [BindProperty] public AccountViewModel Account { get; set; } = new AccountViewModel();

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            User.Identity != null && x.Email == User.Identity.Name);
        if (user is null) throw new InvalidOperationException("Please log in!");

        var account = await _dbContext.Accounts.Include(x=>x.AssociatedNumbers).FirstOrDefaultAsync(x => x.OwnedByUserId == user.Id) ?? new Account();

        Account = new AccountViewModel
        {
            AccountName = account.AccountName,
            AssociatedNumbers = account.AssociatedNumbers.ToList(),
            Id = account.Id,
            PrimaryPhone = account.PrimaryPhone
        };
        
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
        account.PrimaryPhone = GetE164FormattedNumber(Account.PrimaryPhone);
        account.OwnedByUser = user;
        account.OwnedByUserId = user.Id;

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private string GetE164FormattedNumber(string input)
    {
        var util = PhoneNumberUtil.GetInstance();

        var normalized = PhoneNumberUtil.Normalize(input);
        var converted = util.Parse(normalized, "US");

        return util.Format(converted, PhoneNumberFormat.E164);
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string AccountName { get; set; }
    
        [Phone, Required]
        public string PrimaryPhone { get; set; }
    
        public virtual List<Number> AssociatedNumbers { get; set; } = new List<Number>();
    }
}