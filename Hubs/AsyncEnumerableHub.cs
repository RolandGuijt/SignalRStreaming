using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRStreaming;

namespace SignalRChat.Hubs
{
    public class AsyncEnumerableHub : Hub
    {
        public async IAsyncEnumerable<int> Counter(
            CountInfo countInfo,
            [EnumeratorCancellation]
            CancellationToken cancellationToken)
        {
            for (var i = 0; i < countInfo.CountUntil; i++)
            {
                // Check the cancellation token regularly so that the server will stop
                // producing items if the client disconnects.
                cancellationToken.ThrowIfCancellationRequested();

                yield return i;

                // Use the cancellationToken in other APIs that accept cancellation
                // tokens so the cancellation can flow down to them.
                await Task.Delay(countInfo.DelayMilliSeconds, cancellationToken);
            }
        }
    }
}
