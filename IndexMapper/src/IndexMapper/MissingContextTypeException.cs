﻿using System;
using System.Runtime.Serialization;

namespace PurpleSpikeProductions.CosmosDbIndexConfigurator.IndexMapper;
[Serializable]
internal class MissingContextTypeException : Exception
{
    public MissingContextTypeException()
    {
    }

    public MissingContextTypeException(string? message) : base(message)
    {
    }

    public MissingContextTypeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
