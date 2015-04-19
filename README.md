# LocalSettings
Gets missing AppSettings values from an optional Settings File or Environment Variables. This is to work around the problem of storing secrets in Web.config
and app.config.


## Usage
`LocalSettings.Settings` returns a copy of the AppSettings for the application, augmented with values from a settings file or environment variable. 
**Note**: It will not mutate (change or add to) the AppSettings for the application.

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

And you have an Environment Variable named "AZURE_STORAGE_CONNECTION_STRING" which is set to "abcdef123456".

Then the contents of the `NameValueCollection` returned by `LocalSettings.Settings` will be:

```
	Scale.LocalSettings.File = ..\..\settings.xml
	AppSettingTest = AppSettingTestValue
	AZURE_STORAGE_CONNECTION_STRING = abcdef123456
	NoSettingTest =
```
