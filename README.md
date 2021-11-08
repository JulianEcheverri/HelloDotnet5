# HelloDotnet5

## From
- https://www.youtube.com/watch?v=MIJJCR3ndQQ

###Creating a project
 - dotnet new webapi -o HelloDotnet5 --no-https
- api key = 315a974a1c2d6bb270d22c5677765efb

### For secrets key management in dotnet5
dotnet user-secrets init
dotnet user-secrets set ServiceSettings:ApiKey (ServiceSettings is the class that should match: ApiKey the property) 315a974a1c2d6bb270d22c5677765efb

### Adding nuget package for handling failured request (without connection)
dotnet add package Microsoft.Extensions.Http.Polly
dotnet restore
