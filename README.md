# Challengermode CS2 DefaultSkins

DefaultSkins is a simple but useful plugin for CS2 based on the awesome CounterStrikeSharp by roflmuffin. Sets player models to default skin for competitive play.

It also serves as example code for the community on how to use SetModel. Feel free to contribute and maintain it. It is intended to be light weight.

# Valve Server Guidelines
Make sure to read and comply with the [Server guidelines](https://blog.counter-strike.net/index.php/server_guidelines/) when using SetModel.

# Bump CCS version
```dotnet add package CounterStrikeSharp.API -v x.x.xx```

# Build
```dotnet build```

# Install steps
1. Install CounterStrikeSharp and Metamod on server
2. Copy .dll file from ```/bin/debug/net8.0``` and place it in ```cs2/addons/CounterstrikeSharp/Plugins/DefaultSkins/DefaultSkins.dll```

# How to use
```cm_default_skins 1``` to enable and ```cm_default_skins 0``` to disable the default skin enforcement.
```cm_heavy_model 1``` if you want to force heavy models on all of the players and ```cm_heavy_model 0``` stop using heavy models

# Credits
* Plugin Framework: CounterStrikeSharp by roflmuffin
* bober @ CounterStrikeSharp Discord for helping with signatures
* Metamod: https://www.sourcemm.net/


