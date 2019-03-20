# GMaster
A set of projects used for GMaster account management &amp; Web APIs

## Services
An ASP.NET Core MVC Web API application used to be deployed via Docker on Amazon AWS and accessed via REST using the GMaster Chrome Extension within Gmail.

## Query
The Data Access Layer project that is a dependency of the Services project, and accesses the Sql Server database via Dapper, retrieving POCO objects via T-SQL stored procedures.

## Sql
The Sql Server project used to configure & publish the Sql Server 2017 database for GMaster services on Amazon AWS RDS.
