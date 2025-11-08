using Npgsql;
using OCP.API.Data;
using OCP.API.DTOs;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<int> RegisterUserAsync(
        User user,
        string passwordHash,
        string passwordSalt,
        Guid token,
        DateTime expiresAt,
        CancellationToken ct)
    {
        try
        {
            if (await context.Users.AnyAsync(u => u.Email == user.Email, ct))
            {
                return 0;
            }
            if (await context.Users.AnyAsync(u => u.Username == user.Username, ct))
            {
                return -2;
            }
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.EmailConfirmToken = token;
            user.EmailConfirmTokenExpires = expiresAt;

            // Add and save
            await context.Users.AddAsync(user, ct);
            await context.SaveChangesAsync(ct);
    
            return 1;
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException is PostgresException { SqlState: "23505" })
            {
                logger.LogWarning(dbEx, "Unique constraint violation while registering user {Email}", user.Email);
                return 0;
            }

            logger.LogError(dbEx, "Database error during user registration for {Email}", user.Email);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during user registration for {Email}", user.Email);
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .SingleOrDefaultAsync();
    }

    public async Task<List<User>> GetAllAsync(UserFilterDto filter)
    {
        // Start with base query
        var query = context.Users.AsQueryable();

        // Apply filters dynamically
        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.FirstName, $"%{filter.Query}%") ||
                EF.Functions.ILike(u.LastName, $"%{filter.Query}%") ||
                EF.Functions.ILike(u.Email, $"%{filter.Query}%"));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == filter.IsActive.Value);
        }

        // Apply paging
        var limit = Math.Clamp(filter.PageSize, 1, 1000);
        var offset = Math.Max(0, (filter.Page - 1) * filter.PageSize);

        var results = await query
            .OrderByDescending(u => u.UserId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return results;
    }

    public async Task UpdateAsync(User user)
    {
        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return false;
        }

        context.Users.Remove(user);
        var affected = await context.SaveChangesAsync();
        return affected > 0;
    }

    public async Task<AuthUserDto?> GetAuthUserAsync(string emailOrUsername, CancellationToken ct)
    {
        var normalizedInput = emailOrUsername.ToLower(); 
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Email.ToLower().Equals(normalizedInput) || u.Username.ToLower().Equals(normalizedInput))
            .Select(u => new AuthUserDto(
                u.UserId,
                u.Username,
                u.Email,
                u.PasswordHash,
                u.PasswordSalt,
                u.IsActive,
                u.IsConfirmed,
                u.FirstName,
                u.LastName,
                u.IsAdmin
            ))
            .FirstOrDefaultAsync(ct);
    }
}
