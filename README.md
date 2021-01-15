# Introduction 
This is the latest version (V4) of the SSI SiteManager quick start project template.  Based on .NET Core 3.1, Angular 10 and IdentityServer4/OIDC, this project is intended as a baseline architecture and development pattern for accelerating new projects.  

Project provides common core application features such as data access (EF Core Code First), Authentication/Authorization (IdentityServer4), Swagger/WebAPI layer, simple logging and an Angular 10 user interface.
 
Credit and thanks to [Jason Taylor](https://jasontaylor.dev/) for the work on his clean architecture solution template.

#Demo Site
The below demo site is part of the continuous deployment pipeline for ongoing development.  Features and function may differ from this documenation.

- **SitemanagerV4 Demo:** [http://test.socialslice.net/](http://test.socialslice.net/)
- **Username:** SiteManDemo
- **Password:** DemoPassword!123 

# Getting Started On Dev
To Get Started:
1.	Review the clean architecture design patterns at: 
	https://jasontaylor.dev/clean-architecture-getting-started/
2.	Ensure Node.js and Angular are installed
	See: https://angular.io/guide/setup-local
3.	Update WebUI/appsettings.json with intended database connection string.
	As of 01/2021, the project is configured to use a PostgreSQL backend but can be easily migrated to SQL Server or other database providers.  See persistence below. 
4.  Build and run the WebUI project. 

Recommendation: While developing, use VSCode/Node.js to "serve" the angular project (i.e. ng server) and update the Startup.cs to use a proxy while debugging
	Ex: spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");

#Functional Areas
Below are some additional notes on key areas of the project. 

##Persistence
Persistence is implemented using EF Core 3.1 with a code first pattern.  In keeping with the clean architecture pattern, context and configuration is managed as an infrastructure concern with entities within the domain layer. 

With this infrastructure/domain separation, conversion to other database base providers (e.g. SQL Server) is relatively simple.  While the details of implementing EF providers are out of the scope of this document, conversion should be a simple matter of referencing the providers, changing the connection string and updating column types in the EF configuration classes (Eg: 'nvarchar' (SQL Server) vs. 'text' (PostgreSQL)).

A "code first" pattern is being used to manage DB change management; however, scaffolding/reverse engineering an existing database is still supported.  Refer to [the documentation](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx) for context/entity output options that facilitate integration into the domain/infrastructure separation. 

###Identity Server
IdentityServer4 with OpenID Connect/JWT has been implemented within the infrastructure layer.  While, it is expected that most implementations of this project will integrate with an existing or 3rd party authentication system, a (mostly) boiler plate implementation of IdentityServer4 has been included for initial development.  If you intend to utilize the included identity provider, please review all middleware settings (e.g. scopes, claims, endpoints, etc.) prior to production usage. 

####Customizations
- Integer Keys:  All core identity tables have been converted to use <int> keys rather than the default GUID data types. 
- Table Names: Tables names for the identity infrastructure have been renamed to conform to the SiteManager "App" naming convention.  For example, the default "AspNetRole" table has been renamed to "AppRole".
- Claims Profile: In order to support the permissions based authorization system, a custom profile service (IdentityProfileService.cs) has been added to provide role claims to consuming applications. 

###Logging 
All logging is implemented via standard .Net Core logging and conforms to the ILogger pattern.  A starter logging provider (BasicSQLLogProvider.cs) has been included for development and can be used in production environments if no existing enterprise logging solution (e.g. appinsights) is available. 

Configuration and logging levels can be specified within the appsettings.json for each environment.  See [the logging documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0) for details information. 

Please note that the logging data context is independently managed within the provider; however, connection string/configuration is shared with the core infrastructure EF context.  It should be a trivial matter to separate these configurations if a custom/specific logging database is needed.

###NSwag OpenAPI
Swagger ([NSwag](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-5.0&tabs=visual-studio)) code generation and middleware has been used to expedite and standardize development.   Currently, both Swagger UI/API documentation and Angular client library generation are used.   Integration is transparent to developers and automatically generates typescript client libraries during build.

Please note that API Documentation is currently exposed within the app at: /api/index.html?url=/api/specification.json# **While the exposed API endpoints are secured with JWT bearer tokens; be sure to review for security and applicability prior to deploying the app to production.** 

Please note that the Angular/TypeScript client library generated by NSwag depends on the angular authorize interceptor for bearer security and claims. 

###CQRS/MediatR
The application uses the Command and Query Responsibility Segregation ([CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)) pattern from the [MediatR](https://github.com/jbogard/MediatR/wiki) project. This pattern provides a very simple and repeatable process that expedites development and increases overall code quality through separation of concerns.

Using the basic pipeline behaviour inherent with CQRS, the below "behaviours" have been included.  
- PerformanceBehaviour.cs: Monitors request/response pipeline for basic performance data and logs any request over 500ms.
- LoggingBehaviour.cs: (Disabled) Provides details request/response logging for troubleshooting.
- UnhandledExceptionBehaviour.cs: Catchall exception handler in core pipeline. 
- ValidationBehaviour.cs: Core request validation (fluent) mechanism for application request processing.  See request specific validators in each functional area. 

###User Interface
The included SPA interface is based on the [Ng-Matero](https://nzbin.gitbook.io/ng-matero/v/en/) admin template using [Angular Material](https://material.angular.io/) components.  This template is well documented and provides quite a few "nice to have" features such as dynamic menus, multi language/RTL support, SASS theming and scaffolding templates (schematics) for new routes and modules.  

The intention of this integration/implementation is to standardize front-end development using widely adopted and well documented patterns; however, any front-end can be used by addressing: 

- [OpenID/JWT](https://openid.net/what-is-openid/): If user authentication/authorization is required, support for JWT bearer authentication is needed.  Current front-end uses the [OIDC-Client](https://github.com/damienbod/angular-auth-oidc-client) for IdentityServer4 integration however any OpenID compliant solution should be supported.
- [OpenAPI](https://www.openapis.org/): Core WebAPI supports OpenID standards via NSWag.  Currently, NSwag also generates a standardized client access library in typescript.  API changes/models/etc are all automatically generated in the SiteManager.V4-api.ts file and can be used by any typescript client.

Ng-Matero Reference Sites:
- Project Home/Manual: [https://nzbin.gitbook.io/ng-matero/v/en/](https://nzbin.gitbook.io/ng-matero/v/en/)
- Demo & Reference: [https://ng-matero.github.io/ng-matero/auth/login](https://ng-matero.github.io/ng-matero/auth/login)

###Road Map
This public repository provides access to the latest stable (and publicly available) changes & hotfixes.  Ongoing daily development is on a member's only basis.  Current public feature road map is flexible and primarily driven by community need.  

Short Term
- .Net Core/EF Core 5 Update
- Additional Production Deployment Options/Documentation (Azure App Service, Docker, AWS)
- Dynamic Content Management & Forms Engine

Mid Term
- User Communication/Notification systems
- Additional authentication/authorization schemes (Active Directory, Google, ???)

###License
This project is licensed with the MIT license.






	
	






