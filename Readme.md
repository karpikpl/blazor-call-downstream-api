# About

## Blazor
Created with `dotnet new blazorserver -f net7.0 --auth SingleOrg --calls-graph --tenant-id 8ace3cb7-5c52-4b89-ba78-31a116187fb6`

To call external API: `dotnet new blazorserver -f net6.0 --auth SingleOrg --called-api-url https://api.apps.karpala.org:5002`.

https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-web-api-call-api-app-configuration?tabs=aspnetcore#option-2-call-a-downstream-web-api-other-than-microsoft-graph

* Add package `dotnet add package Microsoft.Identity.Web.DownstreamApi`
* Update all dependencies in blazor app for `"Microsoft.Identity` to `2.13.3` - the template has outdated ones

### Configuration

`appsettings.Development.json` file:

```json
{
  /*
  The following identity settings need to be configured
  before the project can be successfully executed.
  For more info see https://aka.ms/dotnet-template-ms-identity-platform 
  */
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "<< YOUR AD DOMAIN >>",
    "TenantId": "<< YOUR TENANT ID >>",
    "ClientId": "<< YOUR CLIENT ID FROM AAD APP REGISTRATION >>",
    "ClientSecret": "<< YOUR CLIENT SECRET FROM AAD APP REGISTRATION >>",
    "ClientCertificates": [],
    "CallbackPath": "/signin-oidc"
  },
  "DownstreamApi": {
    /*
       'Scopes' contains space separated scopes of the Web API you want to call. This can be:
        - a scope for a V2 application (for instance api:b3682cc7-8b30-4bd2-aaba-080c6bf0fd31/access_as_user)
        - a scope corresponding to a V1 application (for instance <App ID URI>/.default, where  <App ID URI> is the
          App ID URI of a legacy v1 Web application
        Applications are registered in the https:portal.azure.com portal.
      */
    "BaseUrl": "https://graph.microsoft.com/beta",
    "Scopes": "user.read"
  },
  "MyApi": {
    /*
       'Scopes' contains space separated scopes of the Web API you want to call. This can be:
        - a scope for a V2 application (for instance api:b3682cc7-8b30-4bd2-aaba-080c6bf0fd31/access_as_user)
        - a scope corresponding to a V1 application (for instance <App ID URI>/.default, where  <App ID URI> is the
          App ID URI of a legacy v1 Web application
        Applications are registered in the https:portal.azure.com portal.
      */
    "BaseUrl": "<< HTTPS URL FOR THE API e.g. https://api.apps.local.org:5002 >>",
    "Scopes": "<< REGISTERED SCOPE FOR THE API >>"
  },
  "AllowedHosts": "*",
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.AspNetCore.Server.Kestrel": "Information"
    }
  },
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "http://blazor.apps.local.org:5071"
      },
      "HttpsInlineCertAndKeyFile": {
        "Url": "https://blazor.apps.local.org:7297",
        "Certificate": {
            // cert file for *.apps.local.org domain
          "Path": "fullchain.pem",
          "KeyPath": "privkey.pem"
        }
      }
    }
  }
}
```

## Api
Created with `dotnet new webapi -f net7.0 -minimal --auth SingleOrg`

https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-app-configuration?tabs=aspnetcore#microsoftidentityweb

### Configuration

`appsettings.Development.json` file:

```json
{
  /*
  The following identity settings need to be configured
  before the project can be successfully executed.
  For more info see https://aka.ms/dotnet-template-ms-identity-platform
  */
    "AzureAd": {
      "Instance": "https://login.microsoftonline.com/",
      "Domain": "<< YOUR AD DOMAIN >>",
      "TenantId": "<< YOUR TENANT ID >>",
      "ClientId": "<< YOUR CLIENT ID FROM AAD APP REGISTRATION >>",
  
      "Scopes": "<< REGISTERED SCOPE FOR THE API >>",
      "CallbackPath": "/signin-oidc"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.AspNetCore.Server.Kestrel": "Information"
      }
    },
    "AllowedHosts": "*",
    "Kestrel": {
      "EndPoints": {
        "Http": {
          "Url": "http://api.apps.local.org:5000"
        },
        "HttpsInlineCertAndKeyFile": {
          "Url": "https://api.apps.local.org:5002",
          "Certificate": {
            // cert file for *.apps.local.org domain
            "Path": "fullchain.pem",
            "KeyPath": "privkey.pem"
          }
        }
      }
    }
  }
  
```

## About

* [Calling the API from the Blazor app.](https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-web-api-call-api-call-api?tabs=aspnetcore)