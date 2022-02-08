namespace SmsTracker.Options;

public class TwilioOptions
{
    public const string Twilio = "Twilio";
    
    public string AccountSid { get; set; }
    
    public string AuthToken { get; set; }
    
    public string FromNumber { get; set; }
}