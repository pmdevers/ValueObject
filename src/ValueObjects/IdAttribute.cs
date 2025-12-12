namespace ValueObjects;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class IdAttribute<TBehaviour, TBaseType> : Attribute
{
}
