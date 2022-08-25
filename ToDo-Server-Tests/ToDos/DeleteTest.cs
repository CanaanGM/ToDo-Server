using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server_Tests.ToDos
{
    public class DeleteTest : TestBase
    {
        [Fact]
        public void Should_Delete()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Users.AddAsync(user);
            context.SaveChangesAsync();

            var ToDo = new ToDo_Server.Data.Models.ToDo
            {
                    Id = id,
                    Name = "todo A",
                    Description = "test",
                    MarkedDone = DateTime.Today,
                    status = true,
                    Created = DateTime.Today,
                    UserId = "1"
                
            };

            context.ToDos.Add(ToDo);
            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id=id}, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Null(context.ToDos.Find(id));


        }

        [Fact]
        public void Should_Fail_2_Delete_No_User()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            var ToDo = new ToDo_Server.Data.Models.ToDo
            {
                Id = id,
                Name = "todo A",
                Description = "test",
                MarkedDone = DateTime.Today,
                status = true,
                Created = DateTime.Today,
                UserId = "1"

            };

            context.ToDos.Add(ToDo);
            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.NotNull(context.ToDos.Find(id));

        }
    }
}
