using DoAn.Application.Abstractions;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserResponse>> GetUserHasRole(Guid roleId, CancellationToken cancellationToken = default)
    {

        var query = (from ur in _db.UserRoles.AsQueryable()
                join u in _db.AppUses on ur.UserId equals u.Id into users
                from user in users
                where ur.RoleId == roleId
                select new UserResponse()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    
                }
            );
        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<UserResponse> GetUsersAsync(CancellationToken cancellationToken)
    {
        var query = (from u in _db.AppUses.AsQueryable()
                where  !u.IsDeleted
                select new UserResponse()
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    Roles = (from ur in _db.UserRoles.AsQueryable()
                        join r in _db.Roles.AsQueryable() on ur.RoleId equals r.Id
                        where ur.UserId == u.Id
                        select r.Name).ToList()
                }
            );
        return query;
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var query = (from u in _db.AppUses.AsQueryable()
                where !u.IsDeleted && u.Id == Id
                select new UserResponse()
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    Roles = (from ur in _db.UserRoles.AsQueryable()
                        join r in _db.Roles.AsQueryable() on ur.RoleId equals r.Id
                        where ur.UserId == u.Id
                        select r.RoleCode).ToList()
                }
            );
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool?> RemoveRoleInUser(Guid id, CancellationToken cancellationToken)
    {
        var userRoles = await _db.UserRoles.Where(x => x.UserId == id).ToListAsync(cancellationToken);
        _db.UserRoles.RemoveRange(userRoles);


        return await _db.SaveChangesAsync(cancellationToken) > 0;
    }
    public async Task<List<string>> GetRoleCodeByUser(Guid id, CancellationToken cancellationToken)
    {
        var queryable = from ur in _db.UserRoles.AsQueryable()
            join r in _db.AppRoles.AsQueryable() on ur.RoleId equals r.Id 
            
            where ur.UserId == id
            select r.RoleCode ;

        var userRoles = await queryable.ToListAsync(cancellationToken);

        return userRoles;
    }

    public async Task<List<string>> GetRoleIdsInUser(Guid id, CancellationToken cancellationToken)
    {
        var query = from ur in _db.UserRoles.AsQueryable()
            join r in _db.Roles.AsQueryable() on ur.RoleId equals r.Id
            where ur.UserId == id
            select r.Id.ToString().ToLower();
        return await query.ToListAsync(cancellationToken);
    }
} 