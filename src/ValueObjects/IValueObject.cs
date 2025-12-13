namespace ValueObjects;

public interface IValueObject<TSelf, TValue>
{
    static abstract TSelf Create(TValue value);
    static abstract TValue ToValue(TSelf value);
}