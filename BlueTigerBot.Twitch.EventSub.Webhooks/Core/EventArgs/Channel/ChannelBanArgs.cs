using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Channel;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Channel
{
    public class ChannelBanArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<ChannelBan>>
    { }
}