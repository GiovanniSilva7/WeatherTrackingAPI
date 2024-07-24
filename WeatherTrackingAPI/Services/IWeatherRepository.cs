using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IWeatherRepository
{
    Task<WeatherCondition> GetLatestConditionAsync(string location);
    Task SaveConditionAsync(WeatherCondition condition);
}

public class WeatherRepository : IWeatherRepository
{
    private readonly List<WeatherCondition> _conditions = new List<WeatherCondition>();

    public async Task<WeatherCondition> GetLatestConditionAsync(string location)
    {
        return _conditions.Where(c => c.Location == location).OrderByDescending(c => c.Timestamp).FirstOrDefault();
    }

    public async Task SaveConditionAsync(WeatherCondition condition)
    {
        _conditions.Add(condition);
    }
}
