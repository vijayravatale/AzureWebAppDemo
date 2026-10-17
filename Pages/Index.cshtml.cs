using LeadDesk.Models;
using LeadDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadDesk.Pages;

public class IndexModel : PageModel
{
    private readonly ILeadStore _store;

    public IndexModel(ILeadStore store)
    {
        _store = store;
    }

    [BindProperty(SupportsGet = true)]
    public string? Q { get; set; }

    [BindProperty(SupportsGet = true)]
    public LeadStage? Stage { get; set; }

    public List<Lead> Leads { get; private set; } = new();

    public Dictionary<LeadStage, int> Counts { get; private set; } = new();

    public int Total { get; private set; }

    public void OnGet()
    {
        Load();
    }

    public IActionResult OnPostDelete(Guid id)
    {
        if (_store.Delete(id))
        {
            TempData["Message"] = "Lead deleted.";
        }

        return RedirectToPage(new { Q, Stage });
    }

    private void Load()
    {
        var all = _store.All();
        Total = all.Count;

        foreach (var stage in Enum.GetValues<LeadStage>())
        {
            Counts[stage] = all.Count(l => l.Stage == stage);
        }

        IEnumerable<Lead> query = all;

        if (Stage.HasValue)
        {
            query = query.Where(l => l.Stage == Stage.Value);
        }

        if (!string.IsNullOrWhiteSpace(Q))
        {
            var term = Q.Trim();
            query = query.Where(l =>
                Has(l.Name, term) || Has(l.Company, term) || Has(l.Email, term));
        }

        Leads = query.OrderByDescending(l => l.UpdatedUtc).ToList();
    }

    private static bool Has(string? value, string term) =>
        value is not null && value.Contains(term, StringComparison.OrdinalIgnoreCase);
}
