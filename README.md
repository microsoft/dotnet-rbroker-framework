.NET RBroker Framework for DeployR
==================================

The .NET RBroker Framework provides a simple yet powerful API that supports the
rapid integration of R Analytics inside any .NET application. Simply define an
RTask, submit your task to an instance of RBroker and be notified when
your RTaskResult is available.

The framework scales effortlessly to support simple integrations through
sophisticated solutions such as high throughput, realtime scoring engines.

Links
-----

  * [Download](http://deployr.revolutionanalytics.com/docanddown/#rbroker)
  * [Quick Start Tutorial](http://deployr.revolutionanalytics.com/documents/dev/rbroker)
  * [Framework API .NETDoc](http://deployr.revolutionanalytics.com/documents/dev/rbroker-.NETdoc)
  * [Framework Dependencies](#dependencies)
  * [Example Code](#examples)
  * [License](#license)

Dependencies
============

Bundled DLL Dependencies
------------------------

Besides the DeployR .NET RBroker Framework DLL itself, `DeployRBroker<version>.dll`,
the framework depends on the
[DeployR .NET Client Library](https://github.com/deployr/dotnet-client-library)
and all of it's third party DLL dependencies.

Building the .NET RBroker Framework
-----------------------------------

The DeployR .NET RBroker Framework was created with Visual Studio 2010, using .NET Framework 4.0.

A successful build will result in the creation of `DeployR-DotNet-RBroker-<version>.zip` file containing the 
DeployR RBroker Framework library and it's dependencies.  It will also create a `DeployRBroker.chm` help file.

Please review the variables in the `build.bat` file and adjust as necessary for your environemnt.  The one
variable that requires special attnetion is `BROKER_LIB_REF_PATH`.  This variable points to the directory 
containing the previosuly built DeployR client library `DeployR<version>.dll`

Examples
========

The DeployR .NET RBroker Framework ships with a number of sample applications
provided to demonstrate some of the key featues introduced by the
[Quick Start Tutorial](http://deployr.revolutionanalytics.com/documents/dev/rbroker)
for the .NET client library. See
[here](examples/tutorial) for details.

Help
====

The DeoployR .NET help file is built with Sandcastle Help File Builder.
The file "deployR.shfbproj", is a "Sandcastle Help File Builder" project file

[Sandcastle Help File Builder](http://shfb.codeplex.com/)

License
=======

Copyright (C) 2010-2014 by Revolution Analytics Inc.

This program is licensed to you under the terms of Version 2.0 of the
Apache License. This program is distributed WITHOUT
ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING THOSE OF NON-INFRINGEMENT,
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. Please refer to the
Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0) for more 
details.