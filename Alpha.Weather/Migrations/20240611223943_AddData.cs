using Alpha.Common.WeatherService;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alpha.Weather.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 100).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();

            foreach (var forecast in forecasts)
            {
                migrationBuilder.InsertData( "WeatherForecast",
                    ["Date", "TemperatureC", "Summary"],
                    [ forecast.Date, forecast.TemperatureC, forecast.Summary] );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM WeatherForecast");
        }
    }
}
