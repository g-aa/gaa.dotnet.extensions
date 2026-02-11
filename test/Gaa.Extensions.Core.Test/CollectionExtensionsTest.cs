namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тесто для <see cref="CollectionExtensions"/>.
/// </summary>
/// <typeparam name="T">Тип значения.</typeparam>
[TestFixture(typeof(int), 100)]
[TestFixture(typeof(string), "QWERTY")]
internal sealed class CollectionExtensionsTest<T>
    where T : notnull
{
    private readonly T _value;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="CollectionExtensionsTest{T}"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    public CollectionExtensionsTest(T value)
    {
        _value = value;
    }

    /// <summary>
    /// Успешное выполнение <see cref="CollectionExtensions.ToListWithValue{T}(T)"/>.
    /// </summary>
    [Test]
    public void SuccessfulToListWithValue()
    {
        // acr
        var list = _value.ToListWithValue();

        // assert
        list.Should().BeOfType<List<T>>();
        list.Should().ContainSingle();
        list.Should().Contain(_value);
    }

    /// <summary>
    /// Успешное выполнение <see cref="CollectionExtensions.ToArrayWithValue{T}(T)"/>.
    /// </summary>
    [Test]
    public void SuccessfulToArrayWithValue()
    {
        // acr
        var list = _value.ToArrayWithValue();

        // assert
        list.Should().BeOfType<T[]>();
        list.Should().ContainSingle();
        list.Should().Contain(_value);
    }
}