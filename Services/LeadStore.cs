using System.Collections.Concurrent;
using LeadDesk.Models;

namespace LeadDesk.Services;

public interface ILeadStore
{
    IReadOnlyList<Lead> All();
    Lead? Get(Guid id);
    Lead Add(Lead lead);
    bool Update(Lead lead);
    bool Delete(Guid id);
}

/// <summary>
/// Thread-safe in-memory store. No database needed; data resets when the app restarts.
/// </summary>
public class InMemoryLeadStore : ILeadStore
{
    private readonly ConcurrentDictionary<Guid, Lead> _leads = new();

    public InMemoryLeadStore()
    {
        Seed();
    }

    public IReadOnlyList<Lead> All() => _leads.Values.Select(l => l.Clone()).ToList();

    public Lead? Get(Guid id) => _leads.TryGetValue(id, out var lead) ? lead.Clone() : null;

    public Lead Add(Lead lead)
    {
        var now = DateTime.UtcNow;
        var copy = lead.Clone();
        copy.Id = Guid.NewGuid();
        copy.CreatedUtc = now;
        copy.UpdatedUtc = now;
        _leads[copy.Id] = copy;
        return copy.Clone();
    }

    public bool Update(Lead lead)
    {
        if (!_leads.TryGetValue(lead.Id, out var existing))
        {
            return false;
        }

        var copy = lead.Clone();
        copy.CreatedUtc = existing.CreatedUtc;
        copy.UpdatedUtc = DateTime.UtcNow;
        _leads[copy.Id] = copy;
        return true;
    }

    public bool Delete(Guid id) => _leads.TryRemove(id, out _);

    private void Seed()
    {
        var now = DateTime.UtcNow;
        AddSeed("Anita Deshpande", "Northwind Logistics", "anita@northwind-logistics.example", "+91 98200 11001",
            LeadStage.Qualified, 850000, "Wants a pilot across two warehouses before Q1.", now.AddDays(-1));
        AddSeed("Rohan Mehta", "Bluepeak Textiles", "rohan@bluepeak.example", "+91 98200 11002",
            LeadStage.New, 240000, "Inbound from the website form.", now.AddDays(-2));
        AddSeed("Sana Qureshi", "Kaveri Health Labs", "sana@kaverilabs.example", "+91 98200 11003",
            LeadStage.Contacted, 1200000, "Demo booked for Thursday.", now.AddDays(-3));
        AddSeed("Vikram Joshi", "Orbit Fabrication", "vikram@orbitfab.example", "+91 98200 11004",
            LeadStage.Won, 560000, "Signed annual plan.", now.AddDays(-6));
        AddSeed("Meera Iyer", "Lumen Education Trust", "meera@lumen-edu.example", "+91 98200 11005",
            LeadStage.Contacted, 310000, null, now.AddDays(-7));
        AddSeed("Arjun Patil", "Saffron Route Travels", "arjun@saffronroute.example", "+91 98200 11006",
            LeadStage.Lost, 180000, "Went with an in-house tool.", now.AddDays(-11));
        AddSeed("Neha Kulkarni", "Tanmay Auto Components", "neha@tanmayauto.example", "+91 98200 11007",
            LeadStage.Qualified, 970000, "Needs SSO and audit logs.", now.AddDays(-4));
    }

    private void AddSeed(string name, string company, string email, string phone,
        LeadStage stage, decimal value, string? notes, DateTime updated)
    {
        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = name,
            Company = company,
            Email = email,
            Phone = phone,
            Stage = stage,
            Value = value,
            Notes = notes,
            CreatedUtc = updated.AddDays(-10),
            UpdatedUtc = updated
        };
        _leads[lead.Id] = lead;
    }
}
