
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<UserInfo>
        {

        }

        public class Handler : IRequestHandler<Query, UserInfo>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._jwtGenerator = jwtGenerator;
                this._userManager = userManager;
            }

            public async Task<UserInfo> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new UserInfo
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }
        }
    }
}