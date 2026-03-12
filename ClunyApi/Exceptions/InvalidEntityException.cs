using System;

namespace ClunyApi.Exceptions
{
    public sealed class InvalidEntityException : Exception
    {
        public string EntityName { get; }

        public InvalidEntityException(string entityName, string? message = null)
            : base(message ?? $"The entity '{entityName}' is invalid for this operation.")
        {
            EntityName = entityName;
        }
    }
}
