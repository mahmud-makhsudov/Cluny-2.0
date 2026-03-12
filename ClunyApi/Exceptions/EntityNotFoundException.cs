using System;

namespace ClunyApi.Exceptions
{
    public sealed class EntityNotFoundException : Exception
    {
        public string EntityName { get; }
        public object? Key { get; }

        public EntityNotFoundException(string entityName, object? key = null)
            : base(key == null
                   ? $"Entity '{entityName}' was not found."
                   : $"Entity '{entityName}' with key '{key}' was not found.")
        {
            EntityName = entityName;
            Key = key;
        }
    }
}
