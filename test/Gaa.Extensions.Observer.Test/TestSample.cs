namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="string"/>.
/// </summary>
[TestFixture]
internal sealed class TestSample
{
    private string _example;

    /// <summary>
    /// Настройка класса.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _example = "test message";
    }

    /// <summary>
    /// Положительный тест для <see cref="string.Equals(string?, string?, System.StringComparison)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    [TestCase("test message")]
    [TestCase("TeSt MeSsAgE")]
    public void MessagePositiveTest(string message)
    {
        // arrange
        var comparisonType = System.StringComparison.OrdinalIgnoreCase;

        // act
        var result = string.Equals(_example, message, comparisonType);

        // assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Отрицательный тест для <see cref="string.Equals(string?, string?, System.StringComparison)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    [TestCase(" test message ")]
    [TestCase("QWERTY")]
    public void MessageNegativeTest(string message)
    {
        // arrange
        var comparisonType = System.StringComparison.OrdinalIgnoreCase;

        // act
        var result = string.Equals(_example, message, comparisonType);

        // assert
        result.Should().BeFalse();
    }
}