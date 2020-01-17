using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == _userAccessor.GetCurrentUsername());
                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Profile = "Not found." });

                if (user.DisplayName != request.DisplayName || user.Bio != request.Bio)
                {
                    user.DisplayName = request.DisplayName ?? user.DisplayName;
                    user.Bio = request.Bio ?? user.Bio;
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success) return Unit.Value;

                    throw new Exception("Problem saving changes");
                }
                else
                {
                    Console.WriteLine("No change.");
                    return Unit.Value;
                }
            }
        }
    }
}