using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Mapper;
using ToDo_Server.Data.Models;

namespace ToDo_Server_Tests.ToDos
{
    public class EditTest : TestBase
    {
        [Fact]
        public async void Should_Edit()
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

            var UpdateTodo = new ToDoUpdateDto
            {
                Name = "todo AAA",
                Description = "test O O O",
                MarkedDone = DateTime.Today,
                status = false,
                Created = DateTime.Today,
            };

            context.ToDos.Add(ToDo);
            context.SaveChanges();


            var sut = new Edit.Handler(context,_mapper, userAccessor.Object);
            var result = sut.Handle(new Edit.Command { Id = id, ToDoDto=UpdateTodo }, CancellationToken.None).Result;
            var updatedTodo =await context.ToDos.FindAsync(id);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(UpdateTodo.Name, updatedTodo.Name);
            Assert.Equal(UpdateTodo.Description, updatedTodo.Description);
            Assert.Equal(UpdateTodo.status, updatedTodo.status);

        }
    }
}
