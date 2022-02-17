using Microsoft.AspNetCore.Mvc.RazorPages;
using SmsTracker.Services;

namespace SmsTracker.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly TwilioService _twilioService;

    public IndexModel(ILogger<IndexModel> logger, TwilioService twilioService)
    {
        _logger = logger;
        _twilioService = twilioService ?? throw new ArgumentNullException(nameof(twilioService));
    }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        await _twilioService.SendSms("+17638071516", "Test from tracker.");
    }
}