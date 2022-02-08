using Microsoft.Extensions.Options;
using SmsTracker.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SmsTracker.Services;

public class TwilioService
{
    private readonly IOptions<TwilioOptions> _twilio;

    public TwilioService(IOptions<TwilioOptions> twilio)
    {
        _twilio = twilio ?? throw new ArgumentNullException(nameof(twilio));
        
        TwilioClient.Init(twilio.Value.AccountSid, twilio.Value.AuthToken);
    }
    public async Task SendSms(string to, string message)
    {
        var msgResource = await MessageResource.CreateAsync(
            body: message,
            from: new PhoneNumber(_twilio.Value.FromNumber),
            to: new PhoneNumber(to));
    }
}