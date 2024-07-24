using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class WeatherJob
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly PushNotificationService _pushNotificationService;

    public WeatherJob(IWeatherRepository weatherRepository, PushNotificationService pushNotificationService)
    {
        _weatherRepository = weatherRepository;
        _pushNotificationService = pushNotificationService;
    }

    public async Task SimulateWeatherChangeAsync()
    {
        // Simular a mudança climática
        var random = new Random();
        var locations = new[] { "Location1", "Location2", "Location3" }; // Exemplo de locais
        var conditions = new[] { "Sunny", "Rainy", "Stormy", "Snowy" }; // Exemplo de condições

        foreach (var location in locations)
        {
            var condition = new WeatherCondition
            {
                Location = location,
                Condition = conditions[random.Next(conditions.Length)],
                Timestamp = DateTime.UtcNow,
                IsAlert = random.NextDouble() > 0.8 // 20% de chance de ser um alerta
            };

            await _weatherRepository.SaveConditionAsync(condition);

            // Enviar notificação se for um alerta
            if (condition.IsAlert)
            {
                var users = GetUsersForLocation(location); // Método para obter os usuários do local
                foreach (var user in users)
                {
                    await _pushNotificationService.SendNotificationAsync(user.Token, "Weather Alert", $"Alert: {condition.Condition} in {location}");
                    // Implementar lógica para reenviar notificações se necessário
                }
            }
        }
    }

    private IEnumerable<User> GetUsersForLocation(string location)
    {
        // Lógica para obter os usuários do local
        // Isso pode envolver consultas ao banco de dados ou outro repositório de usuários
        return new List<User>
        {
            new User { Token = "user_token_1" },
            new User { Token = "user_token_2" }
        };
    }
}
