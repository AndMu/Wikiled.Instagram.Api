using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class
        InstaDirectInboxSubscriptionConverter : IObjectConverter<InstaDirectInboxSubscription,
            InstaDirectInboxSubscriptionResponse>
    {
        public InstaDirectInboxSubscriptionResponse SourceObject { get; set; }

        public InstaDirectInboxSubscription Convert()
        {
            var subscription = new InstaDirectInboxSubscription
            {
                Auth = SourceObject.Auth,
                Sequence = SourceObject.Sequence,
                Topic = SourceObject.Topic,
                Url = SourceObject.Url
            };
            return subscription;
        }
    }
}