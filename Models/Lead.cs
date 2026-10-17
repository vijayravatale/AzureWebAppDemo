using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LeadDesk.Models;

public enum LeadStage
{
    New,
    Contacted,
    Qualified,
    Won,
    Lost
}

public class Lead
{
    // Indian digit grouping: 12,34,567
    private static readonly NumberFormatInfo Inr = new() { NumberGroupSizes = new[] { 3, 2 } };

    public Guid Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Company { get; set; }

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [Phone, StringLength(30)]
    public string? Phone { get; set; }

    public LeadStage Stage { get; set; } = LeadStage.New;

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal? Value { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime UpdatedUtc { get; set; }

    public string FormattedValue =>
        Value.HasValue ? "₹" + Value.Value.ToString("N0", Inr) : "—";

    public Lead Clone() => (Lead)MemberwiseClone();
}
