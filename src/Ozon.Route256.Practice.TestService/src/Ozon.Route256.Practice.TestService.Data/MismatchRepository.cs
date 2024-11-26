using System.Collections.Concurrent;

namespace Ozon.Route256.TestService.Data;

public class MismatchRepository : IMismatchRepository
{
    private readonly ConcurrentBag<Mismatch> _concurrentBag = new();

    public void AddMismatch(Mismatch mismatch) => _concurrentBag.Add(mismatch);

    public IReadOnlyCollection<Mismatch> ListAll() => _concurrentBag.ToArray();

    public void Clear() => _concurrentBag.Clear();
}
