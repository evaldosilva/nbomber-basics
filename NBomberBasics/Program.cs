using NBomber.CSharp;

Console.WriteLine("NBomber Basics");

var scenario = Scenario.Create("hello_world_scenario", async context =>
{
    // you can define and execute any logic here,
    // for example: send http request, SQL query etc
    // NBomber will measure how much time it takes to execute your logic
    await Task.Delay(1_000);

    return Response.Ok();
})
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            );

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();