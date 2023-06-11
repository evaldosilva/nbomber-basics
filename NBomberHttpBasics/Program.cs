using NBomber.CSharp;

Console.WriteLine("NBomber HTTP Basics");

using var httpClient = new HttpClient();

var scenario = Scenario.Create("hello_world_scenario", async context =>
{
    var response = await httpClient.GetAsync("https://nbomber.com");

    return response.IsSuccessStatusCode
        ? Response.Ok()
        : Response.Fail();
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