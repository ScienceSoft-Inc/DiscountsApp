using System.Collections.Concurrent;
using System.Threading;

namespace ScnDiscounts.Helpers
{
    public class NamedLocker
    {
        protected readonly ConcurrentDictionary<string, SemaphoreSlim> Lock = new ConcurrentDictionary<string, SemaphoreSlim>();

        public SemaphoreSlim this[string name] => Lock.GetOrAdd(name, i => new SemaphoreSlim(1, 1));
    }
}
