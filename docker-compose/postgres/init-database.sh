#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER openfga WITH PASSWORD 'password';
    CREATE DATABASE openfga;
    GRANT ALL PRIVILEGES ON DATABASE openfga TO openfga;
    CREATE USER keycloak WITH PASSWORD 'password';
    CREATE DATABASE keycloak;
    GRANT ALL PRIVILEGES ON DATABASE keycloak TO keycloak;
EOSQL
