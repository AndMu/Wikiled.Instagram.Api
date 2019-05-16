# Wikiled.Instagram.Api ![Wikiled.Instagram.Api](http://s8.picofile.com/file/8336601292/insta50x.png)
A complete Instagram API for .NET

Supports almost every features that Instagram app has!

-----
# This project is not maintained anymore. [#233](https://github.com/ramtinak/Wikiled.Instagram.Api/issues/233)
-----
| Target | Branch | Version | Download link | Total downloads |
| ------ | ------ | ------ | ------ | ------ |
| Nuget | master | v1.4.0.0 | [![NuGet](https://img.shields.io/nuget/v/Wikiled.Instagram.Api.svg)](https://www.nuget.org/packages/Wikiled.Instagram.Api) | [![NuGet downloads](https://img.shields.io/nuget/dt/Wikiled.Instagram.Api.svg)](https://www.nuget.org/packages/Wikiled.Instagram.Api) |


Nuget package manager command:
```
PM> Install-Package Wikiled.Instagram.Api
```

## Overview
There are a lot of features and bug fix me and [NGame1](https://github.com/NGame1) and [other contributors](https://github.com/ramtinak/Wikiled.Instagram.Api/graphs/contributors) added to this library.
Check [sample projects](https://github.com/ramtinak/Wikiled.Instagram.Api/tree/master/samples) and [wiki pages](https://github.com/ramtinak/Wikiled.Instagram.Api/wiki) to see how it's works.

## Features
Some of features:

|    |    |    |    |
| ------ | ------ | ------ | ------ |
| Login | Login with cookies | Logout | Create new account email/phone number |
| Edit profile | Change/remove profile picture | Story settings | Get user explore feed |
| Get user timeline feed | Get all user media by username | Get media by its id | Get user info by its username |
| Get current user info | Get tag feed by tag value | Get current user media | Get followers list |
| Get followers list for logged in user | Get following list | Get recent following activity | Get user tags by username |
| Get direct mailbox | Get recent recipients | Get ranked recipients | Get inbox thread |
| Get recent activity | Like media | Unlike media | Follow user |
| Unfollow user | Set account private | Set account public | Send comment |
| Delete comment | Upload photo | Upload video | Get followings list |
| Delete media (photo/video/album) | Upload story (photo/video/album) | Change password | Send direct message |
| Search location | Get location feed | Collection create/get by id/get all/add items | Support challenge required |
| Upload album (videos/photo) | Highlight support | Share story | Send direct photo/video/ stories/profile/ link/location like/live |
| IG TV support | Share media to direct thread | Business account support | Share media as story |

## Usage
#### Use builder to get Insta API instance:
```c#
var api = InstaApiBuilder.CreateBuilder()
                // required
                .SetUser(new UserSessionData(...Your user...))
                // optional
                .UseLogger(new SomeLogger())
                // optional
                .UseHttpClient(new SomeHttpClient())
                // optional
                .UseHttpClientHandler(httpHandlerWithSomeProxy)
                // optional
                .SetRequestDelay(new SomeRequestDelay())
                // optional
                .SetApiVersion(SomeApiVersion)
                .Build();
```
##### Note: every API method has synchronous implementation as well.


