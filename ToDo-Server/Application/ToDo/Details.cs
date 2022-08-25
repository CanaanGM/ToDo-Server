using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ToDo_Server.Application.Core;
using ToDo_Server.Application.Exceptions;
using ToDo_Server.Application.Interfaces;
using ToDo_Server.Data.DbAccess;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Application.ToDo
{
    public class Details
    {
        public class Query : IRequest<Result<ToDoReadDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ToDoReadDto>>
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
            public async Task<Result<ToDoReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
                     x.Id == _userAccessor.GetUserId());

                var todo = await _dataContext.ToDos
                    .FindAsync(request.Id);
                if (todo == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { ToDo = "Not Found" });
                if (user == null || todo.UserId != user.Id)
                    return Result<ToDoReadDto>.Failure("bitch");




                var toDo2Return = await _dataContext.ToDos
                    .ProjectTo<ToDoReadDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (todo.UserId == user.Id)
                    return Result<ToDoReadDto>.Success(toDo2Return);
                return Result<ToDoReadDto>.Failure("something went wrong ;=;");
            }
        }
    }
}
