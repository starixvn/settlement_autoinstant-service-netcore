using System.Threading.Tasks;
using AutoInstant.Constants;
using AutoInstant.Criteria;
using AutoInstant.Models;
using Fanex.Data.Repository;

namespace AutoInstant.Services
{
    public interface NotificationService
    {
        Task AddNotification(Match match, bool isSendToLetsTalk, string messageContent);
    }

    public class NotificationServiceImp : NotificationService
    {
        private readonly IDynamicRepository repository;

        public NotificationServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

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
}