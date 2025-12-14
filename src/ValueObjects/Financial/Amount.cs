using System.Numerics;

namespace ValueObjects.Financial;

public partial record struct Amount :
    IValueObject<Amount, decimal>,
    IIncrementOperators<Amount>,
    IDecrementOperators<Amount>,
    IUnaryPlusOperators<Amount, Amount>,
    IUnaryNegationOperators<Amount, Amount>,
    IAdditionOperators<Amount, Amount, Amount>,
    ISubtractionOperators<Amount, Amount, Amount>,
    IMultiplyOperators<Amount, decimal, Amount>,
    IMultiplyOperators<Amount, int, Amount>,
    IMultiplyOperators<Amount, double, Amount>,
    IDivisionOperators<Amount, decimal, Amount>,
    IDivisionOperators<Amount, double, Amount>,
    IDivisionOperators<Amount, int, Amount>,
    IMultiplyOperators<Amount, Percentage, Amount>,
    IDivisionOperators<Amount, Percentage, Amount>
//IAdditionOperators<Amount, Currency, Money>,

{
    /// <inheritdoc />
    public static Amount operator +(Amount value)
        => Create(+value._value);

    /// <inheritdoc />
    public static Amount operator -(Amount value)
       => Create(-value._value);

    /// <inheritdoc />
    public static Amount operator +(Amount left, Amount right)
        => Create(left._value + right._value);

    /// <inheritdoc />
    public static Amount operator -(Amount left, Amount right)
        => Create(left._value - right._value);

    /// <inheritdoc />
    public static Amount operator ++(Amount value)
        => Create(value._value + 1);

    /// <inheritdoc />
    public static Amount operator --(Amount value)
        => Create(value._value - 1);

    /// <inheritdoc />
    public static Amount operator *(Amount left, decimal right)
        => Create(left._value * right);

    /// <inheritdoc />
    public static Amount operator /(Amount left, decimal right)
        => Create(left._value / right);

    /// <inheritdoc />
    public static bool operator >(Amount left, Amount right)
        => left._value > right._value;

    /// <inheritdoc />
    public static bool operator >=(Amount left, Amount right)
        => right._value >= left._value;

    /// <inheritdoc />
    public static bool operator <(Amount left, Amount right)
        => left._value < right._value;

    /// <inheritdoc />
    public static bool operator <=(Amount left, Amount right)
        => left._value <= right._value;

    /// <inheritdoc />
    public static Amount operator %(Amount left, decimal right)
        => Create(left._value % right);

    /// <inheritdoc />
    public static Amount operator *(Amount left, double right)
        => Create(left._value * (decimal)right);

    /// <inheritdoc />
    public static Amount operator /(Amount left, double right)
        => Create(left._value / (decimal)right);

    /// <inheritdoc />
    public static Amount operator *(Amount left, int right)
        => Create(left._value * right);

    /// <inheritdoc />
    public static Amount operator /(Amount left, int right)
        => Create(left._value / right);


    /// <inheritdoc />
    public static Amount operator *(Amount left, Percentage right)
        => new(left._value * (decimal)right);

    /// <inheritdoc />
    public static Amount operator /(Amount left, Percentage right)
        => Create(left._value / (decimal)right);

    public static bool IsValid(decimal value) => true;

    ///// <inheritdoc />
    //public static Money operator +(Amount left, Currency right)
    //    => Create(right, left);
}