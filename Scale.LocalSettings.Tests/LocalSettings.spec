Summary
=======


Scenario
===============
|Given			| Load()							| 
|No setting		| AppSetting value is null			| 
|Value set		| AppSetting value equals value		| 
|Value changed	| AppSetting value equals new value | 
|App restarted	| AppSetting value has not changed? | 
|Value set		| Not called						| 

LoadSettings()
==============
| Reloading			| Setting already exists in original AppSettings.config
| false				| true,													| Don't reset AppSetting
| true				| true													| Don't reset AppSetting
| false				| false													| Set AppSetting
| true				| false													| Reset AppSetting
