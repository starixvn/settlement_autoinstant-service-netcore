using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;

namespace AutoInstant.Services;

public interface NotificationService
{
    Task AddNotification(Match match, bool isSendToLetsTalk, string messageContent);
}

public class NotificationServiceImp(IDynamicRepository repository) : NotificationService
{
    public async Task AddNotification(Match match, bool isSendToLetsTalk, string messageContent)
    {
        if (isSendToLetsTalk)
        {
            var actionName = match.TypeId == SettleType.FirstHalf ? "HT Auto Instant" : "FT Auto Instant";
            var criteria = new AddNotificationCriteria
            {
                MatchId = match.MatchId,
                ActionName = actionName,
                MessageContent = messageContent
            };
            await repository.ExecuteAsync(criteria);
        }
    }
}