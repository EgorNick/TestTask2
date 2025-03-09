using TestController.Models;

namespace TestController.Services;

public interface IUserService
{
    List<UserResponse> ProcessUsers(List<User> users);
}