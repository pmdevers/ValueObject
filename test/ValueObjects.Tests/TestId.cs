using ValueObjects.Identifiers;

namespace ValueObject.Tests;

[Id<GuidIdBehaviour, Guid>]
public partial record struct TestId { }