# SchoolInstantMessenger
NEA Project for A-level. Instant messenger designed to operate over a WAN, with admin controls suitable for a school enviornment.
The admin login is "Mr Deane:deane". Normal users can be created anytime.
If something doesn't work try the following:
1. Set the path to the database Users.mdf in IMServer
2. Set the server IP in ConstantsModule in IMClient
3. Recreate the ConstantsModule in IMClient (i.e copy the code, delete the module, recreate the module, paste the code back in) since VS seems to be quite buggy about ConstantsModule.
