# GOGGiveawayNotifier

Same as below repos, a CLI tool fetches GOG home page and sends notifications through Telegram, Bark, Email, QQ, PushPlus and DingTalk.

Demo Telegram Channel [@azhuge233_FreeGames](https://t.me/azhuge233_FreeGames)

## Build

Install dotnet 6.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

Follow commands will publish project as a trimmed executable file.

```
git clone https://github.com/azhuge233/GOGGiveawayNotifier.git
cd GOGGiveawayNotifier
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64/...] --sc
```

## Usage

Fill your telegram bot token and chat ID in config.json first.

Check [wiki](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/Config-Description) for more explanations, only notify varaibles are available for this project.

To schedule the program, use cron.d in Linux(macOS) or Task Scheduler in Windows.

## My Free Games Collection

- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Archived)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)
- EpicBundle
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Archived)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/GOGGiveawayNotifier](https://github.com/azhuge233/GOGGiveawayNotifier)
- Ubisoft
    - [https://github.com/azhuge233/UbisoftGiveawayNotifier](https://github.com/azhuge233/UbisoftGiveawayNotifier)
