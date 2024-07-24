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
        // Simular a mudan�a clim�tica
        var random = new Random();
        var locations = new[] { "Location1", "Location2", "Location3" }; // Exemplo de locais
        var conditions = new[] { "Sunny", "Rainy", "Stormy", "Snowy" }; // Exemplo de condi��es

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

            // Enviar notifica��o se for um alerta
            if (condition.IsAlert)
            {
                var users = GetUsersForLocation(location); // M�todo para obter os usu�rios do local
                foreach (var user in users)
                {
                    await _pushNotificationService.SendNotificationAsync(user.Token, "Weather Alert", $"Alert: {condition.Condition} in {location}");
                    // Implementar l�gica para reenviar notifica��es se necess�rio
                }
            }
        }
    }

    private IEnumerable<User> GetUsersForLocation(string location)
    {
        // L�gica para obter os usu�rios do local
        // Isso pode envolver consultas ao banco de dados ou outro reposit�rio de usu�rios
        return new List<User>
        {
            new User { Token = "user_token_1" },
            new User { Token = "user_token_2" }
        };
    }
}
