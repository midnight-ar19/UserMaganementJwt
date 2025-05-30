using UserMaganementJwt.Interfaces;
using UserMaganementJwt.Models;
using UserMaganementJwt.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Configuración
        var users = new List<User>();
        var userRepository = new InMemoryUserRepository(users);
        var jwtSettings = new JwtSettings
        {
            SecretKey = "supersecretkey12345678901234567890",
            Issuer = "myapp",
            Audience = "myapp",
            ExpiryMinutes = 60
        };
        var jwtService = new JwtService(jwtSettings);
        var userService = new UserService(userRepository, jwtService);

        try
        {
            // Registrar usuario
            Console.WriteLine("Registrando usuario...");
            var user = await userService.RegisterAsync("testuser", "test@example.com", "password123");
            Console.WriteLine($"Usuario registrado: {user.Username}, {user.Email}");

            // Login
            Console.WriteLine("Iniciando sesión...");
            var token = await userService.LoginAsync("test@example.com", "password123");
            Console.WriteLine($"Token: {token}");

            // Validar token
            Console.WriteLine("Validando token...");
            var isValid = await userService.ValidateTokenAsync(token);
            Console.WriteLine($"Token válido: {isValid}");

            // Obtener usuario
            Console.WriteLine("Obteniendo usuario...");
            var fetchedUser = await userService.GetUserByIdAsync(user.Id);
            Console.WriteLine($"Usuario: {fetchedUser.Username}, {fetchedUser.Email}");

            // Probar error: login incorrecto
            Console.WriteLine("Probando login incorrecto...");
            try
            {
                await userService.LoginAsync("test@example.com", "wrongpassword");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error esperado: {ex.Message}");
            }

            // Probar error: email duplicado
            Console.WriteLine("Probando registro con email duplicado...");
            try
            {
                await userService.RegisterAsync("anotheruser", "test@example.com", "password123");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error esperado: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users;

    public InMemoryUserRepository(List<User> users)
    {
        _users = users;
    }

    public Task<User> GetByIdAsync(string id)
    {
        return Task.FromResult(_users.Find(u => u.Id == id));
    }

    public Task<User> GetByUsernameAsync(string username)
    {
        return Task.FromResult(_users.Find(u => u.Username == username));
    }

    public Task<User> GetByEmailAsync(string email)
    {
        return Task.FromResult(_users.Find(u => u.Email == email));
    }

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}