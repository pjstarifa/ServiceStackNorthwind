# ServiceStack.Northwind

## Todo
1. Get rid of model classes that have been replaced by the T4 generated classes
1. Get the client packages for Twitter Bootstrap, AngularJS, etc

## Done
1. Changed database to SQL Server
1. Added T4 templates to generate classes
1. Updated solution to .NET 4.5 to get T4 template generation to work
1. Added id identity column to OrderDetails table to resolve multiple index issue
1. Installed Swagger plugin but am getting repeated services
1. Installed the git extensions for VS2K12U2

## Issues
1. Why are classes not being generated as partial? 
1. OrmLite.Poco.tt looks as if it does this but the results are not partials!