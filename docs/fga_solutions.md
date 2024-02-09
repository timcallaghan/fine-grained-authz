# Open source FGA solutions

There are a number of products that aim to solve the FGA problem. In common with all is the desire to centralise the fine-grained access control decision logic, and have applications call into the decision engine to resolve access requests in real-time. Let's look at a few.

## Open Policy Agent (OPA)

[OPA](https://www.openpolicyagent.org/) (pronounced "o-pah") uses a policy approach to solving FGA which hinges on [Role Based Access Control (RBAC)](https://www.openpolicyagent.org/docs/latest/comparison-to-other-systems/#role-based-access-control-rbac) and [Attribute Based Access Control (ABAC)](https://www.openpolicyagent.org/docs/latest/comparison-to-other-systems/#attribute-based-access-control-abac) concepts. [OPA's readme](https://github.com/open-policy-agent/opa?tab=readme-ov-file#how-does-opa-work) highlights the following benefts

* OPA targets cloud-native environments and champions the idea of decoupling access policies from service logic
* Services integrate with OPA so that policies/rules do not have to be hardcoded into application code
* OPA defines a high-level declarative language called [Rego](https://www.openpolicyagent.org/docs/latest/policy-language/) (pronounced "ray go") to author and enforce a wide variety of policies/rules
* OPA is a Cloud Native Computing Foundation (CNCF) _graduated_ project and has been used in various production environments for many years
* The licence model is Apache-2.0
* OPA is not specifically for FGA, but is more generally used for all manner of policy enforcement e.g. 
    * Can user X call operation Y on resource Z?
    * What tags must be set on resource R before it's created?
    * What clusters should workload W be deployed to?
    * Is protocol http exposed on server S?
* It is technically possible to model [Relationship Based Access Control (ReBAC)](https://www.permit.io/blog/relationship-based-access-control-rebac-with-open-policy-agent-opa) in OPA, however [Oso](https://www.osohq.com/) claims OPA can be [prohibitively slow](https://www.osohq.com/cloud/authorization-service) when using OPA for normal application workloads i.e. there is a preference to use it primarily for infrastructure access policies where the number of object, actors, and rules is minimal

See the [introduction](https://www.openpolicyagent.org/docs/latest/) for a detailed explanation of what OPA has to offer. 

## Google Zanzibar derivatives

Google solved FGA at scale, and [published a paper](https://research.google/pubs/pub48190/) about the design and technology which they named Zanzibar. The approach used in Zanzibar is primarily one of Relationship Based Access Control (ReBAC) where access control is defined in terms of the relationships between resources and users/groups. Fundamentally, access control rules are stored in large graphs and checking access reduces to checking graph node connectedness. It has been battle-tested at Google for many years and underpins the authorisation model for many of Google's services e.g. Drive, Calendar, YouTube etc.

After the publication of the Zanzibar paper, numerous open and closed source solutions have emerged that publicly acknowledge Zanzibar's influence on their designs. For example:

* [OpenFGA](https://openfga.dev/) is developed by Okta/Auth0
    * A variant of OpenFGA is used as the basis for [Auth0's fine grained authorization](https://auth0.com/fine-grained-authorization) paid offering
    * The licence is Apache-2.0 and permits commercial use
    * Possible to self-host locally or in the cloud
    * Has a comprehensive API
    * There are SDK wrappers around the API for Java, Node.js, JavaScript, GoLang, Python, and .NET
* [Permify](https://www.permify.co/) offers a basic OSS free version or a paid, managed (hosted) service with many more features
    * Possible to self-host locally or in the cloud
    * The licence is Apache-2.0 and permits commercial use
    * Has a comprehensive API
* [Oso](https://www.osohq.com/) offers a hosted service with support for ReBAC and optionally RBAC/ABAC
    * Has its own modelling language called Polar
    * Has a comprehensive API
    * There are SDK wrappers around the API for Java, Node.js, Ruby, GoLang, Python, and .NET
* [Ory](https://www.ory.sh/permissions/) offers enterprise-grade permissions management
    * Has its own modelling language called [Ory Permissions Language](https://www.ory.sh/what-is-the-ory-permission-language/)
    * Has an OSS version called [Ory Keto](https://github.com/ory/keto)
    * Has a paid, hosted offering
    * The licence is Apache-2.0 and permits commercial use
    * Was the first OSS Zanzibar derivative and as such has [large adoption](https://github.com/ory/keto?tab=readme-ov-file#whos-using-it)
* [Permit.io](https://www.permit.io/) offers full-stack authorization as a service
    * Supports ReBAC, RBAC, ABAC
    * Policy rules are based on OPA's Rego or AWS's Cedar
    * Provides an OSS tool called [Opal](https://github.com/permitio/opal) which is an administration layer for policy engines like [OPA and Cedar Agent](https://github.com/permitio/opal?tab=readme-ov-file#what-is-opal)
    * Offers a hosted service
* [KeyCloak](https://www.keycloak.org/) supports FGA via Authorization Services
    * A number of [different access control](https://www.keycloak.org/docs/latest/authorization_services/index.html) models are supported
    * Fully OSS and has extensive plugins to accomplish a variety of tasks
    * Relies on self-hosting but includes documentation on how to run in production, including scaling and active-passive failover

No doubt there are other offerings that can be added to the above list.

# FGA provider choice recommendations

Of the different offerings identified above, OpenFGA appears to be a good choice because:

* It's backed by a large, global company (Okta/Auth0) that specialises in IAM
* It's used in production at Auth0
* You can use it locally, which means developers don't need to be connected to the internet to be able to run their solutions that have integrated with OpenFGA
* None of the features are gated behind a tiered SaaS pricing model i.e. you can use everything out of the box locally or hosted
* It's got HTTP and gRPC APIs
* It's got a broad selection of SDKs for a variety of languages
* The documentation is comprehensive and includes guides on modelling relationships
* There's a growing list of [community projects](https://github.com/openfga/community?tab=readme-ov-file#community-projects) that extend OpenFGA
* OpenFGA is a CNCF Sandbox project

Note tha OPA is also an attractive option, especially when considering an entire organisational ecosystem. However, at present the application-specific modelling process appears to be more mature in OpenFGA. Likely a [combination of the two approaches](https://www.linkedin.com/pulse/external-authorization-using-istio-opa-chaitanya-tyagi/) would yield the most fleixble system, albeit one with more moving parts.

For an all-in-one solution, KeyCloak is very appealling. It offers a central place to manage authentication and authorisation, including specifying high-level and low-level access controls.

This is not to say that the various hosted options are not without merit. Depending on the problems being solved, a hosted option may be entirely adequte. Many of the hosted options offer highly functional dashboards, which would likely become a necessity at scale i.e. millions of relationship access control records. In addition, not having to maintain infrastructure and SLAs should not be overlooked as a driving reason to adopt a hosted SaaS FGA solution.
