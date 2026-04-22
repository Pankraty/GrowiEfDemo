using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace GrowiEfDemo.Queries;

public static class SearchCompanies
{
    public record Request(
        string? SearchText,
        int Limit = 50,
        int Offset = 0);

    public record Response(int TotalCount, CompanyModel[] Companies);
    public record CompanyModel(int Id, string Name, string Inn, string Ogrn);

    public class Handler(DemoDbContext dbContext) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Get("/companies");
            AllowAnonymous();
        }
        
        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var query = dbContext.Companies.AsNoTracking();
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(c => 
                    c.Name.Contains(request.SearchText) ||
                    c.Inn.StartsWith(request.SearchText) ||
                    c.Ogrn.StartsWith(request.SearchText));
            }
            var totalCount = await query.CountAsync(ct);
            var companies = await query.OrderBy(c => c.Id)
                .Skip(request.Offset)
                .Take(request.Limit)
                .Select(c => new CompanyModel(c.Id, c.Name, c.Inn, c.Ogrn))
                .ToArrayAsync(ct);
            
            var response = new Response(totalCount, companies);
            await Send.ResponseAsync(response, cancellation: ct);
        }
    }
}