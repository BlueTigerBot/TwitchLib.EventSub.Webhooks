using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Channel;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Drop;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Extension;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Stream;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.User;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core
{
    public delegate Task AsyncEventHandler(object sender, System.EventArgs args);
    public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs args) where TEventArgs : System.EventArgs;

    /// <summary>
    /// Class where everything runs together
    /// <para>Listen to events from EventSub from this class</para>
    /// </summary>
    public interface ITwitchEventSubWebhooks
    {
        /// <summary>
        /// Event that triggers on "channel.ban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelBanArgs>? OnChannelBan;
        /// <summary>
        /// Event that triggers on "channel.cheer" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCheerArgs>? OnChannelCheer;
        /// <summary>
        /// Event that triggers on "channel.follow" notifications
        /// </summary>
        event AsyncEventHandler<ChannelFollowArgs>? OnChannelFollow;
        /// <summary>
        /// Event that triggers on "channel.goal.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalBeginArgs>? OnChannelGoalBegin;
        /// <summary>
        /// Event that triggers on "channel.goal.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalEndArgs>? OnChannelGoalEnd;
        /// <summary>
        /// Event that triggers on "channel.goal.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalProgressArgs>? OnChannelGoalProgress;
        /// <summary>
        /// Event that triggers on "channel.hype_train.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainBeginArgs>? OnChannelHypeTrainBegin;
        /// <summary>
        /// Event that triggers on "channel.hype_train.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainEndArgs>? OnChannelHypeTrainEnd;
        /// <summary>
        /// Event that triggers on "channel.hype_train.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainProgressArgs>? OnChannelHypeTrainProgress;
        /// <summary>
        /// Event that triggers on "channel.moderator.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorAdd;
        /// <summary>
        /// Event that triggers on "channel.moderator.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorRemove;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardUpdate;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardRemove;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionUpdate;
        /// <summary>
        /// Event that triggers on "channel.poll.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollBeginArgs>? OnChannelPollBegin;
        /// <summary>
        /// Event that triggers on "channel.poll.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollEndArgs>? OnChannelPollEnd;
        /// <summary>
        /// Event that triggers on "channel.poll.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollProgressArgs>? OnChannelPollProgress;
        /// <summary>
        /// Event that triggers on "channel.prediction.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionBeginArgs>? OnChannelPredictionBegin;
        /// <summary>
        /// Event that triggers on "channel.prediction.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionEndArgs>? OnChannelPredictionEnd;
        /// <summary>
        /// Event that triggers on "channel.prediction.lock" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionLockArgs>? OnChannelPredictionLock;
        /// <summary>
        /// Event that triggers on "channel.prediction.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionProgressArgs>? OnChannelPredictionProgress;
        /// <summary>
        /// Event that triggers on "channel.raid" notifications
        /// </summary>
        event AsyncEventHandler<ChannelRaidArgs>? OnChannelRaid;
        /// <summary>
        /// Event that triggers on "channel.subscribe" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscribeArgs>? OnChannelSubscribe;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionEndArgs>? OnChannelSubscriptionEnd;
        /// <summary>
        /// Event that triggers on "channel.subscription.gift" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionGiftArgs>? OnChannelSubscriptionGift;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionMessageArgs>? OnChannelSubscriptionMessage;
        /// <summary>
        /// Event that triggers on "channel.unban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUnbanArgs>? OnChannelUnban;
        /// <summary>
        /// Event that triggers on "channel.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUpdateArgs>? OnChannelUpdate;
        /// <summary>
        /// Event that triggers if an error parsing a notification or revocation was encountered
        /// </summary>
        event AsyncEventHandler<OnErrorArgs>? OnError;
        /// <summary>
        /// Event that triggers on "drop.entitlement.grant" notifications
        /// </summary>
        event AsyncEventHandler<DropEntitlementGrantArgs>? OnDropEntitlementGrant;
        /// <summary>
        /// Event that triggers on "extension.bits_transaction.create" notifications
        /// </summary>
        event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? OnExtensionBitsTransactionCreate;
        /// <summary>
        /// Event that triggers on if a revocation notification was received
        /// </summary>
        event AsyncEventHandler<RevocationArgs>? OnRevocation;
        /// <summary>
        /// Event that triggers on "stream.offline" notifications
        /// </summary>
        event AsyncEventHandler<StreamOfflineArgs>? OnStreamOffline;
        /// <summary>
        /// Event that triggers on "stream.online" notifications
        /// </summary>
        event AsyncEventHandler<StreamOnlineArgs>? OnStreamOnline;
        /// <summary>
        /// Event that triggers on "user.authorization.grant" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationGrantArgs>? OnUserAuthorizationGrant;
        /// <summary>
        /// Event that triggers on "user.authorization.revoke" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationRevokeArgs>? OnUserAuthorizationRevoke;
        /// <summary>
        /// Event that triggers on "user.update" notifications
        /// </summary>
        event AsyncEventHandler<UserUpdateArgs>? OnUserUpdate;

        /// <summary>
        /// Processes "notification" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="headers">Dictionary of the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessNotificationAsync(Dictionary<string, string> headers, Stream body);
        /// <summary>
        /// Processes "revocation" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="headers">Dictionary of the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessRevocationAsync(Dictionary<string, string> headers, Stream body);
    }
}