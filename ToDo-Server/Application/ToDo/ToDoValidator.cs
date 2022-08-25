using FluentValidation;

using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Application.ToDo
{
    public class ToDoValidator : AbstractValidator<ToDoReadDto>
    {
        public ToDoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            //TODO: fillout lator after adjusting/splitting the Dto
        }
    }


    public class ToDoUpdateValidator : AbstractValidator<ToDoUpdateDto>
    {
        public ToDoUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.status).NotEmpty();
            RuleFor(x => x.MarkedDone).NotEmpty();
        }
    }
}
