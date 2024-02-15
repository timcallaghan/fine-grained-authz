# KeyCloak Configuration

## Configuration overview

The KeyCloak instance is pre-configured with a number of things by importing a realm during container startup. Specifcally:

1. An Admin user with username `admin` and password `password
2. A realm called `pizzamea`
3. 14 users corresponding to the users in the [Org Chart](./images/org_chart.png)
   1. The password for all users is `password`
   2. The login name is of the form `{first_name}.{last_name}@pizza.mea` e.g. `dan.scott@pizza.mea`
4. A number of Groups the represent the different logic groupings within the PizzaMea organisation
5. OIDC Clients for the 6 applications listed in the [main readme](../README.md)
   1. These are pre-configured with logon/logout redirect urls, scopes, and the `restricted-access` role (see below)
6. Custom audience claim mappers that ensure each OIDC client (and backing API) has a distinct `aud` claim in issued access tokens
7. A custom login flow that ensures only users with the `restricted-access` role assigned can log in to specific applications (more on this below)

## Per-client user restrictions

By default, KeyCloak doesn't have any built-in mechanisms for restricting which users are allowed to sign-in (authenticate) against specific OIDC clients.
The reason given is that this should be a client application concern i.e. post-authentication authorization logic.

However, this is at odds with a number of commercial Identity Provider offerings and overlooks the fact that being able to not even issue tokens if the user is not allowed to authenticate against the client application is a front-door security measure i.e. Why should my application even have to process a request if the user is not allowed to access anything in the application?
Can't we off-load this responsibility to the point-of-authentication and reduce the load on our application?
Okta supports this concept through group membership and Sign-On policies attached to OIDC clients.
Similar concepts are available in Azure AD and other IdPs. 
As such, it's desirable to be able to restrict _authentication_ outcomes based on things like group membership in KeyCloak.

Thankfully, there is an [open-source plugin for KeyClock](https://github.com/sventorben/keycloak-restrict-client-auth) that enables this functionality.
The KeyCloak instance has been configured with this plugin loaded, and has been extended by defining a new browser-based authentication flow that also ensures the user is assigned to the client via a special per-client role.

We have then configured `Role Mappings` on each group that assign the `restricted-access` role to any user belonging to the group.
In this way, it's easy to control who has sign-in rights for each separate OIDC client.

## KeyCloak Urls

| Url                                                                 | Purpose                                                                                |
|---------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| [Admin Console](http://localhost:5010/admin/)                       | Administration of the KeyCloak instance                                                |
| [Account Console](http://localhost:5010/realms/pizzamea/account/#/) | Individual user account management - can also be used to perform SSO, including logout |
