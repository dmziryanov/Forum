using System;

namespace RmsAuto.Common.Caching
{
    interface IKeyedEntities<TKey, TEntity>
        where TEntity : class
    {
        bool ContainsKey(TKey key);
        TEntity this[TKey key] { get; }
    }
}
