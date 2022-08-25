using MediatR;

using Microsoft.EntityFrameworkCore;

using ToDo_Server.Application.Core;
using ToDo_Server.Application.Interfaces;
using ToDo_Server.Data.DbAccess;

namespace ToDo_Server.Application.ToDo
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var todo2delete = await _dataContext.ToDos.FindAsync(request.Id);
                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());

                if (todo2delete == null || todo2delete.UserId != user?.Id)
                    return Result<Unit>.Failure("Error Deleting. . .");

                _dataContext.Remove(todo2delete);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                    ? Result<Unit>.Failure("Failed to Delete Todo")
                    : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}