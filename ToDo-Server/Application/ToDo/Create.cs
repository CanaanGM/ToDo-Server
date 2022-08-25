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
    public class Create 
    {
        public class Command : IRequest<Result<ToDoDTO>>
        {
            public ToDoDTO? ToDo { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(x => x.ToDo).SetValidator(new ToDoValidator());
        }

        public class Handler : IRequestHandler<Command, Result<ToDoDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor )
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<ToDoDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return Result<ToDoDTO>.Failure("Failed to get user");

                var newTodo = _mapper.Map<Data.Models.ToDo>(request.ToDo );
                user.ToDos.Add(newTodo);

                _context.ToDos.Add(newTodo);
                var sucess = await _context.SaveChangesAsync() > 0;

                return sucess
                    ? Result<ToDoDTO>.Success(_mapper.Map<ToDoDTO>(newTodo))
                    : Result<ToDoDTO>.Failure("Failed to create to do");
            }
        }
    }
}
