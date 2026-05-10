using System.Threading.Channels;

namespace Nitrox.Server.Subnautica.Services;

/// <summary>
///     Tracks server loading progress and provides updates to connected management clients (e.g., Nitrox Launcher).
/// </summary>
internal sealed class ServerLoadingProgressService
{
    public static readonly Channel<LoadingProgressUpdate> ProgressQueue = Channel.CreateBounded<LoadingProgressUpdate>(new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.DropOldest });

    /// <summary>
    ///     Reports loading progress to any connected management clients.
    /// </summary>
    /// <param name="stage">Description of the current loading stage (e.g., "Loading entities", "Initializing world")</param>
    /// <param name="progress">Progress value between 0.0 and 1.0</param>
    public static void ReportProgress(string stage, float progress)
    {
        ProgressQueue.Writer.TryWrite(new LoadingProgressUpdate(stage, Math.Clamp(progress, 0f, 1f)));
    }

    internal record LoadingProgressUpdate(string Stage, float Progress);
}
