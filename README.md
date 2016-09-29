# LocalSettings
Loads AppSettings values from an optional Settings File and/or from Environment Variables. Works around the problem of storing secrets in Web.config
and App.config for .NET OSS projects.

## TL;DR
1. `PM> Install-Package Scale.LocalSettings`
1. Add your secret appSetting to **web.config**, but leave the value empty.
1. Add to appSettings: `<add key="Scale.LocalSettings.File" value="settings.xml" />`
1. Create **settings.xml** in web root. Copy your secret appSettings with values into that file.
1. `var settings = LocalSettings.Settings;`


## Installation
`nuget install Scale.LocalSettings`


## About
`LocalSettings.Settings` returns a copy of the `AppSettings` for the application, augmented with values from a settings file or environment variable.
LocalSettings will only return values for keys that exist in the `AppSettings` config section. Values are returned in the following order of precedence:

1. `AppSettings` config section. 
2. Settings file specified by the *Scale.LocalSettings.File* appSetting. 
3. Environment Variable.

Azure Web App Settings override web.config appSettings.

**Note**: _LocalSettings will not mutate (change or add to) the application's AppSettings._


### Settings XML file
The path and filename of a Settings file should be specified in the Application's Web.config or App.config file, in an AppSetting named 
*Scale.LocalSettings.File*. It can be any name and in any location that is available to the app. Paths are relative to 
`AppDomain.CurrentDomain.BaseDirectory`. In a Web Application this is the Web root folder. In a Unit Test or Console Application, this is *bin\Debug*.

The format of the file should be the same as the `<appSettings>` XML configuration section, e.g.

```xml
	<?xml version="1.0" encoding="utf-8" ?>
    <appSettings>
	  <add key="YourSettingName" value="YourSettingValue" />
	  <!-- ... -->
    </appSettings>
```


### Environment Variables
Any Process, User or Machine Environment Variable can be used as a setting. You can set a User Environment Variable with the following PowerShell command. 
You must restart any process that needs to access the variable before use.

```PowerShell
    [Environment]::SetEnvironmentVariable("YourEnvVarName", "YourEnvVarSetting", "User")
```


### Usage
`Settings` is a Property of the static `LocalSettings` class. It is populated with settings before the Property is accessed. You can refresh the
settings at anytime by calling the `Refresh` method. This will reload settings from the settings file and Environment Vars, but does not currently refresh
keys and values from the Application's web.config or app.config file.

```csharp
	using Scale;

    // Get AppSettings and replace missing values with values from an optional Settings File or Environment Variables (in that order of precedence).
	var settings = LocalSettings.Settings;

	// Refresh the settings from the file and environment variables at any time.
	LocalSettings.Refresh();
```


### Example
Given that your *Web.config* or *App.config* file contains:

```xml
    <appSettings>
      <add key="Scale.LocalSettings.File" value="..\..\settings.xml"/>
      <add key="AppSettingTest" value=""/>
      <add key="AZURE_STORAGE_CONNECTION_STRING" value=""/>
      <add key="NoSettingTest" value=""/>
    </appSettings>
```

And you have a *settings.xml* file in your Project that contains:

```xml
    <appSettings>
	  <add key="TestSetting" value="TestValue" />
      <add key="AppSettingTest" value="AppSettingTestValue" />
    </appSettings>
```

And you have an Environment Variable named *AZURE_STORAGE_CONNECTION_STRING* which is set to *abcdef123456*.

Then the contents of the `NameValueCollection` returned by `LocalSettings.Settings` will be:

```
	Scale.LocalSettings.File = ..\..\settings.xml
	AppSettingTest = AppSettingTestValue
	AZURE_STORAGE_CONNECTION_STRING = abcdef123456
	NoSettingTest =
```

You can now inject settings into classes that support having settings injected into them, rather than reading directly from 
`System.Configuration.ConfigurationManager.AppSettings`, e.g.

```csharp
	using Scale;
	using Scale.Storage.Blob;

	// ...

	var storage = new AzureBlobStorage(LocalSettings.Settings);
	return await storage.List("photos", true);
```

If you .gitignore your Settings File and don't include it in your .csproj or .sln file, then this file will remain local and will not be deployed. 
In Azure you can use App Configuration Settings to override the web.config appSettings. In other environments you can use [Config Transforms].


## Help
Create an Issue or contact @DanielLarsenNZ if you have any queries. Contributions welcomed.


[Config Transforms]: http://www.codeproject.com/Tips/559849/Transform-Web-Config-when-Deploying-a-Web-Applicat
