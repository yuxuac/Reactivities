using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Validators;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<UserInfo>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();//.Matches("^[0-9a-zA-Z]+$").WithMessage("Username only accept digits and chars.");
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, UserInfo>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jetGenerator;
            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jetGenerator)
            {
                _jetGenerator = jetGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<UserInfo> Handle(Command request, CancellationToken cancellationToken)
            {
                // handler logic
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = "UserName already exists" });

                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new UserInfo
                    {
                        DisplayName = user.DisplayName,
                        Token = _jetGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
                    };
                }
                else if (result.Errors.Any(err => err is IdentityError))
                {
                    var error = result.Errors.FirstOrDefault(err => err is IdentityError);
                    var errorMsg = error.Code + "-" + error.Description;
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = errorMsg });
                }

                throw new Exception("Problem creating user");
            }
        }
    }
}