# NOTICE #
FatherFoxxy will no longer be working on this library. Commits and forks are always welcome, however. FatherFoxxy will continue to deny/accept commits but will not be personally working on the library itself.

# README #
This project is to create a decent class library for interacting with Steam Trades and other parts of the Steam network.

# PREREQS #
You MUST have Visual Studio 2015. Older versions may work, but all testing has been done using Visual Studio 2015. All of the sample code is confirmed working on that specific compiler.

# PROJECTS THAT USE THE STEAMAUTH LIBRARY #
AcceptAllMobileConfirmations.csproj

Please note that with the SteamAuth library-enabled projects, you must compile their project that can be found here: https://github.com/geel9/SteamAuth

Then, on any project that requires the SteamAuth library, you go to Add Reference > Browse > SteamAuth.dll (the .dll will be located in the release\bin\ folder of the SteamAuth project after you compile it.
