# API Setup

We will model the various PizzaMea applications as APIs. 
Rather than building dedicated user interfaces, we'll instead lean on SwaggerUI as our poor-mans UI - the main benefit being that we can tightly integrate SwaggerUI with KeyCloak and test the various authentication and authorisation aspects directly, without having to build a custom UI.

Each API will have minimal resources - just enough to demonstrate the various IAM mechanisms.

Each API will also have a `/users/me` GET endpoint which will return the contents of the supplied JWT access_token.
This makes it easy to inspect the access token and validate the high-level access control.

All APIs are built on top of [ASP.NET Core 8](https://dotnet.microsoft.com/en-us/apps/aspnet/apis) and are containerised to enable easy local run/deploy.

## Swagger UI Customisations

We have configured Swagger UI (via SwashBuckle) to perform OIDC Authorization Code Flow with PKCE against the KeyCloak server running locally.
This means that it's easy to obtain a user-specific access_token for the API by clicking on the `Authorize` button at the top the Swagger UI page.
This then opens another browser tab and shows the KeyCloak login screen where the user enters their credentials.
After sign-in is successful, the browser tab re-directs back to the Swagger UI screen and stores the current authentication session details in local storage.

By default, when using the log-out feature of Swagger UI, only the local token data is removed.
This means that any session with the external IdP (in this case KeyCloak) remains, and if the user wants to get a new access token they don't have to sign in again.
However, this behaviour is potentially confusing and can also lead to situations where the IdP session remains and it becomes difficult to switch users when trying to test.

To get around this, the Swagger UI instance has been customised with a plugin that performs KeyCloak logout as well.
The idea for this was sourced from [here](https://gist.github.com/Fredx87/48fe741eed42efa4e77bd341745084a8), although had to be updated to make it work with more recent versions of KeyCloak.
Essentially, during logout we also open a new browser tab at the logout endpoint of the IdP and supply the post-logout redirect url and id_token hint.
Provided these are accepted, the IdP session is terminated which makes it easy to now log in as a different user by clicking the Swagger UI Authorize button.
