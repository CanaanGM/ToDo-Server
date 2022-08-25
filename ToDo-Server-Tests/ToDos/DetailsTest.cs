using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDo_Server.Application.Exceptions;
using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Models;

namespace ToDo_Server_Tests.ToDos
{
    public class DetailsTest : TestBase
    {
        [Fact]
        public void Should_Get_Details()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Users.AddAsync(user);
            context.SaveChangesAsync();
            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = id,
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "1"

            });
            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = new Guid(),
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "14"

            });

            context.SaveChanges();


            var sut = new Details.Handler(context,_mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query{ Id=id }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("todo A", result.Value.Name);
            Assert.Equal("test", result.Value.Description);
            Assert.Equal(true, result.Value.status);

        }
        [Fact]
        public void Should_Fail_2_Get_Details_No_User()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = id,
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "1"

            });
            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = new Guid(),
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "14"

            });

            context.SaveChanges();


            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = id }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

        }

        [Fact]
        public void Should_Fail_2_Get_Details_Something_Went_Wrong()
        {
            var context = GetDbContext();
            context.Users.AddAsync(user);
            context.SaveChangesAsync();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = id,
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,

            });
            context.ToDos.Add(new ToDo_Server.Data.Models.ToDo
            {
                Id = new Guid(),
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "14"

            });

            context.SaveChanges();
            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = id }, CancellationToken.None).Result;
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

        }

        [Fact]
        public async Task Should_Return_404_If_Not_Found()
        {
            var context = GetDbContext();

            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = new Guid() }, CancellationToken.None);

            await Assert.ThrowsAsync<RestException>(() => result);
        }
    }
}
