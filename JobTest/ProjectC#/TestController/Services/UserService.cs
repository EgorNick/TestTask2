using System.Text.Json;
using TestController.Models;
using TestController.Validation;

namespace TestController.Services;

public class UserService : IUserService
{
    private static readonly string JsonPath = Path.Combine(AppContext.BaseDirectory, "test.json");

    private static List<User> _allUsers = new List<User>();

    private static void LoadUsers()
    {
        if (_allUsers.Count == 0)
        {
            try
            {
                var json = File.ReadAllText(JsonPath);
                _allUsers = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                Console.WriteLine($"[INFO] Загружено {_allUsers.Count} пользователей из {JsonPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка загрузки JSON: {ex.Message}");
                _allUsers = new List<User>();
            }
        }
    }

    public List<UserResponse> ProcessUsers(List<User> users)
    {
        LoadUsers();

        var combinedUsers = CombineUsers(users);

        return combinedUsers.Select(user => ProcessUser(user, combinedUsers)).ToList();
    }

    private List<User> CombineUsers(List<User> users)
    {
        var userDict = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        foreach (var user in _allUsers.Concat(users))
        {
            var name = user.Name?.Trim();
            if (string.IsNullOrEmpty(name)) continue;

            if (!userDict.ContainsKey(name))
            {
                userDict[name] = new User
                {
                    Name = name,
                    Phone = user.Phone,
                    Email = user.Email,
                    Friends = new List<Friend>()
                };
            }

            if (user.Friends != null)
            {
                foreach (var friend in user.Friends)
                {
                    var friendName = friend.Name?.Trim();
                    if (!string.IsNullOrEmpty(friendName) &&
                        !userDict[name].Friends.Any(f => f.Name.Equals(friendName, StringComparison.OrdinalIgnoreCase)))
                    {
                        userDict[name].Friends.Add(new Friend { Name = friendName });
                    }
                }
            }
        }

        return userDict.Values.ToList();
    }

    private static UserResponse ProcessUser(User user, List<User> allUsers)
    {
        var userFriendsMap = allUsers
            .Where(u => !string.IsNullOrEmpty(u.Name))
            .ToDictionary(
                u => u.Name!.Trim().ToLower(),
                u => new HashSet<string>(u.Friends?.Select(f => f.Name!.Trim().ToLower()) ?? Enumerable.Empty<string>())
            );

        var friendPairs = new List<string>();

        if (user.Friends != null)
        {
            foreach (var friend in user.Friends)
            {
                var friendName = friend.Name?.Trim().ToLower();
                if (!string.IsNullOrEmpty(friendName) && userFriendsMap.ContainsKey(friendName))
                {
                    var currentUser = user.Name!.Trim().ToLower();

                    if (userFriendsMap[friendName].Contains(currentUser))
                    {
                        friendPairs.Add($"{user.Name} - {friend.Name}");
                    }
                }
            }
        }

        return new UserResponse
        {
            Name = user.Name,
            Phone = ValidationService.IsValidPhone(user.Phone) ? user.Phone : "Номер телефона содержит ошибки",
            Email = ValidationService.IsValidEmail(user.Email) ? user.Email : "Почта содержит ошибки",
            CountFriends = user.Friends?.Count ?? 0,
            FriendsPairs = friendPairs.Any() ? string.Join(", ", friendPairs) : "Дружеские пары отсутствуют"
        };
    }
}