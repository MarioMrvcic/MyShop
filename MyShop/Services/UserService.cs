using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class UserService : IUserService
{
    private readonly IDbContextFactory<AplicationDbContext> _dbContextFactory;

    public UserService(IDbContextFactory<AplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ShopAppWebUser> GetUserByNameAsync(string userName)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);

        return user;
    }
}