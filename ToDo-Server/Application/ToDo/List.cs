using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ToDo_Server.Application.Core;
using ToDo_Server.Application.Interfaces;
using ToDo_Server.Data.DbAccess;
using ToDo_Server.Data.Mapper;

namespace ToDo_Server.Application.ToDo
{
    public class List
    {
        public class Query : IRequest<Result<List<ToDoReadDto>>>
        {
            public Guid ToDoId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ToDoReadDto>>>
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
            public async Task<Result<List<ToDoReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<List<ToDoReadDto>>.Failure("Bitch!");

                var todos = await _dataContext.ToDos
                    .Where(x => x.UserId == user.Id)
                    .ProjectTo<ToDoReadDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<ToDoReadDto>>.Success(_mapper.Map<List<ToDoReadDto>>(todos));
            }
        }
    }
}
