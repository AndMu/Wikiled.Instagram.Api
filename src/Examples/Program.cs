using Examples.Samples;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Builder;
using Wikiled.Instagram.Api.Serialization;

namespace Examples
{
    internal class InstaProgram
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        private static IInstaApi api;

        private static LoggerFactory loggerFactory;

        private static void Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog();
            var result = Task.Run(MainAsync).GetAwaiter().GetResult();
            if (result)
            {
                return;
            }

            Console.ReadKey();
        }

        public static async Task<bool> MainAsync()
        {
            try
            {
                Console.WriteLine("Starting demo of InstagramApiSharp project");
                // create user session data and provide login details
                var userSession = new UserSessionData { UserName = "XXXX", Password = "XXXX" };
                // if you want to set custom device (user-agent) please check this:
                // https://github.com/ramtinak/InstagramApiSharp/wiki/Set-custom-device(user-agent)

                var delay = RequestDelay.FromSeconds(2, 2);
                // create new InstaApi instance using Builder
                api = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(loggerFactory) // use logger for requests and debug messages
                    .SetRequestDelay(delay)
                    .Build();

                var session = new FileSessionHandler(loggerFactory.CreateLogger<FileSessionHandler>(),
                                                     api,
                                                     new EncryptedSerializer(new PlainSerializer(), api));
                // create account
                // to create new account please check this:
                // https://github.com/ramtinak/InstagramApiSharp/wiki/Create-new-account

                try
                {
                    session.Load("state.bin");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (!api.IsUserAuthenticated)
                {
                    // login
                    Console.WriteLine($"Logging in as {userSession.UserName}");
                    delay.Disable();
                    var logInResult = await api.LoginAsync().ConfigureAwait(false);
                    delay.Enable();
                    if (!logInResult.Succeeded)
                    {
                        Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                        return false;
                    }
                }

                session.Save("state.bin");

                Console.WriteLine("Press 1 to start basic demo samples");
                Console.WriteLine("Press 2 to start upload photo demo sample");
                Console.WriteLine("Press 3 to start comment media demo sample");
                Console.WriteLine("Press 4 to start stories demo sample");
                Console.WriteLine("Press 5 to start demo with saving state of API instance");
                Console.WriteLine("Press 6 to start messaging demo sample");
                Console.WriteLine("Press 7 to start location demo sample");
                Console.WriteLine("Press 8 to start collections demo sample");
                Console.WriteLine("Press 9 to start upload video demo sample");

                var samplesMap = new Dictionary<ConsoleKey, IDemoSample>
                {
                    [ConsoleKey.D1] = new InstaBasics(api),
                    [ConsoleKey.D2] = new InstaUploadPhoto(api),
                    [ConsoleKey.D3] = new InstaCommentMedia(api),
                    [ConsoleKey.D4] = new InstaStories(api),
                    [ConsoleKey.D5] = new InstaSaveLoadState(api),
                    [ConsoleKey.D6] = new InstaMessaging(api),
                    [ConsoleKey.D7] = new InstaLocationSample(api),
                    [ConsoleKey.D8] = new InstaCollectionSample(api),
                    [ConsoleKey.D9] = new InstaUploadVideo(api)
                };
                var key = Console.ReadKey();
                Console.WriteLine(Environment.NewLine);
                if (samplesMap.ContainsKey(key.Key))
                {
                    await samplesMap[key.Key].DoShow().ConfigureAwait(false);
                }

                Console.WriteLine("Done. Press esc key to exit...");

                key = Console.ReadKey();
                return key.Key == ConsoleKey.Escape;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}