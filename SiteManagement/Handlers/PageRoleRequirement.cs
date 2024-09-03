using Core.Entities.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SiteManagement.Handlers;

public class PageRoleRequirement : IAuthorizationRequirement
{

}

public class PageRoleRequirementHandler : AuthorizationHandler<PageRoleRequirement>
{
    public readonly SpecialDbContext _dbContext;
    public PageRoleRequirementHandler(SpecialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, PageRoleRequirement requirement)
    {
        string host = (context.Resource as DefaultHttpContext).Request.Host.Value;
        if (host == "localhost:44347")
        {
            context.Succeed(requirement);
        }
        else
        {
            string[] uri = (context.Resource as DefaultHttpContext)?.Request.Path.Value.Split("/");
            string userName = context.User.FindFirst(x => x.Type == ClaimTypes.Email).Value;

            string controller = uri?[3];
            string method = (context.Resource as DefaultHttpContext)?.Request.Method;

            IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@userName", Value =userName },
                new SqlParameter { ParameterName = "@controller", Value =controller }
            };

            IList<PagesRoutesSP> result = await _dbContext.Access
               .FromSqlRaw("exec [dbo].[sp_IsUserPagesAccessNew]  @userName, @controller", sqlParameters.ToArray())
               .ToArrayAsync().ConfigureAwait(false);

            if (result.Any())
            {
                string allowedMethods = "";

                var temp = result.Where(x => x.updatable == true).FirstOrDefault();
                if (temp != null)
                {
                    allowedMethods = "GET,PUT,POST,DELETE";
                }
                else
                {
                    var temp2 = result.Where(x => x.readable == true).FirstOrDefault();
                    if (temp != null)
                    {
                        allowedMethods = "GET";
                    }
                }
                if (allowedMethods.Contains(method))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
        return Task.CompletedTask;
    }
}