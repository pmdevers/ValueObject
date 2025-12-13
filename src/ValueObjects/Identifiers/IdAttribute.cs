namespace ValueObjects.Identifiers;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class IdAttribute<TBehaviour, TBaseType> : Attribute
{
}
