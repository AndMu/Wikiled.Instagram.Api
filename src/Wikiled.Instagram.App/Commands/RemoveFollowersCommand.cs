using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class RemoveFollowersCommand : DiscoveryCommand
    {
        private readonly ILogger<RemoveFollowersCommand> log;

        private readonly RemoveFollowConfig config;

        private readonly IInstaApi instagram;

        public RemoveFollowersCommand(ILogger<RemoveFollowersCommand> log, IInstaApi instagram, RemoveFollowConfig config, ISessionHandler session)
            : base(log, instagram, config, session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.instagram = instagram ?? throw new ArgumentNullException(nameof(instagram));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected override async Task Internal(CurrentUser currentUser, CancellationToken token)
        {
            var following = await instagram.UserProcessor.GetUserFollowingAsync(currentUser.UserName, PaginationParameters.MaxPagesToLoad(100)).ConfigureAwait(false);
            log.LogInformation("User is following {0}", following.Value.Count);

            var followingLookup = following.Value.ToLookup(item => item.Pk);

            var followers = await instagram.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(100)).ConfigureAwait(false);
            log.LogInformation("User has {0} followers", followers.Value.Count);

            var followersLookup = followers.Value.ToLookup(item => item.Pk);

            log.LogInformation("User is following {0} without feedback", followingLookup.Count(item => !followersLookup.Contains(item.Key)));
            for (int i = config.From; i < config.To; i++)
            {
                var user = following.Value[i];
                if (!followersLookup.Contains(user.Pk))
                {
                    log.LogInformation("Unfollowing {0}", user.FullName);
                    await instagram.UserProcessor.UnFollowUserAsync(user.Pk).ConfigureAwait(false);
                }
            }
        }
    }
}
