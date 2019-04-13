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

## Communication Between Chrome Extension, Gmail, & Web Server
The process of obtaining a secure line of communication between the Chrome Extension, Gmail tab, and the Gmaster web server is very complicated. First of all, the Chrome Extension is broken up into two parts: **background script** & **content script**. The content script runs within the Gmail tab itself, and the background script runs like a background service within the Chrome web browser and has access to every content script instance within the Chrome Extension. Only the background script can create secure popup windows that can load a web page from the Gmaster web application, and only the background script can communicate with the content script loaded within the Gmail tab.

So, in order to successfully authenticate a **Google Sign-in** and pass a Gmaster **authentication key** to the content script for safe-keeping, Gmaster must follow the steps below:

1. **Background script** creates an **open connection** in Chrome
2. User opens Gmail tab and a **content script** loads, checking local storage for Gmaster **authentication key**.
   1. If **authentication key** exists, load Gmaster UI into Gmail
   2. If **authentication key** doesn't exist, follow next steps
3. **Content script** connects to **background script** via **open connection** and sends a command to background script asking to authenticate the user
4. **Background script** opens a new **popup window** that loads an **authentication web page** from the **Gmaster web application**
5. **Authentication web page** loads Google API via Javascript and a **Google Sign-In** window pops up asking the user to either log in or select a Google account to give Gmaster permission to access
6. Google returns a **code** back to Javascript within the **authentication web page**, which then gets sent to the **Gmaster web server** via an **AJAX** call
7. **Gmaster web server** uses Google **code** to retrieve an **access token** and **refresh token** from Google that allows Gmaster to call various Google APIs using the user's **Google credentials**. The refresh token is used to generate a new access token once the access token expires (and it expires after 1 hour). These tokens are saved into the **Gmaster database** and associated with the user's email address & internal userId
8. Gmaster also generates a unique **authentication key** (dubbed a *developer key* within the Gmaster-Server project) and sends the authentication key as the response to the **AJAX** call.
9. Javascript receives the **authentication key** via the **AJAX** response and sends a message to the **Chrome Extension** (by extension ID) along with the **authentication key**
10. The **background script** receives the **authentication key** provided by the Gmaster popup window and sends a response containing the authentication key to the **content script** loaded within the Gmail tab via the **open connection** established between the background script & content script
11. The **content script** finally saves the Gmaster **authentication key** into **local storage** that will be used to make AJAX requests to the **Gmaster Web API** for accessing the user's **Google credentials** and ultimately executing commands within the Gmail API on the user's behalf
12. If the Gmail tab is closed & reopened or refreshed, the **content script** will try to retrieve the Gmaster **authentication key** from local storage so that the user doesn't have to use the Google Sign-In popup window again.

## After The User Authenticates with Gmaster
After a new (or existing) user authenticates with the Gmaster web application using Google Sign-In, the content script will make an AJAX call to the **Gmaster Web API** to retrieve the user's **subscription plan** details from Gmaster. If the user doesn't have a subscription with Gmaster yet, the user will be shown a modal popup asking the user to select a subscription plan in order to activate the Gmaster features. If they agree to select a subscription plan, the modal popup will close and Gmail will load the **subscription plan page** within the body of Gmail's UI, keeping the user within Gmail. Also, a **Subscription Plans** menu item will appear within the Gmail navigation side-bar so that the user can navigate to their various inboxes and still come back to the subscription plan page when they are ready to pay for a subscription to Gmaster.

## Available Subscription Plans for Gmaster
There will be different subscription plan tiers based on various target audiences so that Gmaster can be affordable for all target audiences, whether it be a single entreprenuer or large-scale enterprise company. Below are a refined list of these subscription plans and the various features they support.

#### Beginner Plan ($9.95/month)
This plan is for the very basic user who wishes to send email campaigns to a moderate email list
* Send unlimited emails (based on Gmail daily limits)
* Contains Gmaster email signature

#### Daily Plan ($14.95/month)
For the daily user who needs to send personalized email campaigns without Gmaster branding, this is the ideal plan.
* Send unlimited emails (based on Gmail daily limits)
* Removes Gmaster email signature

#### Pro Plan ($24.95/month)
Professional users will need the ability to set up followup campaigns that are triggered based on customer response.
* Send unlimited emails (based on Gmail daily limits)
* Removes Gmaster email signature
* Setup Auto-Followup Campaigns
* Send Q&A Polls

#### Team Plans ($15.95/user/month)
Marketing teams will be able to achieve everything a professional user can do while collaborating on campaigns as a team.
* Send unlimited emails (based on Gmail daily limits)
* Removes Gmaster email signature
* Setup Auto-Followup Campaigns
* Send Q&A Polls
* Collaborate on Team Campaigns
* Plan starts at **5 users** @ **$79.95/month**

## Everyone Gets A Team
Even if a user signs up for an individual plan, a **Team** record is generated for them. This happens because the user can always upgrade to a team subscription. Also, this allows campaigns & address book entries to be created on a per-team basis instead of a per-user basis and enables collaboration between users who are a part of the same team.

## Subscription System
Users must be able to subscribe to Gmaster & manage their subscription from within Gmail. The following features must be available.
* **Subscribe** to any plan
* **Upgrade** current subscription (adding team members)
* **Downgrade** current subscription (from Team plan only)
* **Unsubscribe** from Gmaster
  * If the user unsubscribes from Gmaster within **5 business days** without launching any campaigns, the will receive a **full refund**.

## Team Security
The owner of the team can assign moderators & contributers to a team.
* **Moderators** can update permissions for specific Campaigns so contributers can only collaborate on specific campaigns
* **Contributers** simply collaborate on team campaigns and if they have permission, can create their own campaigns.

## Using Stripe API with Gmaster
Issues arise when trying to use the Stripe API with Gmaster's Chrome extension. The Stripe JavaScript library cannot be injected within the Gmail web page because the Chrome extension's content script cannot access the Stripe object loaded within the web page. The Stripe JavaScript library also cannot be loaded within the Chrome extension's background script because Stripe will raise an error claiming that the connection is not secure using HTTPS (the Chrome extension uses *chrome://* instead of *https://*). Thus, the Stripe API can only be loaded within a popup window loaded by the background script, and the popup window must load a secure web page from the Gmaster server. Communication between Stripe, the Gmaster web server, and the Chrome extension background script & content script will work very similar to the Google Sign-In system for Gmaster. 

1. User clicks **subscribe** button located within Gmaster's **Available Plans** page that was loaded within the Gmail web page
2. Modal popup appears asking user to select either Credit Card or PayPal
3. User selects **Credit Card** option
2. Chrome opens a popup window to *https://gmaster.datasilk.io/chrome/stripe*
2. User types in credit card information and clicks **Pay** button
3. Stripe JavaScript API sends info to Stripe server & returns a customer ID
4. Gmaster stores **Stripe customerID** in database associated with Gmaster user account
5. popup window closes and Modal popup within the Gmail web page displays payment & subscription details
6. Gmaster loads UI within Gmail so user can start using Gmaster features

## Reporting
Users will be able to view reports about a campaign after it has started running. 

#### Execution Report
Displays a set of statistics about a campaign.
* total emails added to campaign
* total emails sent
* total emails left in queue
* total emails bounced
* total emails retried to send
* total email retry fails
* total emails opened
* total email responses
* total emails sent to (specific) followup campaign 
* total (specific answer) to Q/A Poll

#### Timeline Report
Displays a set of time lapse charts that visualizes how a campaign was executed and how the users responded
* emails sent / bounced / retries / opened / responses
* followup campaign triggers
* each followup campaign (emails sent / bounced / retries / opened / responses)
* Q/A Poll questions answered
