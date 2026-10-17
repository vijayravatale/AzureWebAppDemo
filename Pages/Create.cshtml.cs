using LeadDesk.Models;
using LeadDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadDesk.Pages;

public class CreateModel : PageModel
{
    private readonly ILeadStore _store;

    public CreateModel(ILeadStore store)
    {
        _store = store;
    }

    [BindProperty]
    public Lead Lead { get; set; } = new();

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _store.Add(Lead);
        TempData["Message"] = "Lead added.";
        return RedirectToPage("Index");
    }
}
