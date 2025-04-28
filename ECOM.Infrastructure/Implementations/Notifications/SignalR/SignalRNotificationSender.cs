using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.Notifications;
using ECOM.Shared.Library.Functions.Helpers;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Notifications.SignalR
{
    public class SignalRNotificationSender<THub>(
        ILog log,
        IHubContext<THub> hubContext,
        IOptions<AppSettings> options) : INotificationSender where THub : Hub
    {
        private readonly ILog _logger = log;
        private readonly IHubContext<THub> _hubContext = hubContext;
        private readonly AppSettings _appSettings = options.Value;

        
        public async Task SendAsync(NotificationMessage message)
        {
            var receiveMethodName = _appSettings.SignalIR.ReceiveMethod;

            _logger.Information($"Starting to send SignalR notification (Type: {message.Type}, Target Users: {string.Join(", ", message.TargetUserIds ?? [])}).");

            try
            {
                await CommonHelper.RetryAsync(async () =>
                {
                    if (message.TargetUserIds != null && message.TargetUserIds.Length > 0)
                    {
                        if (message.TargetUserIds.Length == 1)
                        {
                            _logger.Debug($"Sending notification to user: {message.TargetUserIds.First()} (Method: {receiveMethodName}).");
                            await _hubContext.Clients.User(message.TargetUserIds.First().ToString())
                                             .SendAsync(receiveMethodName, message);
                        }
                        else
                        {
                            _logger.Debug($"Sending notification to users: {string.Join(", ", message.TargetUserIds)} (Method: {receiveMethodName}).");
                            await _hubContext.Clients.Users(message.TargetUserIds.Select(id => id.ToString()).ToList())
                                             .SendAsync(receiveMethodName, message);
                        }
                    }
                    else
                    {
                        _logger.Debug($"Sending broadcast notification (Method: {receiveMethodName}).");
                        await _hubContext.Clients.All.SendAsync(receiveMethodName, message);
                    }
                }, TimeSpan.FromSeconds(_appSettings.DbContext.Retry.IntervalInSeconds), _appSettings.DbContext.Retry.MaxAttemptCount);

                _logger.Information($"Successfully sent SignalR notification (Type: {message.Type}, Target Users: {string.Join(", ", message.TargetUserIds ?? [])}).");
            }
            catch (HubException ex)
            {
                _logger.Error($"HubException occurred while sending SignalR notification (Type: {message.Type}, Target Users: {string.Join(", ", message.TargetUserIds ?? [])}).", ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"An unexpected error occurred while sending SignalR notification (Type: {message.Type}, Target Users: {string.Join(", ", message.TargetUserIds ?? [])}).", ex);
                throw;
            }
        }
    }
}