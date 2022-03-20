using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Channel;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Drop;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Extension;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Stream;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.User;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.NamingPolicies;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Channel;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Drop;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Extension;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Stream;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.User;

namespace BlueTigerBot.Twitch.EventSub.Webhooks
{
    /// <inheritdoc/>
    /// <summary>
    /// <para>Implements <see cref="ITwitchEventSubWebhooks"/></para>
    /// </summary>
    public class TwitchEventSubWebhooks : ITwitchEventSubWebhooks
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DictionaryKeyPolicy = new SnakeCaseNamingPolicy()
        };

        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelBanArgs>? OnChannelBan;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCheerArgs>? OnChannelCheer;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelFollowArgs>? OnChannelFollow;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalBeginArgs>? OnChannelGoalBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalEndArgs>? OnChannelGoalEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalProgressArgs>? OnChannelGoalProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainBeginArgs>? OnChannelHypeTrainBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainEndArgs>? OnChannelHypeTrainEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainProgressArgs>? OnChannelHypeTrainProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollBeginArgs>? OnChannelPollBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollEndArgs>? OnChannelPollEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollProgressArgs>? OnChannelPollProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionBeginArgs>? OnChannelPredictionBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionEndArgs>? OnChannelPredictionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionLockArgs>? OnChannelPredictionLock;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionProgressArgs>? OnChannelPredictionProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelRaidArgs>? OnChannelRaid;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscribeArgs>? OnChannelSubscribe;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionEndArgs>? OnChannelSubscriptionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionGiftArgs>? OnChannelSubscriptionGift;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionMessageArgs>? OnChannelSubscriptionMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUnbanArgs>? OnChannelUnban;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUpdateArgs>? OnChannelUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<OnErrorArgs>? OnError;
        /// <inheritdoc/>
        public event AsyncEventHandler<DropEntitlementGrantArgs>? OnDropEntitlementGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? OnExtensionBitsTransactionCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<RevocationArgs>? OnRevocation;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOfflineArgs>? OnStreamOffline;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOnlineArgs>? OnStreamOnline;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationGrantArgs>? OnUserAuthorizationGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationRevokeArgs>? OnUserAuthorizationRevoke;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserUpdateArgs>? OnUserUpdate;

        /// <inheritdoc/>
        public async Task ProcessNotificationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                if (!headers.TryGetValue("Twitch-Eventsub-Subscription-Type", out var subscriptionType))
                {
                    OnError?.Invoke(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Type header was not found" });
                    return;
                }

                switch (subscriptionType)
                {
                    case "channel.ban":
                        var banNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelBan>>(body, _jsonSerializerOptions);
                        await (OnChannelBan?.Invoke(this, new ChannelBanArgs { Headers = headers, Notification = banNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.cheer":
                        var cheerNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCheer>>(body, _jsonSerializerOptions);
                        await (OnChannelCheer?.Invoke(this, new ChannelCheerArgs { Headers = headers, Notification = cheerNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.follow":
                        var followNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelFollow>>(body, _jsonSerializerOptions);
                        await (OnChannelFollow?.Invoke(this, new ChannelFollowArgs { Headers = headers, Notification = followNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.goal.begin":
                        var goalBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalBegin>>(body, _jsonSerializerOptions);
                        await (OnChannelGoalBegin?.Invoke(this, new ChannelGoalBeginArgs { Headers = headers, Notification = goalBeginNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.goal.end":
                        var goalEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalEnd>>(body, _jsonSerializerOptions);
                        await (OnChannelGoalEnd?.Invoke(this, new ChannelGoalEndArgs { Headers = headers, Notification = goalEndNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.goal.progress":
                        var goalProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalProgress>>(body, _jsonSerializerOptions);
                        await (OnChannelGoalProgress?.Invoke(this, new ChannelGoalProgressArgs { Headers = headers, Notification = goalProgressNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.hype_train.begin":
                        var hypeTrainBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainBegin>>(body, _jsonSerializerOptions);
                        await (OnChannelHypeTrainBegin?.Invoke(this, new ChannelHypeTrainBeginArgs { Headers = headers, Notification = hypeTrainBeginNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.hype_train.end":
                        var hypeTrainEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainEnd>>(body, _jsonSerializerOptions);
                        await (OnChannelHypeTrainEnd?.Invoke(this, new ChannelHypeTrainEndArgs { Headers = headers, Notification = hypeTrainEndNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.hype_train.progress":
                        var hypeTrainProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainProgress>>(body, _jsonSerializerOptions);
                        await (OnChannelHypeTrainProgress?.Invoke(this, new ChannelHypeTrainProgressArgs { Headers = headers, Notification = hypeTrainProgressNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.moderator.add":
                        var moderatorAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelModerator>>(body, _jsonSerializerOptions);
                        OnChannelModeratorAdd?.Invoke(this, new ChannelModeratorArgs { Headers = headers, Notification = moderatorAddNotification! });
                        break;
                    case "channel.moderator.remove":
                        var moderatorRemoveNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelModerator>>(body, _jsonSerializerOptions);
                        await (OnChannelModeratorRemove?.Invoke(this, new ChannelModeratorArgs { Headers = headers, Notification = moderatorRemoveNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.channel_points_custom_reward.add":
                        var customRewardAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        await (OnChannelPointsCustomRewardAdd?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardAddNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.channel_points_custom_reward.remove":
                        var customRewardRemoveNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        await (OnChannelPointsCustomRewardRemove?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardRemoveNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.channel_points_custom_reward.update":
                        var customRewardUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        await (OnChannelPointsCustomRewardUpdate?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardUpdateNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.channel_points_custom_reward_redemption.add":
                        var customRewardRedemptionAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(body, _jsonSerializerOptions);
                        await (OnChannelPointsCustomRewardRedemptionAdd?.Invoke(this, new ChannelPointsCustomRewardRedemptionArgs { Headers = headers, Notification = customRewardRedemptionAddNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.channel_points_custom_reward_redemption.update":
                        var customRewardRedemptionUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(body, _jsonSerializerOptions);
                        await (OnChannelPointsCustomRewardRedemptionUpdate?.Invoke(this, new ChannelPointsCustomRewardRedemptionArgs { Headers = headers, Notification = customRewardRedemptionUpdateNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.poll.progress":
                        var pollProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPollProgress>>(body, _jsonSerializerOptions);
                        await (OnChannelPollProgress?.Invoke(this, new ChannelPollProgressArgs { Headers = headers, Notification = pollProgressNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.prediction.begin":
                        var predictionBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionBegin>>(body, _jsonSerializerOptions);
                        await (OnChannelPredictionBegin?.Invoke(this, new ChannelPredictionBeginArgs { Headers = headers, Notification = predictionBeginNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.prediction.end":
                        var predictionEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionEnd>>(body, _jsonSerializerOptions);
                        await (OnChannelPredictionEnd?.Invoke(this, new ChannelPredictionEndArgs { Headers = headers, Notification = predictionEndNotification! }) ?? Task.CompletedTask);
                        break;

                    case "channel.prediction.progress":
                        var predictionProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionProgress>>(body, _jsonSerializerOptions);
                        await (OnChannelPredictionProgress?.Invoke(this, new ChannelPredictionProgressArgs { Headers = headers, Notification = predictionProgressNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.raid":
                        var raidNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelRaid>>(body, _jsonSerializerOptions);
                        await (OnChannelRaid?.Invoke(this, new ChannelRaidArgs { Headers = headers, Notification = raidNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.subscribe":
                        var subscribeNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscribe>>(body, _jsonSerializerOptions);
                        await (OnChannelSubscribe?.Invoke(this, new ChannelSubscribeArgs { Headers = headers, Notification = subscribeNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.subscription.end":
                        var subscriptionEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionEnd>>(body, _jsonSerializerOptions);
                        await (OnChannelSubscriptionEnd?.Invoke(this, new ChannelSubscriptionEndArgs { Headers = headers, Notification = subscriptionEndNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.subscription.gift":
                        var subscriptionGiftNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionGift>>(body, _jsonSerializerOptions);
                        await (OnChannelSubscriptionGift?.Invoke(this, new ChannelSubscriptionGiftArgs { Headers = headers, Notification = subscriptionGiftNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.subscription.message":
                        var subscriptionMessageNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionMessage>>(body, _jsonSerializerOptions);
                        await (OnChannelSubscriptionMessage?.Invoke(this, new ChannelSubscriptionMessageArgs { Headers = headers, Notification = subscriptionMessageNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.unban":
                        var unbanNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelUnban>>(body, _jsonSerializerOptions);
                        await (OnChannelUnban?.Invoke(this, new ChannelUnbanArgs { Headers = headers, Notification = unbanNotification! }) ?? Task.CompletedTask);
                        break;
                    case "channel.update":
                        var channelUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelUpdate>>(body, _jsonSerializerOptions);
                        await (OnChannelUpdate?.Invoke(this, new ChannelUpdateArgs { Headers = headers, Notification = channelUpdateNotification! }) ?? Task.CompletedTask);
                        break;
                    case "drop.entitlement.grant":
                        var dropGrantNotification = await JsonSerializer.DeserializeAsync<BatchedNotificationPayload<DropEntitlementGrant>>(body, _jsonSerializerOptions);
                        await (OnDropEntitlementGrant?.Invoke(this, new DropEntitlementGrantArgs { Headers = headers, Notification = dropGrantNotification! }) ?? Task.CompletedTask);
                        break;
                    case "extension.bits_transaction.create":
                        var extBitsTransactionCreateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ExtensionBitsTransactionCreate>>(body, _jsonSerializerOptions);
                        await (OnExtensionBitsTransactionCreate?.Invoke(this, new ExtensionBitsTransactionCreateArgs { Headers = headers, Notification = extBitsTransactionCreateNotification! }) ?? Task.CompletedTask);
                        break;
                    case "stream.offline":
                        var streamOfflineNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<StreamOffline>>(body, _jsonSerializerOptions);
                        await (OnStreamOffline?.Invoke(this, new StreamOfflineArgs { Headers = headers, Notification = streamOfflineNotification! }) ?? Task.CompletedTask);
                        break;
                    case "stream.online":
                        var streamOnlineNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<StreamOnline>>(body, _jsonSerializerOptions);
                        await (OnStreamOnline?.Invoke(this, new StreamOnlineArgs { Headers = headers, Notification = streamOnlineNotification! }) ?? Task.CompletedTask);
                        break;
                    case "user.authorization.grant":
                        var userAuthGrantNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserAuthorizationGrant>>(body, _jsonSerializerOptions);
                        await (OnUserAuthorizationGrant?.Invoke(this, new UserAuthorizationGrantArgs { Headers = headers, Notification = userAuthGrantNotification! }) ?? Task.CompletedTask);
                        break;
                    case "user.authorization.revoke":
                        var userAuthRevokeNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserAuthorizationRevoke>>(body, _jsonSerializerOptions);
                        await (OnUserAuthorizationRevoke?.Invoke(this, new UserAuthorizationRevokeArgs { Headers = headers, Notification = userAuthRevokeNotification! }) ?? Task.CompletedTask);
                        break;
                    case "user.update":
                        var userUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserUpdate>>(body, _jsonSerializerOptions);
                        await (OnUserUpdate?.Invoke(this, new UserUpdateArgs { Headers = headers, Notification = userUpdateNotification! }) ?? Task.CompletedTask);
                        break;
                    default:
                        OnError?.Invoke(this, new OnErrorArgs { Reason = "Unknown_Subscription_Type", Message = $"Cannot parse unknown subscription type {subscriptionType}" });
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }

        /// <inheritdoc/>
        public async Task ProcessRevocationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<object>>(body, _jsonSerializerOptions);
                OnRevocation?.Invoke(this, new RevocationArgs { Headers = headers, Notification = notification! });
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }
    }
}