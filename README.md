# Packages

## GoogleApi

### GmailApi.Client

It wrapps [Google.Apis.Gmail](https://developers.google.com/api-client-library/dotnet/apis/gmail/v1) in the attempt to simplify server side authorization over Gmail.

**Security Notice**: Take note that by using this package the automatic authorization step (that opens a new window and asks for `Google` account credentials) is replaced by a manual step done apriori of using this package. Extra care is needed in keeping the autorization assets safe (`credentials.json` and `token.json`).

#### Usage - Authorization

1. Add `credentials.json` in the root of the console application linked to this package. This file can be obtained by:

- Following the initial steps from [Gmail API .NET Quickstart](https://developers.google.com/gmail/api/quickstart/dotnet), or
- Adding new credentials for your app in the [Google API Console](https://console.cloud.google.com/apis/credentials)

2. Run the console app. After you authorize the app to access your Google account, you will notice a new folder created on disk: `token.json` wich contains the Gmail auth token file.

3. Create a new `UserCredentialsProvider` and fetch the `UserCredential`:

```csharp
var settings = new AuthSettings
{
    ClientId = "{your app client id from credentials.json}",
    ClientSecret = "{your app client secret from credentials.json}",
    RefreshToken = "{your user refresh token from token.json}",
    Scopes = new[] { GmailService.Scope.GmailReadonly } // authorization scopes - in this case for Gmail
};
var credentialProvider = new UserCredentialsProvider(settings);

var userCredential = await credentialProvider.FetchAsync();
```

#### Usage - Extract email snippet content

```csharp
var gmailService = new GmailServiceWrapper("{your app name}", credential);

var request = new GmailRequestBuilder()
    .UseFrom("{from email address}")
    .UseSubject("{email subject}")
    .UseLabel("{email label/category}")
    .UseNewerThan(DateTime.Now.AddMinutes(-10))
    .UseSnippetRegex(new Regex(".*"))
    .Request;

var content = gmailService.ExtractEmailSnippetContent(request);
```
