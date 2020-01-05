
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        // Input
        public class Query : IRequest<Activity>
        {
            public Guid Id { get; set; }
        }

        // The method to get output
        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _datacontext;
            public Handler(DataContext datacontext)
            {
                this._datacontext = datacontext;
            }
            
            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await this._datacontext.Activities.FindAsync(request.Id);
                return activity;
            }
        }
    }
}
