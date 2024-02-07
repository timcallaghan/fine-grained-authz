# Fine-grained Authorisation

A demonstration of different concepts in IAM and fined-grained authorisation (FGA) using an examle of moderate complexity.

## Motivation

In Identity and Access Management (IAM), open standards exist to clearly define the mechanisms that can identify who someone is and what they have access too. 
OpenID Connect (OIDC) and OAuth2 have risen to the fore as the de facto standard for IAM and have been highly successful in solving a subset of IAM. Specifically, these open standards have streamlined the process of authentication and high-level authorisation for the benefit of all.

By "high-level authorisation" I mean coarse-grain access controls.  Things like "does this user have access to this application", "is this service allowed to call this particular API" etc. High-level controls are good for a number of reasons:

* They allow applications/gateways to filter incoming requests and reject unauthorised access quickly
* They encourage centralisation of access control so that organisations can quickly audit who has access to what
* They allow the identity provider to efficiently reject access token requests when the user/service doesn't have access to the required OAuth2 scopes
* Access control is encoded in the access token, which can drastically simplify implementation
* They provide a simplified, top-level approach to authorisation and sometimes that is all that is required to enforce security

However, enforcing access control in real-world applications is rarely as simple as implementing high-level OAuth2 scopes. Applications usually contain resources, and those resources typically have an owner. Often the owner is not a simple as a single human, instead involving an inheritance chain controlling access through cascading organisational group memberships.

Users are typically members of an organisation and through operational roles they inherit access to subsets of the organisation's total resources. Sometimes they can access resources but in a limited capacity e.g. read-only, only resources associated with this business unit etc.

Additionally, resource data is often exposed via public APIs and accessed by other services (machine users). The owner of the resource might not want all of the resource's data (i.e. fields) to be available to 3rd party services.

All of the above hint at a need for fine-grain, low-level access controls, in addition to coarse-grain, high-level access controls. Historically each application has typically solved this problem independently and in isolation of other parts of the organisation's application ecosystem. This leads to increasingly complex access control audits and makes reasoning about who has access to specific resources, and in what capacity, a difficult task.

Rather than implementing a bespoke fine-grained access control system from scratch, we will instead work out a specific solution using [OpenFGA](https://openfga.dev/). See the [FGA solutions](./docs/fga_solutions.md) document for a review of the various FGA solution options available (as of Q1 2024), and reasoning about why OpenFGA was chosen.

## The Problem

We consider the problem of implementing IAM at the fictional pizza franchise company PizzaMea (yes, it helps if you say this in a thick Italian accent!).

PizzaMea consists of a governing corporate entity, and a couple of franchised stores located in Brisbane and Sydney.

![Org units](./docs/images/org_units.png)

The corporate entity houses the various c-suite roles typical of a company, as well as maintaining oversight of the national supply chain for all stores. Stores are run by local managers, with staff operating Point Of Sale (POS) equipment on-site.

![Org chart](./docs/images/org_chart.png)

The company has a significant investment in technology and operates a combination of 3rd-party software and infrastructure, as well as a number of internal and publicly facing applications.

Following best practice security, the company has decided to implement an IAM solution with the following properties:

1. Single Sign-on for anyone that needs to interact with the company's systems and data
2. Zero Trust principles, specifically:
    1. Everything requires authentication and authorisation
    2. Tight control over who has access to what
    3. Just-in-time authorisation for mutable role duties
    4. Minimsation of damage when breaches occur
3. Ability to partition access control decisions based on a number of factors, including:
    1. User and resource relationships
    2. Location
    3. Day and time

Our job is to propose an IAM solution that covers the above requirements, and then deliver a working proof-of-concept (PoC) of the proposal that demonstrates how the core requirements have been met.