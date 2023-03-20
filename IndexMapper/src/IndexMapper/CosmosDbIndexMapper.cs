using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using PurpleSpikeProductions.CosmosDbIndexConfigurator.ConfigurationLib;
using PurpleSpikeProductions.CosmosDbIndexConfigurator.IndexMapper.PropertyMappers;

namespace PurpleSpikeProductions.CosmosDbIndexConfigurator.IndexMapper;

public interface ICosmosDbIndexMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly">Assembly to load from</param>
    ImmutableArray<MappedIndexes> MapIndexes(Assembly assembly);
}

public class CosmosDbIndexMapper : ICosmosDbIndexMapper
{
    private readonly IndexPropertyMapper _indexMapper = new IndexPropertyMapper();
    private readonly PartitionKeyPropertyMapper _partitionKeyMapper = new PartitionKeyPropertyMapper();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly">Assembly to load from</param>
    public ImmutableArray<MappedIndexes> MapIndexes(Assembly assembly)
    {
        var typesWithIdsToMap = LoadClassesWithIdsToMap(assembly);
        return LoadMappedIndexesFromDbSetProperties(typesWithIdsToMap);
    }

    private ImmutableArray<MappedIndexes> LoadMappedIndexesFromDbSetProperties(ImmutableArray<MappedType> typesToMap)
    {
        var builder = ImmutableArray.CreateBuilder<MappedIndexes>(typesToMap.Length);

        foreach (var typeToMap in typesToMap)
        {
            var partitionKey = _partitionKeyMapper.MapPropertyWithAttribute(typeToMap.Type, indexPath: "/");
            var indexes = _indexMapper.MapPropertiesWithAttribute(typeToMap.Type, indexPath: "/");

            var mappedIndexes = new MappedIndexes(ContainerName: typeToMap.ContainerName, partitionKey, indexes);
            builder.Add(mappedIndexes);
        }

        return builder.MoveToImmutable();
    }

    private ImmutableArray<MappedType> LoadClassesWithIdsToMap(Assembly assembly)
    {
        var builder = ImmutableArray.CreateBuilder<MappedType>();
        foreach (var type in assembly.DefinedTypes)
        {
            try
            {
                var attr = type.GetCustomAttribute<IdConfiguredEntityAttribute>();
                if (attr is object)
                {
                    builder.Add(new MappedType(type, attr.ContainerName));
                }

            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error reflecting the {type.AssemblyQualifiedName} assembly. {ex.Message}");
            }
        }

        return builder.ToImmutableArray();
    }

    private record MappedType(TypeInfo Type, string ContainerName);
}
