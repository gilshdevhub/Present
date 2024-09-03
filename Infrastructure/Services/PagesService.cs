using Core.Entities.PagesManagement;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PagesService : IPagesService
{
    private readonly SpecialDbContext _context;
    private readonly IdentityDbContext _identityDbContext;

    public PagesService(SpecialDbContext context, IdentityDbContext identityDbContext)
    {
        _context = context;
        _identityDbContext = identityDbContext;
    }


    public async Task<IEnumerable<Page>> GetPagesAsync()
    {
        var pages = await _identityDbContext
            .Page
            .Include(p => p.PageRoleNew)
            .ThenInclude(x => x.Role)
            .ToArrayAsync();
        return pages;
    }

    public async Task<IEnumerable<PageResponse>> GetPagesPerUserAsync(string UserName)
    {

        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@userName", Value =UserName }
            };

        try
        {
            IEnumerable<PageResponse> pages
                = await _context.PagesPerUsers
                .FromSqlRaw("exec [dbo].[sp_PagesPerUserNew]  @userName", sqlParameters.ToArray())
               .ToArrayAsync().ConfigureAwait(false);
            return pages;
        }
        catch (Exception ex)
        {
            return null;
        }
       
    }

                         public async Task<bool> AddPageAsync(Page pageToAdd)
    {
        _ = await _identityDbContext.Page.AddAsync(pageToAdd).ConfigureAwait(false);

        bool success = await _identityDbContext.SaveChangesAsync() > 0;

        return success;
    }

    public async Task<bool> UpdatePageAsync(Page pageToUpdate)
    {
        _ = _identityDbContext.Page.Attach(pageToUpdate);
        _identityDbContext.Entry(pageToUpdate).State = EntityState.Modified;
        bool success = await _identityDbContext.SaveChangesAsync() > 0;

        return success;
    }

    private async Task<int> CompleteAsync()
    {
        return await _identityDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeletePageAsync(int Id)
    {
        Page page = await _identityDbContext.Page.SingleOrDefaultAsync(p => p.Id == Id);
        bool success = false;
        if (page != null)
        {
            _ = _identityDbContext.Page.Remove(page);
            success = await _identityDbContext.SaveChangesAsync() > 0;
        }
        return success;
    }
}
