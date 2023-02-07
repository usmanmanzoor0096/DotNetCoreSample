# Auth-Service - [Swagger](http://localhost:3000/swagger/index.html)
It is a very simple Dot Net Core application, where I covered basic Dot Net Identity authentication operations, along with Cloud services. I used N-Tier Architecture to develop this application which provides us with the following benefits Secure, Ease of management, Scalability, and flexibility. As far as the Design patterns are concerns I used Repository Pattern for adding my all business knowledge in a single place which factors in a service layer.

### Auth Service API’S are responsible to perform following activities.

# Account.
 * **POST**: Update User Profile 
 * **POST**: Update User Wallet information 
 * **POST**: Update User public profile 
# Auth
* **POST**: Login 
* **POST**: Get New Access Token (When token get expired!!)
* **POST**: Register Client 
* **POST**: Register Admin (only Super Admin can have capability to create admins) 
* **POST**: Register User 
* **POST**: Find user information by email.
* **PUT**:  Set Password
* **PUT**:  Change Password 
* **POST**: Forgot Password 
* **POST**:  Validate Token 
* **POST**:  Active/Inactve User 
 
## Main System Components:
* **Cloud Flare**: Cloud flare may be used to have easy access to HTTPS and a certificate, and also to prevent DDOS.
* **Web Servers**: Are the list of web servers that run the Bloomly web application and serve user HTTP requests.
* **Cache**: A cache is used to store frequently requested data. All data that is changed has to have its corresponding cached data invalidated.
* **Relational Database**: Stores all data that is non-critical, and also serves as a persistent “second layer” caching mechanism for data stored inside the blockchain.  All data changed in the blockchain has to have its corresponding copy in the database updated.
* **CDN**: Stores images for quicker retrieval.

# Tech Stack
Given below is the tech stack we have used in our application. For development on the server side, 
  * We are using [asp.net](http://asp.net) core.
  * For the database, I am using SQL server. 

### Cloud Databases:
- [AWS](https://aws.amazon.com/products/?nc2=h_ql_prod&aws-products-all.sort-by=item.additionalFields.productNameLowercase&aws-products-all.sort-order=asc&awsf.re%3AInvent=*all&awsf.Free%20Tier%20Type=*all&awsf.tech-category=*all) Databases for MSSQL Server - Amazon Web Services, Inc. is a subsidiary of Amazon that provides on-demand cloud computing platforms and APIs to individuals, companies, and governments, on a metered pay-as-you-go basis. These cloud computing web services provide distributed computing processing capacity and software tools via AWS server farms.
- Regular backups configured on the cloud

### ORM
* [Entity fram work](https://learn.microsoft.com/en-us/ef/core/) v-6.0.6 - Entity Framework (EF) Core is a lightweight, extensible, open source and cross-platform version of the popular Entity Framework data access technology. 

### Queue:
- SQS - For sending emails to end users, initiated by each micro service.

### File Storage: 
- [AWS S3 Bucket](https://aws.amazon.com/s3/) - Amazon Simple Storage Service (Amazon S3) is an object storage service offering industry-leading scalability, data availability, security, and performance. Customers of all sizes and industries can store and protect any amount of data for virtually any use case, such as data lakes, cloud-native applications, and mobile apps. With cost-effective storage classes and easy-to-use management features, you can optimize costs, organize data, and configure fine-tuned access controls to meet specific business, organizational, and compliance requirements.

### Security:
* [Dot Net Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio) - Manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more. 
* [JWT](https://jwt.io/) Cookies base authentication: The server side can send the JWT token to the browser through a cookie, and the browser will automatically bring the JWT token in the cookie header when requesting the server-side interface, and the server side can verify the JWT token in the cookie header to achieve authentication.

### Logging: 
* [Logger](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line) - Logger is used for creating customized error log files or an error can be registered as a log entry in the Windows Event Log on the administrator's machine.  
* [SEQ](https://datalust.co/seq) - Seq accepts logs via HTTP, GELF, custom inputs, and the seqcli command-line client, with plug-ins or integrations available for .NET Core, Java, Node.js, Python, Ruby, Go, Docker, message queues, and many other technologies.

### Unit Test:
* [xUnit.net](https://www.nuget.org/packages/xunit.analyzers) is a free, open source, community-focused unit testing tool for the .NET Framework. Written by the original inventor of NUnit v2, xUnit.net is the latest technology for unit testing C#, F#, VB.NET and other .NET languages. xUnit.net works with ReSharper, CodeRush, TestDriven.NET and Xamarin. It is part of the .NET Foundation, and operates under their code of conduct. It is licensed under Apache 2 (an OSI approved license).

### Code Analyzer
* [Microsoft.CodeAnalysis.Analyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.Analyzers/) Analyzers for consumers of Microsoft.CodeAnalysis NuGet package, i.e. extensions and applications built on top of .NET Compiler Platform (Roslyn). This package is included as a development dependency of Microsoft.CodeAnalysis NuGet package and does not need to be installed separately if you are referencing Microsoft.CodeAnalysis NuGet package.
* [xunit.analyzers](https://www.nuget.org/packages/xunit.analyzers) - xUnit.net is a developer testing framework, built to support Test Driven Development, with a design goal of extreme simplicity and alignment with framework features. Installing this package provides code analyzers to help developers find and fix frequent issues when writing tests and xUnit.net extensibility code.


<!-- [GitHub ](https://pages.github.com/) -->
