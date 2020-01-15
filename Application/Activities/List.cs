
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly ILogger<List> _logger;
            private readonly IMapper _mapper;
            public Handler(DataContext context, ILogger<List> logger, IMapper mapper)
            {
                this._mapper = mapper;
                this._logger = logger;
                this._context = context;
            }

            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                                .ToListAsync(cancellationToken);
                    // .Include(x => x.UserActivities)
                    // .ThenInclude(x => x.AppUser)
                    // .ToListAsync(cancellationToken);

                // List<ActivityDto> result = new List<ActivityDto>();
                // foreach (var activity in activities)
                // {
                //     var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);
                //     result.Add(activityToReturn);
                // }

                return _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            }
        }
    }
}