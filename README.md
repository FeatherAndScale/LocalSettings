# LocalSettings
Gets missing AppSettings values from an optional Settings File or Environment Variables. This is to work around the problem of storing secrets in Web.config
and app.config.

## Usage
```csharp
    // Get AppSettings and replace missing values with values from an optional Settings File or Environment Variables (in that order of precedence).
	var settings = LocalSettings.Settings;

	// Refresh the settings from the file and environment variables at any time.
	LocalSettings.Refresh();
```
