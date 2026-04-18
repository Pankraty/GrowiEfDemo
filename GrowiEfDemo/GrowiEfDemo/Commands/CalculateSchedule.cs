using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace GrowiEfDemo.Commands;

public static class CalculateSchedule
{
    public record Request([property:FromRoute, SwaggerIgnore]string ContractKey, DateOnly StartDate, int NumberOfInstallments, decimal Amount);

    public record Response(Installment[] Installments)
    {
        public decimal TotalInterest => Installments.Sum(i => i.Interest);
        public decimal TotalAmount => Installments.Sum(i => i.Principal + i.Interest);
    };
    
    public record Installment(DateOnly Date, decimal Principal, decimal Interest);

    public class Handler(DemoDbContext dbContext) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/contracts/{contractKey}/schedules");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var contract = await dbContext.Contracts
                    .FirstOrDefaultAsync(c => c.Key == request.ContractKey, ct) ;
            
            if (contract == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            var installments = CalculateInstallments(request.StartDate, request.NumberOfInstallments, request.Amount, contract.Rate);
            
            await Send.ResponseAsync(new Response(installments), cancellation: ct);
        }

        private static Installment[] CalculateInstallments(DateOnly startDate, int numberOfInstallments,
            decimal amount, decimal rate)
        {
            var installments = new Installment[numberOfInstallments];
            var monthlyRate = rate / 12 / 100;

            #region Unnecessary details
            var annuityCoefficient = monthlyRate * (1m + 1m / (Pow(1m + monthlyRate, numberOfInstallments) - 1m));
            var monthlyPayment = decimal.Round(amount * annuityCoefficient, 0);
            var remainingBalance = amount;
            for (var i = 0; i < numberOfInstallments; i++)
            {
                var date = startDate.AddMonths(i);
                var interestPayment = decimal.Round(remainingBalance * monthlyRate, 0);
                var principalPayment = monthlyPayment - interestPayment;

                // Adjust last payment to handle rounding differences
                if (i == numberOfInstallments - 1)
                {
                    principalPayment = remainingBalance;
                }

                remainingBalance -= principalPayment;
                installments[i] = new Installment(date, principalPayment, interestPayment);
            }

            return installments;

            static decimal Pow(decimal x, int y)
            {
                if (y < 0) throw new NotSupportedException("Negative powers are not supported by the method");
                var result = 1.0m;
                for (int i = 0; i < y; i++) result *= x;
                return result;
            }
            #endregion
        }
    }
}
