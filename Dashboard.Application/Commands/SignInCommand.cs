using Dashboard.Application.Abstractions;
using Dashboard.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dashboard.Application.Commands
{
    public class SignInCommand : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IHashService _hashService;

        public SignInCommandHandler(IApplicationDbContext context, IHashService hashService, ITokenService tokenService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

            if (user == null)
            {
                throw new Exception(nameof(User));
            }

            if (user.Password != _hashService.GetHash(request.Password))
            {
                throw new Exception();
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName)
            };

           return _tokenService.GetToken(claims.ToArray());
        }
    }
}
