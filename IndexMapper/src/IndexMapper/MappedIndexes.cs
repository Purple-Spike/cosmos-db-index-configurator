using System.Collections.Immutable;

namespace PurpleSpikeProductions.CosmosDbIndexConfigurator.IndexMapper;

public record MappedIndexes(string ContainerName, string? PartitionKey, ImmutableArray<string> IncludedIndexes);
