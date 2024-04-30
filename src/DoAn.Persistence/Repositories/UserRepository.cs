using DoAn.Application.Abstractions;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<UserResponse> GetUsersAsync()
    {
        var query = (from u in _db.AppUses.AsQueryable()
                where !u.IsDirector
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
}