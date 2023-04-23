using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Grains;

try
{
    using IHost host = await StartClientAsync();
    var client = host.Services.GetRequiredService<IClusterClient>();

    await DoClientWorkAsync(client);
    Console.ReadKey();

    await host.StopAsync();

    return 0;
}
catch (Exception e)
{
    Console.WriteLine($$"""
        Exception while trying to run client: {{e.Message}}
        Make sure the silo the client is trying to connect to is running.
        Press any key to exit.
        """);

    Console.ReadKey();
    return 1;
}

static async Task<IHost> StartClientAsync()
{
    var builder = new HostBuilder()
        .UseOrleansClient(client =>
        {
            client.UseLocalhostClustering();
            client.AddOutgoingGrainCallFilter<AsyncLocalOutgoingFilter>();
        })
        .ConfigureLogging(logging => logging.AddConsole());

    var host = builder.Build();
    await host.StartAsync();

    Console.WriteLine("Client successfully connected to silo host \n");

    return host;
}

static async Task DoClientWorkAsync(IClusterClient client)
{
    var grain = client.GetGrain<IAsyncLocalTestGrain>(0);

    var firstTask = new Task(async () =>
    {
        UnittestAsyncLocalContext.SetUnittestContext("Unittest_TestAsync1");
        await grain.PrintAsyncLocal1();
    });

    var secondTask = new Task(async () =>
    {
        UnittestAsyncLocalContext.SetUnittestContext("Unittest_TestAsync2");
        await grain.PrintAsyncLocal1();
    });

    firstTask.Start();
    secondTask.Start();

    await Task.WhenAll(
        firstTask,
        secondTask
        );

    await Task.Delay(10);

    await grain.PrintAsyncLocal1();
}