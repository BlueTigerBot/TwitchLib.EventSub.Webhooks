using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Stream;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Stream
{
    public class StreamOfflineArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<StreamOffline>>
    { }
}