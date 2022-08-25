using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Moq;

using ToDo_Server.Application.Interfaces;
using ToDo_Server.Controllers;
using ToDo_Server.Data.DbAccess;
using ToDo_Server.Data.Mapper;
using ToDo_Server.Data.Models;
using ToDo_Server.Infrastructure.Security;

namespace ToDo_Server_Tests
{
    public class TestBase
    {
        public Mock<IHttpContextAccessor> httpContextAccessorMock;
        public Mock<IConfiguration> ConfigurationMock;
        public Mock<UserManager<AppUser>> UserManagerMock;
        public Mock<SignInManager<AppUser>> SignInManagerMock;
        public Mock<ITokenService> TokenServiceMock;
        public Mock<IUserAccessor> userAccessor;
        public readonly IMapper _mapper;
        public AppUser user;
        public TestBase()
        {
            user = new AppUser()
            {
                UserName = "test",
                Email = "test@example.com",
                Id = "1",
                DisplayName = "test"
            };

            httpContextAccessorMock = new Mock<IHttpContextAccessor>();


            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = mockMapper.CreateMapper();

            userAccessor = new Mock<IUserAccessor>();
            userAccessor.Setup(x => x.GetUsername()).Returns("test");
            userAccessor.Setup(x => x.GetUserId()).Returns("1");


            UserManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            UserManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<AppUser>(user));


            SignInManagerMock = new Mock<SignInManager<AppUser>>(
                UserManagerMock.Object,
                httpContextAccessorMock.Object,
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null,
                null,
                null,
                null
            );
            SignInManagerMock.Setup(x =>
                    x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false))
                .Returns(It.IsAny<Task<SignInResult>>());

            SignInManagerMock.Setup(x =>
                    x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false))
                .Returns(Task.FromResult<SignInResult>(SignInResult.Success));

            ConfigurationMock = new Mock<IConfiguration>();
            ConfigurationMock.Setup(x => x[It.IsAny<string>()]).Returns("Is it secret? is it safe?");


            //TokenServiceMock = new Mock<ITokenService>();
            //TokenServiceMock.Setup(x => x.CreateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(GenToken(user));
            //TokenServiceMock.Setup(x => x.GenerateRefreshToken())
            //    .Returns(new RefreshToken() { AppUser = user, Token = GenToken(user), Expires = DateTime.UtcNow, Revoked = DateTime.UtcNow });


        }

        public DataContext GetDbContext(bool useSqlite = false)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            if (useSqlite)
            {
                builder.UseSqlite("Data Source=:memory", x => { });
            }
            else
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var dbContext = new DataContext(builder.Options);

            if (useSqlite)
            {
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}