# ASP.NET Core Web Crawler

This is an open source, multi-threaded and stateless website crawler written in C# / ASP.NET Core, persisting in IBM's Cloudant NoSQL DB and configured for a Linux Docker image.

[![Deploy to Bluemix](https://bluemix.net/deploy/button.png)](https://bluemix.net/deploy?repository=https://github.com/renanbm/webcrawler)

## Run the app locally

1. Install ASP.NET Core and the Dotnet CLI by following the [Getting Started][] instructions
+ Clone this app
+ cd into the app directory and then `src/WebCrawler.Spider.Web`
+ Copy the value for the VCAP_SERVICES envirionment variable from the application running in Bluemix and paste it in the vcap-local.json file
+ Run `dotnet restore`
+ Run `dotnet run`
+ Access the running app in a browser at http://localhost:63939

[Getting Started]: http://docs.asp.net/en/latest/getting-started/index.html
