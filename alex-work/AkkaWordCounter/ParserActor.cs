using Akka.Actor;
using Akka.Event;
using static AkkaWordCounter.CounterCommands; // make message handlers less verbose
using static AkkaWordCounter.DocumentCommands;

namespace AkkaWordCounter;

public sealed class ParserActor : UntypedActor
{
    private readonly ILoggingAdapter _log = Context.GetLogger();
    private readonly IActorRef _countingActor;

    public ParserActor(IActorRef countingActor)
    {
        _countingActor = countingActor;
    }

    private const int TokenBatchSize = 10;

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case ProcessDocument process:
                {
                    // chunk tokens into buckets of 10
                    foreach (var tokenBatch in process.RawText.Split(" ").Chunk(TokenBatchSize))
                    {
                        _countingActor.Tell(new CountTokens(tokenBatch));
                    }

                    // we are finished 🥀
                    _countingActor.Tell(new ExpectNoMoreTokens());
                    break;
                }
            default:
                Unhandled(message);
                break;
        }
    }
}