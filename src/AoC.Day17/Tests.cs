using Xunit;
using Registers = (uint A, uint B, uint C);

namespace AoC.Day17.Tests;

public class Tests
{
    [Fact(DisplayName = "If register C contains 9, the program 2,6 would set register B to 1.")]
    public void TestRegisterCContains9()
    {
        // Arrange
        var registers = new Registers { C = 9 };
        var program = new uint[] { 2, 6 };

        Computer sut = new(registers, program);

        // Act
        sut.Run();

        // Assert
        Assert.Equal<uint>(1, sut.Registers.B);
    }

    [Fact(DisplayName = "If register A contains 10, the program 5,0,5,1,5,4 would output 0,1,2.")]
    public void TestRegisterAContains10()
    {
        // Arrange
        var registers = new Registers { A = 10 };
        var program = new uint[] { 5, 0, 5, 1, 5, 4 };
        var expectedOutput = new uint[] { 0, 1, 2 };

        Computer sut = new(registers, program);

        // Act
        sut.Run();

        // Assert
        Assert.Equal(expectedOutput, sut.Output);
    }

    [Fact(DisplayName = "If register A contains 2024, the program 0,1,5,4,3,0 would output 4,2,5,6,7,7,7,7,3,1,0 and leave 0 in register A.")]
    public void TestRegisterAContains2024()
    {
        // Arrange
        var registers = new Registers { A = 2024 };
        var program = new uint[] { 0, 1, 5, 4, 3, 0 };
        var expectedOutput = new uint[] { 4, 2, 5, 6, 7, 7, 7, 7, 3, 1, 0 };

        Computer sut = new(registers, program);

        // Act
        sut.Run();

        // Assert
        Assert.Equal(expectedOutput, sut.Output);
        Assert.Equal<uint>(0, sut.Registers.A);
    }

    [Fact(DisplayName = "If register B contains 29, the program 1,7 would set register B to 26.")]
    public void TestRegisterBContains29()
    {
        // Arrange
        var registers = new Registers { B = 29 };
        var program = new uint[] { 1, 7 };

        Computer sut = new(registers, program);

        // Act
        sut.Run();

        // Assert
        Assert.Equal<uint>(26, sut.Registers.B);
    }

    [Fact(DisplayName = "If register B contains 2024 and register C contains 43690, the program 4,0 would set register B to 44354.")]
    public void TestRegisterBContains2024AndCContains43690()
    {
        // Arrange
        var registers = new Registers { B = 2024, C = 43690 };
        var program = new uint[] { 4, 0 };

        Computer sut = new(registers, program);

        // Act
        sut.Run();

        // Assert
        Assert.Equal<uint>(44354, sut.Registers.B);
    }
}