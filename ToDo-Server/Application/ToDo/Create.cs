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
        public class Command : IRequest<Result<ToDoReadDto>>
        {
            public ToDoReadDto? ToDo { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(x => x.ToDo).SetValidator(new ToDoValidator());
        }

        public class Handler : IRequestHandler<Command, Result<ToDoReadDto>>
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

            public async Task<Result<ToDoReadDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return Result<ToDoReadDto>.Failure("Failed to get user");

                var newTodo = _mapper.Map<Data.Models.ToDo>(request.ToDo );
                user.ToDos.Add(newTodo);

                _context.ToDos.Add(newTodo);
                var sucess = await _context.SaveChangesAsync() > 0;

                return sucess
                    ? Result<ToDoReadDto>.Success(_mapper.Map<ToDoReadDto>(newTodo))
                    : Result<ToDoReadDto>.Failure("Failed to create to do");
            }
        }
    }
}
