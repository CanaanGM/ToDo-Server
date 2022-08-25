using Microsoft.AspNetCore.Mvc;
using ToDo_Server.Application.ToDo;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Controllers
{
    public class ToDoController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoDTO todo)
        {
            return HandleResult(await Mediator.Send(new Create.Command { ToDo= todo}));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ToDoDTO todo)
        {

            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, ToDoDto= todo }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}
