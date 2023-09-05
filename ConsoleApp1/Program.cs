using CsvHelper;
using HtmlAgilityPack;
using System.Globalization;

// URL de la página web que deseas analizar
string url = "https://forecast.weather.gov/MapClick.php?lat=41.8843&lon=-87.6324#.YbH0ptBBxPY";

// Realizar una solicitud HTTP
var doc = new HtmlWeb().Load(url);

// Buscar el contenedor de pronóstico semanal
var weeklyForecast = doc.DocumentNode.SelectSingleNode("//div[@id='seven-day-forecast-container']");

// Extraer los períodos del pronóstico
var forecastPeriodList = weeklyForecast.SelectNodes(".//p[contains(@class, 'period-name')]").Select(x => x.InnerText.Trim()).ToArray(); ;

// Extraer las temperaturas del pronóstico
var forecastTemperatureList = weeklyForecast.SelectNodes(".//p[contains(@class, 'temp')]").Select(x=> x.InnerText).ToArray();

// Extraer las descripciones del pronóstico
var forecastDescriptionList = weeklyForecast.SelectNodes(".//p[@class='short-desc']").Select(x => x.InnerText.Trim()).ToArray();

// Crear una lista con los datos
var weatherDataList = forecastPeriodList.Select((item, i) => (item, i)).Select(x=> new
{
    ForecastPeriod = forecastPeriodList[x.i],
    ForecastTemperature = forecastTemperatureList[x.i],
    ForecastDescription = forecastDescriptionList[x.i]
}).ToArray();

// Escribir los datos en un archivo CSV
using (var writer = new StreamWriter("weather_data.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
csv.WriteRecords(weatherDataList);