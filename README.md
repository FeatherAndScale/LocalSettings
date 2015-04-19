# LocalSettings
Loads AppSettings values from an optional Settings File and/or from Environment Variables. Works around the problem of storing secrets in Web.config
and App.config for .NET OSS projects.

## Installation
`nuget install Scale.LocalSettings`

## Usage
`LocalSettings.Settings` returns a copy of the `AppSettings` for the application, augmented with values from a settings file or environment variable.
LocalSettings will only return values for keys that exist in the AppSettings config section. Values are returned in the following order of precedence:
1. AppSettings config section. 2. Settings file specified by the Scale.LocalSettings.File appSetting. 3. Environment Variable.
**Note**: _LocalSettings will not mutate (change or add to) the applciation's AppSettings._

```csharp
    // Get AppSettings and replace missing values with values from an optional Settings File or Environment Variables (in that order of precedence).
	var settings = LocalSettings.Settings;

	// Refresh the settings from the file and environment variables at any time.
	LocalSettings.Refresh();
```

### Example
Given that your _Web.config_ or _App.config_ file contains:

```xml
    <appSettings>
      <add key="Scale.LocalSettings.File" value="..\..\settings.xml"/>
      <add key="AppSettingTest" value=""/>
      <add key="AZURE_STORAGE_CONNECTION_STRING" value=""/>
      <add key="NoSettingTest" value=""/>
    </appSettings>
```

And you have a _settings.xml_ file in your Project that contains:

```xml
    <appSettings>
	  <add key="TestSetting" value="TestValue" />
      <add key="AppSettingTest" value="AppSettingTestValue" />
    </appSettings>
```

And you have an Environment Variable named _"AZURE_STORAGE_CONNECTION_STRING"_ which is set to _"abcdef123456"_.

Then the contents of the `NameValueCollection` returned by `LocalSettings.Settings` will be:

```
	Scale.LocalSettings.File = ..\..\settings.xml
	AppSettingTest = AppSettingTestValue
	AZURE_STORAGE_CONNECTION_STRING = abcdef123456
	NoSettingTest =
```

## Help
Create an Issue or contact @DanielLarsenNZ if you have any queries. Contributions welcomed.