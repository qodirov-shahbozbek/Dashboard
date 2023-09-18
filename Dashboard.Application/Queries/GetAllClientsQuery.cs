using Dashboard.Application.Abstractions;
using Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Application.Queries
{
    public class GetAllClientsQuery : IQuery<List<Client>>
    {       
    }

    public class GetAllClientsQueryHandler : IQueryHandler<GetAllClientsQuery, List<Client>>
    {

        private readonly IApplicationDbContext _dbContext;

        public GetAllClientsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Client>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Clients.Select(x => new Client()
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Balance = x.Balance
            }
            ).ToListAsync(cancellationToken);
        }
    }
}
