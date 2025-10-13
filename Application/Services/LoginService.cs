using Application.DTOs;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public interface ILoginService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
}

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public LoginService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var usuario = await _unitOfWork.Usuarios.GetByCorreoAsync(loginDto.Correo);
        
        if (usuario == null)
            return null;

        var hashedPassword = PasswordHelper.HashPassword(loginDto.Password);
        
        if (usuario.PasswordHash != hashedPassword)
            return null;

        var token = GenerateJwtToken(usuario);
        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

        return new AuthResponseDto
        {
            Token = token,
            Usuario = usuarioDto
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _unitOfWork.Usuarios.ExisteCorreoAsync(registerDto.Correo))
            return null;

        var usuario = _mapper.Map<Usuario>(registerDto);
        usuario.PasswordHash = PasswordHelper.HashPassword(registerDto.Password);
        usuario.FechaCreacion = DateTime.UtcNow;

        await _unitOfWork.Usuarios.AddAsync(usuario);
        await _unitOfWork.CommitAsync();

        var token = GenerateJwtToken(usuario);
        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

        return new AuthResponseDto
        {
            Token = token,
            Usuario = usuarioDto
        };
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}