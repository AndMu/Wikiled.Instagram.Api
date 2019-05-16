using System;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Logic;

namespace Examples.Samples
{
    internal class InstaMessaging : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaMessaging(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            var recipientsResult = await api.MessagingProcessor.GetRankedRecipientsAsync();
            if (!recipientsResult.Succeeded)
            {
                Console.WriteLine("Unable to get ranked recipients");
                return;
            }

            Console.WriteLine("You can check more example of direct messaging in wiki pages:");
            Console.WriteLine("https://github.com/ramtinak/InstagramApiSharp/wiki/Direct-messaging");

            Console.WriteLine($"Got {recipientsResult.Value.Threads.Count} ranked threads");
            foreach (var thread in recipientsResult.Value.Threads)
            {
                Console.WriteLine($"Threadname: {thread.ThreadTitle}, users: {thread.Users.Count}");
            }

            var inboxThreads =
                await api.MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1));
            if (!inboxThreads.Succeeded)
            {
                Console.WriteLine("Unable to get inbox");
                return;
            }

            Console.WriteLine($"Got {inboxThreads.Value.Inbox.Threads.Count} inbox threads");
            foreach (var thread in inboxThreads.Value.Inbox.Threads)
            {
                Console.WriteLine($"Threadname: {thread.Title}, users: {thread.Users.Count}");
            }

            var firstThread = inboxThreads.Value.Inbox.Threads.FirstOrDefault();
            // send message to specific thread
            var sendMessageResult = await api.MessagingProcessor.SendDirectTextAsync(
                $"{firstThread.Users.FirstOrDefault()?.Pk}",
                firstThread.ThreadId,
                "test");
            Console.WriteLine(sendMessageResult.Succeeded ? "Message sent" : "Unable to send message");

            // just send message to user (thread not specified)
            sendMessageResult =
                await api.MessagingProcessor.SendDirectTextAsync($"{firstThread.Users.FirstOrDefault()?.Pk}",
                                                                      string.Empty,
                                                                      "one more test");
            Console.WriteLine(sendMessageResult.Succeeded ? "Message sent" : "Unable to send message");
        }
    }
}