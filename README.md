# README #
This project is to create a decent class library for interacting with Steam Trades and other parts of the Steam network.

# CHANGELOG OF PREVIOUS COMMIT #
Made changes to:

IEconServiceHandler.cs:
-Migrated ApiUrl to BaseUrl and made the corresponding changes to the code.
ISteamEconomyHandler.cs:
-Removed ToAssetClassInfo method, GetAssetClassInfo now does this automatically and returns an AssetClassInfo object.
InventoryHandler.cs:
-Changed initializer to include SteamId, Config/Account objects no longer required.
-Removed old RefreshInventories method that required Config object.
Web.cs:
-General readability improvements.