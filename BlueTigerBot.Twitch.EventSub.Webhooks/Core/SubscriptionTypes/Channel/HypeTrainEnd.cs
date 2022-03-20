﻿using System;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.Models.HypeTrain;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Core.SubscriptionTypes.Channel
{
    /// <summary>
    /// HypeTrain End subscription type model
    /// <para>Description:</para>
    /// <para>A Hype Train ends on the specified channel.</para>
    /// </summary>
    public class HypeTrainEnd : HypeTrainBase
    {
        /// <summary>
        /// The current level of the Hype Train.
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// The time when the Hype Train cooldown ends so that the next Hype Train can start.
        /// </summary>
        public DateTime CooldownEndsAt { get; set; } = DateTime.MinValue;
    }
}