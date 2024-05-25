using System;
using System.Runtime.Serialization;

namespace PurpleSpikeProductions.CosmosDbIndexConfigurator.IndexMapper;
[Serializable]
internal class InvalidDbContextTypeException : Exception
{
    public InvalidDbContextTypeException()
    {
    }

    public InvalidDbContextTypeException(string? message) : base(message)
    {
    }

    public InvalidDbContextTypeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
