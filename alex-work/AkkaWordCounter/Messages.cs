using Akka.Actor;

namespace AkkaWordCounter;

public static class DocumentCommands
{
    public sealed record ProcessDocument(string RawText);
}

// Counter Inputs
public static class CounterCommands
{
    public sealed record CountTokens(IReadOnlyList<string> Tokens);

    // parser reached the end of the document
    public sealed record ExpectNoMoreTokens();
}

// Counter Queries
public static class CounterQueries
{
    // Send this acter a notification once counting is complete
    public sealed record FetchCounts(IActorRef Subscriber);
}