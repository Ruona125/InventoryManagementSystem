using WebAPI.DTOs;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
}
