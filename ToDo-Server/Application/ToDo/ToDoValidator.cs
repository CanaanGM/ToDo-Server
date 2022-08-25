using FluentValidation;

using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Application.ToDo
{
    public class ToDoValidator : AbstractValidator<ToDoDTO>
    {
        public ToDoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            //TODO: fillout lator after adjusting/splitting the Dto
        }
    }
}
