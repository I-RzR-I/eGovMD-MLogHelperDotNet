> :point_up: From the beginning let's make clear one moment. It is a wrapper for logging service provided by [e-governance agency](https://egov.md/). If you accessed this repository and already installed it or you want or install the package you already contacted the agency and discussed that and obtained the necessary information for clean implementation. :muscle:

<br/>

Currently is available only `REST` logging implementation. `SOAP` implementation will be added in the future.

In curret wrapper are available a few methods like:
* `RegisterEvent/Async`;
* `RegisterEventBatch/Async`;

For more details, please check the documentation obtained from the responsible authorities where you can find all the smallest details necessary for the implementation and understanding of the working flow.
<br/>
In dependece of what type of project you have, in you configuration file please provide available configurable parameters.<br/>

<hr/>

**Configure the application settings file**

In case you use `netstandard2.0`, `netstandard2.1`, `net5`, `netcoreapp3.1` in your project find a settings file like `appsettings.json` or `appsettings.env.json` and complete it with the following parameters.
```json
  "RemoteMLogOptions": {
    "ServiceClientAddress": "logging service",
    "ServiceCertificatePath": "service certificate",
    "ServiceCertificatePassword": "password for service certificate",
    "ServiceEventRegisterPath": "registration log path",
    "ServiceEventGetPath": "query log path",
    "ServiceTimeoutInMinute": "request execution timeout"
  }
```

If you have the `app/web.config` file (must common for .net framework projects)
```xml
  <configSections>
    <section name="MLogConfigurationSection"
             type="MLogHelperDotNet.Helpers.ConfSection.MLogConfigurationSection,MLogHelperDotNet" />
  </configSections>

  <MLogConfigurationSection>
    <RemoteMLogOptions>
      <option key="ServiceClientAddress" value="logging service" />
      <option key="ServiceCertificatePath" value="service certificate" />
      <option key="ServiceCertificatePassword" value="password for service certificate" />
      <option key="ServiceEventRegisterPath" value="registration log path" />
      <option key="ServiceEventGetPath" value="query log path" />
      <option key="ServiceTimeoutInMinute" value="request execution timeout" />
    </RemoteMLogOptions>
  </MLogConfigurationSection>
```

<hr/>

**Calling the service**

In case of using the `netstandard2.0+` in your project, after adding configuration data, you must set dependency injection for using functionality. In your project in the file `Startup.cs` add the following part of the code:
```csharp
public void ConfigureServices(IServiceCollection services)
        {
            ...
            
            services.AddMLogService(Configuration);
            
            ...
        }
```

In the service that will be implemented, inject internal service (`IMLogEndpointClientService`).
```csharp
public class Logging
    {
        private readonly Func<EndpointType, IMLogEndpointClientService> _clientFactory;

        public Logging(Func<EndpointType, IMLogEndpointClientService> clientFactory)
        {
             _clientFactory = clientFactory;
        }
        
        public IActionResult SendSoap()
        {
            var endpoint = _clientFactory(EndpointType.REST);
            var result = endpoint.RegisterEvent("JSON content");

            return JsonResult(result);
        }
        ...
    }
```

**Net Framework** <br/>
```csharp
var logInstance = new MLogRestClientService();
var log = logInstance.RegisterEvent(new MLogEventDto()
         {
             EventCorrelation = Guid.NewGuid().ToString(),
             EventID = Guid.NewGuid().ToString(),
             EventTime = DateTime.Now,
             Subject = "test"
         });
         
 //----------------------------------------
 var log = logInstance.RegisterEvent("JSON content");
```


For more information about the fields and how to complete them, you can find in integration guide obtained from the service provider.




