> **Note** This repository is developed using .net framework 4.5, .netstandard2.0, .netstandard2.1, net5.0, netcoreapp3.1.

[![NuGet Version](https://img.shields.io/nuget/v/MLogHelperDotNet.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MLogHelperDotNet/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/MLogHelperDotNet.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MLogHelperDotNet)


One important reason for developing this repository is to quickly implement the governmental logging service provided by [e-governance agency](https://egov.md/), named `MLog`, available in the Republic of Moldova.<br/>

The current repository appears as a result of several implementations in projects from scratch, losing a lot of time and desire for a more easy way of implementation in new projects.
This repository is a wrapper for the currently available service. Using a few configuration parameters from the application settings file `appsettings.json`, `app.config`, or `web.config` you may implement them very easily into your own application.<br/>
Using the wrapper you will no longer be forced to install the application certificate on the current machine/server.
<br/>

Available configuration settings are: 
* `ServiceClientAddress` -> `MLog` service URL;
* `ServiceCertificatePath` -> Service/application certificate for `MLog` (file with *.pfx at the end);
* `ServiceEventRegisterPath` -> Service/application registration log path;
* `ServiceEventGetPath` -> Service/application query log path;
* `ServiceTimeoutInMinute` -> Service/application request execution timeout.

For more information about that, follow the info from using doc.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/MLogHelperDotNet" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package MLogHelperDotNet -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)