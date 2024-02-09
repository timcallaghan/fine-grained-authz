# Infrastructure and Applications Setup

We're going to need a number of things to implement the PoC. 

Things we'll need:

1. [Docker Compose](https://docs.docker.com/compose/) as the local orchestration tool
2. A dedicated docker network
3. A database server - we'll use [PostgreSQL](https://www.postgresql.org/)
4. A simple DB Admin tool - we'll use [pgAdmin](https://www.pgadmin.org/)
5. An Identity Provider - we'll use [KeyCloak](https://www.keycloak.org/)
6. An OpenTelemetry back-end for observing the system - we'll use [Seq](https://datalust.co/seq)
7. An FGA product - we'll use [OpenFGA](https://openfga.dev/)

All of these are provisioned locally in the [docker-compose-infra](../docker-compose-infra.yml) file.

One thing to note: Seq requires an admin password _hash_ to be supplied. We can generate this with the Seq docker container.

The [create-world](../create-world.sh) script handles the automation of generating this admin password hash and supplying it to Seq during startup.