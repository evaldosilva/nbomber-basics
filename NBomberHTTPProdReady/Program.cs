using NBomber.CSharp;
using NBomber.Http.CSharp;

Console.WriteLine("NBomber HTTP Prod Ready");

using var httpClient = new HttpClient();

var scenario = Scenario.Create("http_scenario", async context =>
{
    var request =
        Http.CreateRequest("GET", "https://nbomber.com")
            .WithHeader("Accept", "text/html")
            .WithBody(new StringContent("{ some JSON }"));

    var response = await Http.Send(httpClient, request);

    return response;
})
.WithoutWarmUp()
.WithLoadSimulations(
// In this example, we configure 
// (RampingInject) - ramp up from 0 to 200 requests per second for 1 minute,
// (Inject) - then we keep the rate of 200 for the next 30 sec.*

    Simulation.RampingInject(rate: 200,
                             interval: TimeSpan.FromSeconds(1),
                             during: TimeSpan.FromMinutes(1)),

    Simulation.Inject(rate: 200,
                      interval: TimeSpan.FromSeconds(1),
                      during: TimeSpan.FromSeconds(30))
);

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();