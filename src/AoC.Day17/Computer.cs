public class Computer
{
    int _instructionPointer = 0;

    (uint A, uint B, uint C) _registers;

    List<uint> _program;
    List<uint> _output = [];

    public Computer((uint A, uint B, uint C) registers, IEnumerable<uint> program)
    {
        _registers = registers;
        _program = [.. program];
    }

    public List<uint> Run()
    {
        while (_instructionPointer < _program.Count)
        {
            OpCode opCode = (OpCode)(_program[_instructionPointer] & 0b111);

            uint operand = (opCode, _program[_instructionPointer + 1]) switch
            {
                (OpCode code, uint value) when !code.IsLiteral() => value,
                (_, uint value) when value <= 3 => value,
                (_, 4) => _registers.A,
                (_, 5) => _registers.B,
                (_, 6) => _registers.C,
                _ => throw new InvalidOperationException("Invalid operand")
            };

            ExecuteInstruction(opCode, operand);

            _instructionPointer += 2;
        }

        return _output;
    }

    public void ExecuteInstruction(OpCode opCode, uint operand)
    {
        switch (opCode)
        {
            case OpCode.ADV:
                _registers.A /= (uint)Math.Pow(2, operand);
                break;
            case OpCode.BXL:
                _registers.B ^= operand;
                break;
            case OpCode.BST:
                _registers.B %= 8;
                break;
            case OpCode.JNZ:
                if (_registers.A != 0) _instructionPointer = (int)operand - 2;
                break;
            case OpCode.BXC:
                _registers.B ^= _registers.C;
                break;
            case OpCode.OUT:
                _output.Add(_registers.B % 8);
                break;
            case OpCode.BDV:
                _registers.B = _registers.A / (uint)Math.Pow(2, operand);
                break;
            case OpCode.CDV:
                _registers.C = _registers.A / (uint)Math.Pow(2, operand);
                break;
            default:
                throw new InvalidOperationException("Invalid opcode");
        }
    }
}
public enum OpCode
{
    ADV = 0, // division => A/(2^COMBO) -> A [Truncated]
    BXL = 1, // bitwise XOR  => B xOR Literal -> B
    BST = 2, // => B % 8 -> B
    JNZ = 3, // jump if A not zero [if jumps it does not add 2]
    BXC = 4, // bitwise AND => B xOR C -> B
    OUT = 5, // => COMBO % 8 -> OUTPUT
    BDV = 6, // division => A/(2^COMBO) -> B [Truncated]
    CDV = 7 // division => A/(2^COMBO) -> C [Truncated]
}


static class OpCodeMethods
{
    public static bool IsCombo(this OpCode opCode) => opCode is OpCode.ADV or OpCode.BST or OpCode.OUT or OpCode.BDV or OpCode.CDV;
    public static bool IsLiteral(this OpCode opCode) => !opCode.IsCombo();
}