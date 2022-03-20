using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.User;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.User
{
    public class UserAuthorizationGrantArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<UserAuthorizationGrant>>
    { }
}