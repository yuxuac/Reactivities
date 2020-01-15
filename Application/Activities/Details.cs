
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using AutoMapper;

namespace Application.Activities
{
    public class Details
    {
        // Input
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }
        }

        // The method to get output
        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _datacontext;
            private readonly IMapper _mapper;
            public Handler(DataContext datacontext, IMapper mapper)
            {
                this._mapper = mapper;
                this._datacontext = datacontext;
            }

            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await this._datacontext.Activities
                    .FindAsync(request.Id);
                    
                    // .Include(x => x.UserActivities)
                    // .ThenInclude(x => x.AppUser)
                    // .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not found" });

                var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);

                return activityToReturn;
            }
        }
    }
}
