using System.Threading;
using System.Threading.Tasks;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs;
using BlueTigerBot.Twitch.EventSub.Webhooks.Core.EventArgs.Channel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlueTigerBot.Twitch.EventSub.Webhooks.Example
{
    public class EventSubHostedService : IHostedService
    {
        private readonly ILogger<EventSubHostedService> _logger;
        private readonly ITwitchEventSubWebhooks _eventSubWebhooks;

        public EventSubHostedService(ILogger<EventSubHostedService> logger, ITwitchEventSubWebhooks eventSubWebhooks)
        {
            _logger = logger;
            _eventSubWebhooks = eventSubWebhooks;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventSubWebhooks.OnError += OnError;
            _eventSubWebhooks.OnChannelFollow += OnChannelFollow;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventSubWebhooks.OnError -= OnError;
            _eventSubWebhooks.OnChannelFollow -= OnChannelFollow;
            return Task.CompletedTask;
        }

        private Task OnChannelFollow(object sender, ChannelFollowArgs e)
        {
            _logger.LogInformation($"{e.Notification.Event.UserName} followed {e.Notification.Event.BroadcasterUserName} at {e.Notification.Event.FollowedAt.ToUniversalTime()}");
            return Task.CompletedTask;
        }

        private Task OnError(object sender, OnErrorArgs e)
        {
            _logger.LogError($"Reason: {e.Reason} - Message: {e.Message}");
            return Task.CompletedTask;
        }
    }
}