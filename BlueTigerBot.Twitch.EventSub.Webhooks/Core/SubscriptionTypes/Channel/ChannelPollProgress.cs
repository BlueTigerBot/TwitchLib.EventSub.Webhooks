﻿using System;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models.Polls;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Channel
{
    /// <summary>
    /// Channel Poll Progress subscription type model
    /// <para>Description:</para>
    /// <para>Users respond to a poll on a specified channel.</para>
    /// </summary>
    public class ChannelPollProgress : ChannelPollBase
    {
        /// <summary>
        /// The time the poll will end.
        /// </summary>
        public DateTime EndsAt { get; set; } = DateTime.MinValue;
    }
}