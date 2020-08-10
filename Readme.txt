ZenseMe (Windows Phone, Zune, MTP device scrobbler for Last.fm)
By Arnold Vink (Dumbie)
Release: v2.0.0

--------------------------------------------------------

If you are building from source, you will need to get an API key from last.fm. Place your 'API key' and 'Shared secret' in ZenseMeResources/Keys.cs. If anyone knows a better way to do this, please tell me.


| Installation Instructions
1) Extract the ZenseMe directory to any directory you want.
2) Make sure that you installed the requirements (.NET Framework 4.5 / Visual C++)
3) Launch ZenseMe.exe
4) Fetch the songs from your device by clicking on "fetchsongs"
5) The first time you submit songs, allow the app to access your last.fm account
6) Have fun scrobbling songs from your mobile devices.

| Update Instructions
When you are updating ZenseMe from a previous version make sure you "DON'T" delete the "Data" directory,
This directory contains a database with all your previous made scrobbles, skipped and ignored tracks.

| Tips and tricks
- When the songs don't show up on your Last.fm profile make sure ZenseMe is not blocked by your firewall.
- By right clicking on the "Recently Played/Ignored Tracks" list you can select all tracks, artists, albums or devices.
- Your device only saves the playcount from stored tracks, tracks streamed with your Zune Pass or Google Music will not count.
-(?) You can check if your device is compatible with ZenseMe in this list: http://www.last.fm/group/ZenseMe/forum/245330/_/1015674
-(?) If all the songs show up with a playcount of 0 it means your device does not support playcount information or is set to MSC mode.

| Requirements
- .NET Framework 4.5 (Fixes System.Web error) | Download: http://go.microsoft.com/fwlink/?LinkId=225702
- Visual C++ 2010 SP1 (Fixes dll errors) | Download: http://go.microsoft.com/fwlink/?LinkId=210621
- Zune Software (Installs device drivers) | Download: http://go.microsoft.com/?linkid=9753463

| Donation
Feel free to make a donation on: http://donation.arnoldvink.com

| Special thanks to
Zenses - Sixones (Adam Livesley)

| Changelog
v2.0.0 (10-august-2020)
- Project revived from the dead
- Now uses the Last.fm Web Services Scrobbling API 2.0
- No longer requires username and password after first use

v1.1.6 (23-november-2016)
- Fixed https connection related scrobbling errors.
- Updated sqlite files to version 1.0.103 (32-bit)
* Update requires ZenseMe.exe.config to be replaced.

v1.1.5 (9-march-2016)
- Improved the scrobbling speed to Last.fm.
- Added quick select all songs button to tabs.
- Fixed utc date time related scrobbling errors.
- Updated sqlite files to version 1.0.99 (32-bit)

v1.1.2 (7-september-2015)
- Updated to latest Last.fm api changes to fix scrobbling.
- Updated sqlite files to version 1.0.98 (32-bit)

v1.1.1 (22-july-2014)
- Updated sqlite files to version 1.0.93 (32-bit)

v1.1.0 (27-november-2012)
- Modernized the user interface
- Added multiple devices support
- Fixed some scrobbling errors

v1.0.4 (25-october-2012)
- Updated sqlite files to version 1.0.82 (32-bit)
- Misc. fixes and performance improvements

v1.0.3 (30-june-2012)
- Added Media Monkey running check
- Device name now stores with friendly name
- Misc. fixes and performance improvements

v1.0.2 (30-may-2012)
- Added Sony Media Go running check
- Added tips/help and about windows to help menu
- Updated sqlite files to version 1.0.81 (32-bit)
- Misc. fixes and performance improvements

v1.0.1 (01-february-2012)
- Fixed Short Date timezone format errors.
- Fixed sometimes not scrobbling to last.fm.
- Changed "Confirm Scrobble" window not on top.
- Updated sqlite files to version 1.0.79

v1.0.0 (17-january-2012)
- Added fetch button Zune / Songbird running check.
- Improved database saving stability.

v0.9.9 (02-january-2012)
- Added option to switch between Album Artist and Artist
- Improved overall fetching performance
- Added current fetch artist status

v0.9.8 (10-december-2011)
- Added fetch lower playcount/scrobble check
- Added double scrobble empty setting check
- Improved overall stability and performance
- Updated sqlite files to version 1.0.77

v0.9.7 (27-november-2011)
- Added Songbird running startup check
- Added time between double track scrobbling setting
- Added battery level status to device information
- Fixed scrobble time calculation not matching timezone

v0.9.6 (24-november-2011)
- Updated app manifest to asInvoker

R95 (31-october-2011)
- Added custom time scrobbling
- Added MD5 password storage encryption
- Added UAC Run as administrator manifest
- Improved overall stability and performance

R93 (21-october-2011)
- Updated sqlite files to version 1.0.76.0

R92 (20-october-2011)
- Fixed updated song playcount reading not working

R91 (11-october-2011)
- Added "Open your profile" button on main screen
- Fixed invalid username and password message not showing

R90 (10-october-2011)
- Added function "Skip Track" which pretends to scrobble
- Added ignore/un-ignore function to temporary ignore certain tracks
- Fixed scrobbling date desc > ascending on last.fm site
- Improved overall stability and performance

R85 (08-october-2011)
- Improved overall stability and performance

R83 (07-october-2011)
- Changed invalid track length to message

R82 (07-october-2011)
- Added more information in summary window
- Added select songs from device option
- Added current track scrobble status
- Added automatically refreshing after fetching
- Fixed all known unexpected SQL crashes
- Fixed recently played scrollbar position
- Improved overall performance

R60 (04-october-2011)
- Added "All Tracks" tab to show all available tracks
- Updated to Last.fm Submissions Protocol v1.2.1
- Scrobble time now decreases instead of increasing time
- Added fetch status update when finished fetching
- Fixed last track not clearing after scrobble
- Fixed scrobble time to UTC Now time zone
- Fixed one more SQL Crash

R53 (30-september-2011)
- Fixed window resizing (track list resizing)
- Fixed double scrobbling sometimes not working

R50 (29-september-2011)
- Added playcount scrobbling
- Added device model to tracklist
- Added Wrong/invalid username stop command
- Now shows seconds in lists instead of ms
- Fixed history list showing all songs

R42 (28-september-2011)
- Fixed scrobble list view (click first)
- Changed refresh button to song and device
- Added scrobbled track history list
- Added track moving after successful scrobble
- Added device in database after fetching
- Improved the User Interface

R28 (25-september-2011)
- Added refresh device button in file menu

R26 (25-september-2011)
- Fixed SQLlite dll in resources directory

R25 (25-september-2011)
- Initial test release