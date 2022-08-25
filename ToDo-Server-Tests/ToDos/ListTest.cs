using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Models;

namespace ToDo_Server_Tests.ToDos
{
    public class ListTest: TestBase
    {
        [Fact]
        public void Should_Get_List()
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
                status = false,
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


            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("todo A", result.Value[0].Name);
            Assert.Equal("test", result.Value[0].Description);
            Assert.False(result.Value[0].status);

        }

        [Fact]
        public void Should_Fail_2_Get_List_No_user()
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

            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
