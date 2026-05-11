namespace StreamingApp.Admin.Models;

public sealed class AdminDashboardViewModel
{
    public int GenreCount { get; set; }
    public int MovieCount { get; set; }
    public int SeriesCount { get; set; }
    public int SeasonCount { get; set; }
    public int EpisodeCount { get; set; }
    public int UserCount { get; set; }
    public int SubscriptionPlanCount { get; set; }
    public int UserSubscriptionCount { get; set; }
    public int PendingReviewCount { get; set; }
    public int PromotedContentCount { get; set; }
}
