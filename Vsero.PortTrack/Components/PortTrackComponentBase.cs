using Microsoft.AspNetCore.Components;

namespace Vsero.PortTrack.Components;

public abstract class PortTrackComponentBase : ComponentBase, IDisposable
{
    
    protected CancellationTokenSource? _cts;


    protected CancellationToken CancellationToken(int seconds = 10)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
        return _cts.Token;
    }



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }



    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }

}
