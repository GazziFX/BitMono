﻿namespace BitMono.Core.Renaming;

[SuppressMessage("ReSharper", "InvertIf")]
public class Renamer
{
    private readonly NameCriticalAnalyzer _nameCriticalAnalyzer;
    private readonly SpecificNamespaceCriticalAnalyzer _specificNamespaceCriticalAnalyzer;
    private readonly ObfuscationSettings _obfuscationSettings;
    private readonly RandomNext _randomNext;

    public Renamer(
        NameCriticalAnalyzer nameCriticalAnalyzer,
        SpecificNamespaceCriticalAnalyzer specificNamespaceCriticalAnalyzer,
        IOptions<ObfuscationSettings> configuration,
        RandomNext randomNext)
    {
        _nameCriticalAnalyzer = nameCriticalAnalyzer;
        _specificNamespaceCriticalAnalyzer = specificNamespaceCriticalAnalyzer;
        _obfuscationSettings = configuration.Value;
        _randomNext = randomNext;
    }

    public string RenameUnsafely()
    {
        var strings = _obfuscationSettings.RandomStrings!;
        var randomStringOne = strings[_randomNext(0, strings.Length - 1)] + " " + strings[_randomNext(0, strings.Length - 1)];
        var randomStringTwo = strings[_randomNext(0, strings.Length - 1)];
        var randomStringThree = strings[_randomNext(0, strings.Length - 1)];
        return $"{randomStringTwo} {randomStringOne}.{randomStringThree}";
    }
    public void Rename(IMetadataMember member)
    {
        if (member is TypeDefinition type)
        {
            if (_nameCriticalAnalyzer.NotCriticalToMakeChanges(type))
            {
                type.Name = RenameUnsafely();
            }
        }
        if (member is MethodDefinition method)
        {
            if (_nameCriticalAnalyzer.NotCriticalToMakeChanges(method))
            {
                method.Name = RenameUnsafely();
            }
        }
        if (member is FieldDefinition field)
        {
            field.Name = RenameUnsafely();
        }
        if (member is ParameterDefinition parameter)
        {
            parameter.Name = RenameUnsafely();
        }
    }
    public void Rename(params IMetadataMember[] members)
    {
        for (var i = 0; i < members.Length; i++)
        {
            Rename(members[i]);
        }
    }
    public void RemoveNamespace(IMetadataMember member)
    {
        if (member is TypeDefinition type)
        {
            if (_specificNamespaceCriticalAnalyzer.NotCriticalToMakeChanges(type))
            {
                type.Namespace = string.Empty;
            }
        }
        if (member is MethodDefinition method)
        {
            if (_nameCriticalAnalyzer.NotCriticalToMakeChanges(method))
            {
                if (method.DeclaringType != null)
                {
                    method.DeclaringType.Namespace = string.Empty;
                }
            }
        }
        if (member is FieldDefinition field)
        {
            if (field.DeclaringType != null)
            {
                field.DeclaringType.Namespace = string.Empty;
            }
        }
    }
    public void RemoveNamespace(params IMetadataMember[] members)
    {
        members.ForEach(RemoveNamespace);
    }
}