using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StreamingApp.Domain.Enums;

namespace StreamingApp.Admin.Models;

public sealed class SeasonListItemViewModel
{
    public int Id { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public int SeasonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public int EpisodeCount { get; set; }
}

public sealed class SeasonFormViewModel
{
    public int Id { get; set; }

    [Display(Name = "Series")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a series.")]
    public int SeriesId { get; set; }

    [Display(Name = "Season Number")]
    [Range(1, int.MaxValue)]
    public int SeasonNumber { get; set; } = 1;

    [Required]
    [StringLength(256)]
    public string Title { get; set; } = string.Empty;

    public List<SelectListItem> SeriesOptions { get; set; } = [];
}

public sealed class EpisodeListItemViewModel
{
    public int Id { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public string SeasonTitle { get; set; } = string.Empty;
    public int EpisodeNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
}

public sealed class EpisodeFormViewModel
{
    public int Id { get; set; }

    [Display(Name = "Season")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a season.")]
    public int SeasonId { get; set; }

    [Display(Name = "Episode Number")]
    [Range(1, int.MaxValue)]
    public int EpisodeNumber { get; set; } = 1;

    [Required]
    [StringLength(256)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Display(Name = "Video URL")]
    [Required]
    [StringLength(1024)]
    public string VideoUrl { get; set; } = string.Empty;

    [Display(Name = "Duration Seconds")]
    [Range(1, int.MaxValue)]
    public int DurationSeconds { get; set; }

    public List<SelectListItem> SeasonOptions { get; set; } = [];
}

public sealed class UserListItemViewModel
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SubscriptionCount { get; set; }
}

public sealed class UserFormViewModel
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Email Verified")]
    public bool IsEmailVerified { get; set; }
}

public sealed class SubscriptionPlanListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlanDuration Duration { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}

public sealed class SubscriptionPlanFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; } = string.Empty;

    public PlanDuration Duration { get; set; } = PlanDuration.Monthly;

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Display(Name = "Duration Days")]
    [Range(1, 5000)]
    public int DurationDays { get; set; } = 30;
}

public sealed class UserSubscriptionListItemViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool AutoRenew { get; set; }
}

public sealed class UserSubscriptionFormViewModel
{
    public int Id { get; set; }

    [Display(Name = "User")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a user.")]
    public int UserId { get; set; }

    [Display(Name = "Plan")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a plan.")]
    public int SubscriptionPlanId { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.PendingPayment;

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);

    [Display(Name = "Auto Renew")]
    public bool AutoRenew { get; set; } = true;

    [Display(Name = "Cancelled At")]
    public DateTime? CancelledAt { get; set; }

    public List<SelectListItem> UserOptions { get; set; } = [];
    public List<SelectListItem> PlanOptions { get; set; } = [];
}

public sealed class ReviewListItemViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }
    public string BodyPreview { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class ReviewFormViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }

    [Required]
    [StringLength(4000)]
    public string Body { get; set; } = string.Empty;

    [Display(Name = "Approved")]
    public bool IsApproved { get; set; }
}

public sealed class PromotedContentListItemViewModel
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public sealed class PromotedContentFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Label { get; set; } = string.Empty;

    [Display(Name = "Content Type")]
    public ContentType ContentType { get; set; } = ContentType.Movie;

    [Display(Name = "Content Id")]
    [Range(1, int.MaxValue)]
    public int ContentId { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}
