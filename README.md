[![Version](https://img.shields.io/static/v1?label=version&message=2.2.1&color=blue)](https://bitbucket.org/dobriyanchik/unicorntaf/src/unicorn-2.2.1/)
[![Coverage](https://img.shields.io/static/v1?label=coverage&message=58%&color=silver)](https://bitbucket.org/dobriyanchik/unicorntaf/src/master/)
[![Licence](https://img.shields.io/static/v1?label=license&message=Apache-2.0&color=green)](https://www.apache.org/licenses/LICENSE-2.0)


Unicorn test automation framework
=============================

Content
------------

The following files and directories are presented:

      example/         code with TAF usage examples
	  src/             framework core sources
	  LICENSE.txt	   licence file
	  README.md        this file


TAF Solution Structure
=============================


Unicorn.Taf.Core
------------
Core of unicorn automation framework.

* Unit test framework implementation
* Logger abstractions
* Base utilities

Unicorn.ReportPortalAgent
------------
Ready for use realization of EPAM ReportPortal extension.

Unicorn.UI.Core
------------
UI core of unicorn automation framework.

* Driver, PageObject and search context abstractions
* UI matchers
* User input clients (mouse, keyboard)

Unicorn.UI.Win
------------
Implementation of desktop GUI interaction which is based on UIA3 library.

* GUI Driver implementation
* Typified controls implementations
* PageObject implementation
* UI actions

Unicorn.UI.Win
------------
Implementation of desktop GUI interaction which is based on UIA3 library.

* Web Driver implementation
* Typified controls implementations
* PageObject implementation

Unicorn.UI.Desktop
------------
Implementation of desktop GUI interaction which is based on UIAutomation library.

* GUI Driver implementation
* Typified controls implementations
* PageObject implementation
* UI actions

Unicorn.UI.Mobile
------------
Implementation of Mobile UI interaction which is based on Appium.

* Web Driver implementation (iOS, Android)
* Typified controls implementations (iOS, Android)
* PageObject implementation (iOS, Android)