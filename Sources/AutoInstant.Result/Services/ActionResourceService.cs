using AutoInstant.Result.Models;

namespace AutoInstant.Result.Services
{
    public interface ActionResourceService
    {
        XmlResource HalftimeFirstGoalResources(string halftimeGoal);

        XmlResource FullTimeFirstGoalResources(string halftimeGoal, string fullTimeGoal);
    }

    public class ActionResourceServiceImp : ActionResourceService
    {
        private const byte ManuallySettled = 5;
        private const byte Completed = 4;
        private const string HalftimeFirstGoal = "127F";
        private const string FullTimeFirstGoal = "14F";

        private readonly FirstGoalService goalResultService;

        public ActionResourceServiceImp(FirstGoalService goalResultService)
        {
            this.goalResultService = goalResultService;
        }

        public XmlResource HalftimeFirstGoalResources(string halftimeGoal)
        {
            var firstGoal = goalResultService.FirstGoal(halftimeGoal);

            return new XmlResource
            {
                ResourceId = HalftimeFirstGoal,
                ResultStatusId = ManuallySettled,
                SettlementStatusId = Completed,
                HomeValue = firstGoal
            };
        }

        public XmlResource FullTimeFirstGoalResources(string halftimeGoal, string fullTimeGoal)
        {
            var goal = $"{halftimeGoal}{fullTimeGoal}";
            var firstGoal = goalResultService.FirstGoal(goal);

            return new XmlResource
            {
                ResourceId = FullTimeFirstGoal,
                ResultStatusId = ManuallySettled,
                SettlementStatusId = Completed,
                HomeValue = firstGoal
            };
        }
    }
}