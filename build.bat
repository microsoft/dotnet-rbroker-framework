@echo off

::Capture the current dir
set ROOT_DIR=%CD%

::specify the version of the library
set VERSION=7.4.0

::set the configuration (Release or Debug)
set CONFIG=Release



::
::
::DEPLOYR BROKER FRAMEWORK LIBRARY CONFIGURATION
::
::

::IMPORTANT!!!!
::set the location where referenced assemblies can be found (Broker depends on previously built deployr client library files)
::you will have to adjust this path as necessary
set BROKER_LIB_REF_PATH=%ROOT_DIR%\..\dotnet-client-library\release

::define output dir
set BROKER_LIB_OUTDIR=%ROOT_DIR%\release\

::define which version of msbuild we are going to use (Broker lib uses .NET 4.0)
set DOTNET_4.0_BUILD_EXE=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild 

::specify the location of the source solution file (i.e. DeployRBroker.sln)
set BROKER_LIB_SRC_DIR=%ROOT_DIR%\src

::specify broker zip command
set BROKER_ZIP_CMD=zip DeployR-DotNet-RBroker-%VERSION%.zip *.dll

::
::
::DEPLOYR BROKER FRAMEWORK LIBRARY BUILD
::
::

::delete the output directory
if exist %BROKER_LIB_OUTDIR% rmdir %BROKER_LIB_OUTDIR% /s /q

::build the library
cd %BROKER_LIB_SRC_DIR%
%DOTNET_4.0_BUILD_EXE% /p:Configuration=%CONFIG%;ReferencePath=%BROKER_LIB_REF_PATH%;OutDir=%BROKER_LIB_OUTDIR% /t:Clean;Rebuild

::create the zip file
if exist %BROKER_LIB_OUTDIR% (
	cd %BROKER_LIB_OUTDIR%
	%BROKER_ZIP_CMD%
)


::
::
::HELP FILE
::
::

::Broker lib Sandcastle Help File Builder project file
set BROKER_LIB_HELP_PROJECT=%ROOT_DIR%\help\DeployRBroker.shfbproj

:build broker lib help file
cd %BROKER_LIB_SRC_DIR%
%DOTNET_4.0_BUILD_EXE% /p:Configuration=%CONFIG%;ReferencePath=%BROKER_LIB_SRC_DIR%;OutDir=%BROKER_LIB_OUTDIR%;OutputPath=%BROKER_LIB_OUTDIR% %BROKER_LIB_HELP_PROJECT%

::reset the directory
cd %ROOT_DIR%

