using System.Collections.Generic;

namespace AutoService.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalRequests { get; set; }
        public int NewRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int CancelledRequests { get; set; }
        public double AverageCompletionDays { get; set; }

        public List<CarTypeStatistic> ProblemsByType { get; set; } = new();
    }

    public class CarTypeStatistic
    {
        public string CarType { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}