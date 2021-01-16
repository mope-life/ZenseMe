ZenseMe (Windows Phone, Zune, MTP device scrobbler for Last.fm)
Release: v2.0.2
By Dustin Ross
Originally by Arnold Vink (Dumbie)
--------------------------------------------------------------------------------


If you are building from source, you will need to get an API key from last.fm. Place your 'API key' and 'Shared secret' in ZenseMeResources/Keys.cs. If anyone knows a better way to do this, please tell me.


| Requirements:
- .NET Framework 4.5 (Fixes System.Web error) | Download: http://go.microsoft.com/fwlink/?LinkId=225702
- Visual C++ 2010 SP1 (Fixes dll errors) | Download: http://go.microsoft.com/fwlink/?LinkId=210621
- Zune Software (Installs device drivers) | Download: http://go.microsoft.com/?linkid=9753463


| Special thanks to:
Zenses - Sixones (Adam Livesley)


| Release readme:
--------------------------------------------------------------------------------
ZenseMe (Windows Phone, Zune, MTP device scrobbler for Last.fm)
Release: v2.0.2
By Dustin Ross
Originally by Arnold Vink (Dumbie)

--------------------------------------------------------

Hi, thanks for using this app! Actually, I didn't really make it; they just pay me to talk about it.


VERY IMPORTANT NOTE!

If you are updating from a previous version, you will want to KEEP the folder "Data/". Copy that folder from the directory for your old version to the directory for your new version. This will save you the trouble of double scrobbling/skipping/ignoring a bunch of tracks and listens you've already dealt with.


USAGE:

1. Unzip this directory if you haven't done so already, and put it wherever you want. This program is self contained in the directory - no installation required.

2. You will have to close out of the following media players before attempting to run ZenseMe: Zune, songbird, Media Go, MediaMonkey. I'm not sure why--presumably they can interfere with communication between this app and your device.

3. I think it also helps to plug in your device before launching ZenseMe.

4. Click "fetchsongs" to start loading up your playcounts. [This writer finds that this step takes an awful long time. Go get a coffee or something.]

5. The results will arrive in the "Recently Played" tab. Go there, select whichever tracks you want the world to know about, then click "Scrobble Tracks."

6. Repeat steps 2-5 whenever you want to scrobble some more.


NOTES:

i) The first time you fetch songs, you might have thousands upon thousands of listens to scrobble. I recommend selecting them all and clicking "Skip Tracks", because scrobbling these old plays might be a very time-intensive task. Plus it might include music you haven't listened to since high school, or (more importantly) music you've already scrobbled.

ii) The first time you click "Link with profile" or "Scrobble tracks", you will be taken to a browser to log in to your last.fm account. JavaScript will probably throw a couple errors at you, but that's fine. Once you've authorized ZenseMe on your last.fm account, close the browser window. You only have to do this once.

iii) The app is only ever associated with one user and one database. If you have multiple Zune/last.fm users (however unlikely that is in this era), just copy this whole directory for each new instance.


TROUBLE:

- If everything works but no plays show up on your profile, it might be a problem with your firewall (or so I'm told).

- If you accidentally or intentionally revoke authorization for ZenseMe in your last.fm account, just click "Detach from profile" on the main tab and then link again.

- If you get any game-ending error messages, kindly copy the contents of the message and send to dustinross@live.com.


CONTRIBUTE:

If you'd like to help improve this app, go to github.com/mope-life/ZenseMe

Happy scrobbling!
-Dusty Scott Ross, Aug. 2020
-Updated Oct. 2020
-Updated Jan. 2021
--------------------------------------------------------------------------------


| Changelog
v2.0.2 (15-january-2021)
- Autoignore feature added
- Fixed a bug which caused scrobbling to fail after one or two tracks
- Database now automatically updates itself from old versions

v2.0.1 (16-october-2020)
- Status bar more accurately logs progress while fetching from device
- Database now includes "Genre" tag
- "Select artist/album" context menu removed and replaced with "Filtered Selection" box
- Authentication process streamlined and now remembers Username
- "Link with profile" button in main page now exists

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