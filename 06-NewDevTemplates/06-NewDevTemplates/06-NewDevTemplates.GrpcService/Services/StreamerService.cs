using System.Collections.Concurrent;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace _06_NewDevTemplates.GrpcService.Services
{
    public class StreamerService : Streamer.StreamerBase
    {
        private readonly ILogger<StreamerService> _logger;

        private static readonly ConcurrentDictionary<string, IServerStreamWriter<StreamResponse>> _subscribers = new ConcurrentDictionary<string, IServerStreamWriter<StreamResponse>>();

        public StreamerService(ILogger<StreamerService> logger)
        {
            _logger = logger;
        }

        public override async Task Do(
            IAsyncStreamReader<StreamRequest> requestStream,
            IServerStreamWriter<StreamResponse> responseStream,
            ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
                return;

            _subscribers.TryAdd(requestStream.Current.ClientId, responseStream);

            do
            {
                if (requestStream.Current == null)
                    continue;

                _logger.LogInformation($"X: {requestStream.Current.X}, Y: {requestStream.Current.Y}, ClientId: {requestStream.Current.ClientId}");

                var response = new StreamResponse
                {
                    X = requestStream.Current.X,
                    Y = requestStream.Current.Y
                };

                foreach (var subscriber in _subscribers)
                {
                    await subscriber.Value.WriteAsync(response);
                }

            } while (await requestStream.MoveNext());
        }
    }
}