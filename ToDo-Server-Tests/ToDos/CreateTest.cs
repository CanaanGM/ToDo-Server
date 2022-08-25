using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;
using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server_Tests.ToDos
{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(user);
            context.SaveChangesAsync();

            var todo = new Create.Command
            {
                ToDo = new ToDoReadDto
                {
                    Name = "todo A",
                    Description = "test",
                    MarkedDone = DateTime.Today,
                    status = true,
                    Created = DateTime.Today,
                }
            };


            var sut = new Create.Handler(context,_mapper, userAccessor.Object);
            var result = sut.Handle(todo, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(todo.ToDo.Name, result.Value.Name);
            Assert.Equal(todo.ToDo.Description, result.Value.Description);
            Assert.Equal(todo.ToDo.Created, result.Value.Created);

        }
        [Fact]
        public void Should_Fail_No_User()
        {
            var context = GetDbContext();

            var todo = new Create.Command
            {
                ToDo = new ToDoReadDto
                {
                    Name = "todo A",
                    Description = "test",
                    MarkedDone = DateTime.Today,
                    status = true,
                    Created = DateTime.Today,
                }
            };

            var sut = new Create.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(todo, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

        }

        [Fact]
        public void Should_Fail_Something_Went_Wrong()
        {
            var context = GetDbContext();

            context.Users.AddAsync(user);
            context.SaveChangesAsync();

            var todo = new Create.Command
            {
                ToDo = new ToDoReadDto
                {
                    Name = "todo A",
                    Description = "test",
                    MarkedDone = DateTime.Today,
                    status = true,
                    Created = DateTime.Today,
                }
            };

            context.Remove(user);
            context.SaveChanges();

            var sut = new Create.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(todo, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

        }
    }
}
