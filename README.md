# GOGGiveawayNotifier

A CLI tool
- Fetches GOG giveaway and free game news
- Sends free game notifications to Telegram, Bark, Email, QQ, PushPlus, DingTalk, PushDeer, Discord and MeoW.
- Auto claim GOG giveaway by carrying given cookies and sending GET request to [https://www.gog.com/giveaway/claim](https://www.gog.com/giveaway/claim)
  - Different than regular free games, giveways have their own special banners in GOG home page.

Demo Telegram Channel [@azhuge233_FreeGames](https://t.me/azhuge233_FreeGames)

## Build

Install dotnet 10.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

Follow commands will publish project as a executable file.

```
git clone https://github.com/azhuge233/GOGGiveawayNotifier.git
cd GOGGiveawayNotifier
dotnet publish -c Release -p:PublishDir=/your/path/here -r [win-x64/osx-x64/linux-x64/...] --sc
```

## Usage

Set your telegram bot token and chat ID in config.json first.

Check [wiki](https://github.com/azhuge233/GOGGiveawayNotifier/wiki/Config-Description) for more explanations.

### Repeatedly running

The program will not add while/for loop, it's a scraper. To schedule the program, use cron.d in Linux(macOS) or Task Scheduler in Windows.

## My Free Games Collection

- IndiegameBundles (EpicBundle alternative)
    - [https://github.com/azhuge233/IndiegameBundlesNotifier](https://github.com/azhuge233/IndiegameBundlesNotifier)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/GOGGiveawayNotifier](https://github.com/azhuge233/GOGGiveawayNotifier)
- Ubisoft
    - [https://github.com/azhuge233/UbisoftGiveawayNotifier](https://github.com/azhuge233/UbisoftGiveawayNotifier)
- PlayStation Plus
    - [https://github.com/azhuge233/PSPlusMonthlyGames-Notifier](https://github.com/azhuge233/PSPlusMonthlyGames-Notifier)
- Reddit community
    - [https://github.com/azhuge233/RedditFreeGamesNotifier](https://github.com/azhuge233/RedditFreeGamesNotifier)
- Epic Games Store
    - [https://github.com/azhuge233/EGSFreeGamesNotifier](https://github.com/azhuge233/EGSFreeGamesNotifier)
    - [https://github.com/azhuge233/EGSMobileFreeGamesNotifier](https://github.com/azhuge233/EGSMobileFreeGamesNotifier)
- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Archived)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)(Not maintained)
- EpicBundle (site not updated)
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Archived)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)(Archived)
