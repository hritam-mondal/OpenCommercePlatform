using OCP.API.Data;
using OCP.API.DTOs;

namespace OCP.API.Repositories;

public class StatusRepository(ApplicationDbContext context) : IStatusRepository
{
    public async Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken ct)
    {
        return await context.Statuses
            .AsNoTracking()
            .OrderBy(s => s.StatusType)
            .ThenBy(s => s.StatusId)
            .Select(s => new StatusDto
            {
                StatusId = s.StatusId,
                StatusType = s.StatusType,
                StatusText = s.StatusText,
                Description = s.Description
            }).ToListAsync(ct);
    }
}
