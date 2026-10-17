using LeadDesk.Models;
using LeadDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadDesk.Pages;

public class DetailsModel : PageModel
{
    private readonly ILeadStore _store;

    public DetailsModel(ILeadStore store)
    {
        _store = store;
    }

    public Lead Lead { get; private set; } = new();

    public IActionResult OnGet(Guid id)
    {
        var existing = _store.Get(id);
        if (existing is null)
        {
            return NotFound();
        }

        Lead = existing;
        return Page();
    }
}
