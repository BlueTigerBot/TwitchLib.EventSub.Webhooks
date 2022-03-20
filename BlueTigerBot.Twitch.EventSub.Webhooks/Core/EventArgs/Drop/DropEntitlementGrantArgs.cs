using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Drop;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Drop
{
    public class DropEntitlementGrantArgs : TwitchLibEventSubEventArgs<BatchedNotificationPayload<DropEntitlementGrant>>
    { }
}