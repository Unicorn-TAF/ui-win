[![Licence](https://img.shields.io/static/v1?label=license&message=Apache-2.0&color=white&style=plastic)](https://www.apache.org/licenses/LICENSE-2.0)
[![Pipelines](https://img.shields.io/bitbucket/pipelines/dobriyanchik/unicorntaf/master?style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/pipelines)
[![Coverage](https://img.shields.io/static/v1?label=coverage&message=73%&color=green&style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/src/master/)
[![Issues](https://img.shields.io/bitbucket/issues/dobriyanchik/unicorntaf?style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/issues?status=new&status=open)  

**Nuget**  
[![TafApiVersion](https://img.shields.io/static/v1?label=Taf.Api&message=1.0.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.Taf.Api/)
[![TafCoreVersion](https://img.shields.io/static/v1?label=Taf.Core&message=3.1.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.Taf.Core/)
[![BackendVersion](https://img.shields.io/static/v1?label=Backend&message=2.0.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.Backend/)
[![UiCoreVersion](https://img.shields.io/static/v1?label=UI.Core&message=3.1.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.UI.Core/)
[![UiWebVersion](https://img.shields.io/static/v1?label=UI.Web&message=3.1.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.UI.Web/)
[![UiWinVersion](https://img.shields.io/static/v1?label=UI.Win&message=3.0.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.UI.Win/)
[![UiMobileVersion](https://img.shields.io/static/v1?label=UI.Mobile&message=1.0.0&color=blue&style=plastic)](https://www.nuget.org/packages/Unicorn.UI.Mobile/)  

Unicorn test automation framework
=================================

Content
-------

The following files and directories are presented:

      example/         			code with TAF usage examples
	  src/             			framework core sources
	  bitbucket-pipelines.yml	YAML of the CI pipeline
	  icon.png					icon file
	  LICENSE.txt	   			licence file
	  README.md        			this file


Solution Structure
======================

Unicorn.Taf.Api
---------------
Common API for core of unicorn automation framework.

* Common APIs
* AppDomain isolation
* Unicorn AssemblyLoadContext

Unicorn.Taf.Core
----------------
Core of unicorn automation framework.

* Unit test framework implementation
* Base tests runners
* Logger abstractions
* Base utilities
* Asserts and base matchers

Unicorn.Backend
---------------
Implementation of clients for interaction with services based on `HttpClient`.

* Client for REST services
* REST service matchers collection

> TBD: Client for SOAP services  
> TBD: batabase client

Unicorn.UI.Core
---------------
UI core of unicorn automation framework.

* Driver, PageObject and search context abstractions
* UI matchers

Unicorn.UI.Win
--------------
Implementation of desktop GUI interaction based on UIA3 library.

* GUI Driver implementation
* Typified controls implementations
* PageObject implementation
* UI actions
* User input clients (mouse, keyboard)

Unicorn.UI.Web
--------------
Implementation of browser interaction based on Selenium.

* Web Driver implementation
* Typified controls implementations
* PageObject implementation
* Abstract WebSite and pages pool

Unicorn.UI.Mobile
-----------------
Implementation of Mobile UI interaction which is based on Appium (for Android).

* Web and App Driver implementation
* Typified controls implementations
* PageObject implementation