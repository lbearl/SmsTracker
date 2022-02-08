# SmsTracker
An app to allow multiple people to track events via SMS

## Getting started 

You'll need Twilio credentials to get started, including a SID, Auth Token, and a phone number. Either add a Twilio item to the appsettings.json, or the user-secrets, or expose it as environment variables:

``` json
  "Twilio": {
    "AccountSid": "XXXXXX",
    "AuthToken": "XXXXXXXXX",
    "FromNumber": "+15555555555"
  }
```

If you don't have a Twilio account already, if you sign up here you get a $10 credit (and so do I): https://www.twilio.com/referral/L7yzRF

## Assumptions

 - Currently the app is NANP-centric, numbers outside of North America may or may not work (haven't tested it). 
 - The app is written tightly against Twilio, a future direction could be to put a facade in front of the Twilio logic and allow other providers (but I haven't seen any SMS providers that seem to have any compelling benefit over Twilio).
