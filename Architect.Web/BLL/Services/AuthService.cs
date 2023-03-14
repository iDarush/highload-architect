using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Architect.Web.BLL.Exceptions;
using Architect.Web.BLL.Models;
using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.DAL.Models;
using Architect.Web.DAL.Repositories.Interfaces;
using Architect.Web.Dto.Requests;
using Architect.Web.Security;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Architect.Web.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserRegisterRequest> _passwordHasher;
    private readonly AuthOptions _authOptions;

    public AuthService(
        IUserRepository userRepository,
        IOptionsSnapshot<AuthOptions> authOptions,
        IPasswordHasher<UserRegisterRequest> passwordHasher)
    {
        authOptions.Value.Validate();

        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _authOptions = authOptions.Value;
    }

    public async Task<string> Login(
        Guid userId,
        string password,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user == null)
        {
            throw new InvalidCredentialsException(InvalidCredentialsException.State.InvalidUserId);
        }

        var result = _passwordHasher.VerifyHashedPassword(
            UserRegisterRequest.Empty(password),
            hashedPassword: user.PasswordHash,
            providedPassword: password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException(InvalidCredentialsException.State.InvalidPassword);
        }

        var userModel = user.Adapt<UserModel>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = userModel.ClaimsIdentity,
            Expires = DateTime.UtcNow.AddDays(_authOptions.TokenLifetimeInDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }

    public async Task<Guid> Register(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>() with
        {
            PasswordHash = _passwordHasher.HashPassword(request, request.Password)
        };

        var guid = await _userRepository.Insert(user, cancellationToken);
        return guid;
    }
}
