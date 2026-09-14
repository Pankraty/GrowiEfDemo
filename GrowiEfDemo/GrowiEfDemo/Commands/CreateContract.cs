using FastEndpoints;

namespace GrowiEfDemo.Commands;

public static class CreateContract
{
    public record Request(decimal Rate, RateMode RateMode);

    public record Response(string Key, decimal Rate, RateMode RateMode);

    public class Handler(DemoDbContext dbContext) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/contracts");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var contract = new Contract(0, GenRandomKey(6), request.Rate, request.RateMode);
            dbContext.Contracts.Add(contract);
            await dbContext.SaveChangesAsync(ct);
            await Send.ResponseAsync(new Response(contract.Key, contract.Rate, contract.RateMode), cancellation: ct);
        }
        
        private string GenRandomKey(int length) => 
            string.Concat(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                .Select(s => s[Random.Shared.Next(s.Length)]));
    }
}