# Dependency Injection in Azure Functions using Microsoft.Extensions.DependencyInjection

A Microsoft.Extensions.DependencyInjection implementation based on the 
Autofac based implmenetation of CJ van der Smissen [AzureFunctions.AutoFac](https://github.com/introtocomputerscience/azure-function-autofac-dependency-injection) 


[AzureFunctions.AutoFac on NuGet](https://www.nuget.org/packages/AzureFunctions.Autofac)

[AzureFunctions.Extensions.DependencyInjection on NuGet](https://www.nuget.org/packages/AzureFunctions.Extensions.DependencyInjection)

## Roadmap & Updates
**1.1.0**
- Multi Target Frameworks : NET462, NET471 and NETSTANDARD2.0

**1.0.1**
- License Added

**1.0.0**
- Initial working version based on the referenced implementation.
---

## Usage
In order to implement the dependency injection you have to create a class to configure DependencyInjection and add an attribute on your function class.

### Configuration
Create a class and call the DependencyInjection.Initialize method. Perform the registrations as you normally would with Autofac.
```c#
public class Startup
{
    public Startup()
    {
        if(!DependencyInjection.IsInitialized)
        {
            DependencyInjection.Initialize(ConfigureServices);

            // Some other stuff such as AutoMapper.
            Mapper.Initialize(c =>
            {
                AzureFunctionsAutoMapperConfig.Initialize(c);
                BusinessAutoMapperConfig.Initialize(c);
            });
        }            
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddBusinessLayer();
    }
}
```
### Function Attribute and Inject Attribute
Once you have created your Startup or Config class you need to annotate your function class indicating which config to use and annotate any parameters that are being injected. 
> Note: All injected parameters must be registered with the `IServiceCollection` in order for this to work.
```c#
[ConfigureServices(typeof(Startup))]
public class GreeterFunction
{
    [FunctionName("GreeterFunction")]
    public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage request, 
        TraceWriter log, 
        [Inject]IGreeter greeter, 
        [Inject]IGoodbyer goodbye)
    {
        log.Info("C# HTTP trigger function processed a request.");
        return request.CreateResponse(HttpStatusCode.OK, $"{greeter.Greet()} {goodbye.Goodbye()}");
    }
}
```
### ~~Using Named Dependencies~~ [Unsupported]
~~Support has been added to use named dependencies. Simple add a name parameter to the Inject attribute to specify which instance to use.~~
```c#
[ConfigureServices(typeof(Startup))]
public class GreeterFunction
{
    [FunctionName("GreeterFunction")]
    public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage request, 
        TraceWriter log, 
        [Inject]IGreeter greeter, 
        [Inject("Main")]IGoodbyer goodbye, // Unsupported
        [Inject("Secondary")]IGoodbyer alternateGoodbye // Unsupported
    )
    {
        log.Info("C# HTTP trigger function processed a request.");
        return request.CreateResponse(HttpStatusCode.OK, $"{greeter.Greet()} {goodbye.Goodbye()} or {alternateGoodbye.Goodbye()}");
    }
}
```


