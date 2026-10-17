using LeadDesk.Models;
using LeadDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadDesk.Pages;

public class EditModel : PageModel
{
    private readonly ILeadStore _store;

    public EditModel(ILeadStore store)
    {
        _store = store;
    }

    [BindProperty]
    public Lead Lead { get; set; } = new();

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

    public IActionResult OnPost(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Lead.Id = id;
        if (!_store.Update(Lead))
        {
            return NotFound();
        }

        TempData["Message"] = "Changes saved.";
        return RedirectToPage("Index");
    }
}
