using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vets.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vets.Queries
{
    public class GetVetStatisticsQuery : IRequest<VetStatisticsDto>
    {
        public int VetId { get; set; }
    }

    public class GetVetStatisticsHandler : IRequestHandler<GetVetStatisticsQuery, VetStatisticsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetVetStatisticsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VetStatisticsDto> Handle(GetVetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var stats = await _context.VetStatistics
                .FromSqlInterpolated($"EXEC sp_GetVetStatistics {request.VetId}")
                .ToListAsync(cancellationToken);

            return stats.FirstOrDefault() ?? new VetStatisticsDto();
        }
    }
}
