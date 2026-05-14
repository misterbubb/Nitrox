using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Nitrox.Model.Constants;
using Nitrox.Model.Logger;

namespace Nitrox.Launcher.Models.Services;

/// <summary>
///     Writes the gRPC connection info in a file that Nitrox servers can access. On Windows, writes the named pipe name. On Linux, writes the port number.
/// </summary>
internal class WriteGrpcPortFileService(IServer server) : IHostedLifecycleService
{
    private readonly string filePath = Path.Combine(Path.GetTempPath(), LauncherConstants.GRPC_LISTEN_PORT_TEMP_FILE_NAME);
    private readonly IServer server = server;

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StartingAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        string connectionInfo;
        
        if (OperatingSystem.IsWindows())
        {
            // On Windows, write the named pipe name
            connectionInfo = LauncherConstants.GRPC_NAMED_PIPE_NAME;
        }
        else
        {
            // On Linux, write the port number
            IServerAddressesFeature? addressFeature = server.Features.Get<IServerAddressesFeature>();
            int grpcPort = addressFeature.Addresses.Select(a => new Uri(a).Port).First();
            connectionInfo = grpcPort.ToString();
        }
        
        int attempts = 10;
        while (attempts-- > 0)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, connectionInfo, cancellationToken);
                break;
            }
            catch (Exception ex)
            {
                Log.Warn($"Failed to write gRPC connection info (attempt {10 - attempts}/10): {ex.Message}");
                await Task.Delay(500, cancellationToken);
            }
        }
    }

    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        try
        {
            File.Delete(filePath);
        }
        catch (Exception)
        {
            // ignored
        }
        return Task.CompletedTask;
    }
}
