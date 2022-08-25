using AutoMapper;
using FluentValidation;
using MediatR;

using Microsoft.EntityFrameworkCore;

using ToDo_Server.Application.Core;
using ToDo_Server.Application.Interfaces;
using ToDo_Server.Data.DbAccess;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Application.ToDo
{
    public class Edit
    {


        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public ToDoDTO ToDoDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(x => x.ToDoDto).SetValidator(new ToDoValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var todo = await _dataContext.ToDos.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
                                 x.Id == _userAccessor.GetUserId());

                if (user == null || todo == null || todo.UserId != user.Id)
                    return Result<Unit>.Failure("Failed to edit todo");


                _mapper.Map(request.ToDoDto, todo);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit todo")
                          : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}