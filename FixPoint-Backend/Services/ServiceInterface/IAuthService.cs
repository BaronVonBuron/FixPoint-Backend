using FixPoint_Backend.DataAccess;

namespace FixPoint_Backend.Services.ServiceInterface;

public interface IAuthService
{
    string Login(LoginDTO loginDto);
}