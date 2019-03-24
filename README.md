# GMaster
A set of projects used for GMaster account management &amp; Web APIs

## App
An ASP.NET Core MVC Web API application used to be deployed via Docker on Amazon AWS and accessed via REST using the GMaster Chrome Extension within Gmail.

## Query
The Data Access Layer project that is a dependency of the App project, and accesses the Sql Server database via Dapper, retrieving POCO objects via T-SQL stored procedures.

## Sql
The Sql Server project used to configure & publish the Sql Server 2017 database for GMaster services on Amazon AWS RDS.

## Development Process
In order to build & test this application, various actions must be accomplished in a specific order. From an overall perspective, we must do the following:

1. Install Gmaster Chrome Extension
2. Sign Up for Gmaster via Gmail
3. Give Gmaster Google API access for Gmail
4. Test Gmaster features within Gmail!

This may sound simple, but there are many hoops to jump through. So far, I have found that I need to do the following:

1. Create Google/Gmail account
2. Sign up for **Google APIs**
3. Set Up [Google Sign-In for Server-Side Apps](https://developers.google.com/identity/sign-in/web/server-side-flow)
   1. Create new Gmaster user account based on Google User Account info, or...
   2. Sign into existing Gmaster account based on Google User Email address
   3. Generate developer key for user to store & use in Gmaster Chrome Extension
4. Create Credentials for Google APIs
   1. Set up **OAuth consent screen**
      1. verify Top-Level Domain `datasilk.io`
      2. create `gmaster.datasilk.io/privacy-policy` web page
      3. create `gmaster.datasilk.io/terms` web page
      4. publish `gmaster.datasilk.io` web site via Amazon AWS ECS (Docker)
      5. publish Gmaster Google Chrome Extension
   2. Set up **OAuth Client ID** credentials for **Chrome App**
   3. Set up **OAuth Client ID** credentials for **Web application**
5. Get Google to review my credentials request
