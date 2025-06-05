using Microsoft.EntityFrameworkCore;
using OrderService.Api.Data;
using OrderService.Api.DTOs;

namespace OrderService.Api.Services;

public interface IDeadLetterService
{
    Task<IEnumerable<DeadLetterDto>> GetAllAsync();
}

public class DeadLetterService : IDeadLetterService
{
    private readonly OrderDbContext _db;

    public DeadLetterService(OrderDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DeadLetterDto>> GetAllAsync()
    {
        return await _db.DeadLetters
            .OrderByDescending(dl => dl.DeadLetteredAt)
            .Select(dl => new DeadLetterDto
            {
                Id = dl.Id,
                MessageId = dl.MessageId,
                Body = dl.Body,
                DeadLetteredAt = dl.DeadLetteredAt,
                ErrorReason = dl.ErrorReason,
                ErrorDescription = dl.ErrorDescription
            }).ToListAsync();
    }
}
