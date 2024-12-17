var input = await File.ReadAllLinesAsync(Path.Join(Directory.GetCurrentDirectory(), "input.txt"));

var registerA = int.Parse(input[0].Split(' ')[2]);
var registerB = int.Parse(input[1].Split(' ')[2]);
var registerC = int.Parse(input[2].Split(' ')[2]);
var registersGlobal = new long[] {registerA, registerB, registerC}; // 0 - A, 1 - B, 2 - C
var instructions = input[4].Split(' ')[1].Split(',').Select(byte.Parse).ToArray();

GetOutput(registersGlobal, instructions, out var outputs);

Console.WriteLine($"First part: {string.Join(",", outputs.Take(outputs.Count - 1))}"); // For some reason it prints one character too much


return;

void GetOutput(long[] registers, byte[] instructions, out List<long> outputs, List<long> expectedOutputs = null)
{
    outputs = [];
    var i = 1;
    while (i < instructions.Length)
    {
        if (expectedOutputs != null && outputs.Count > 0 && expectedOutputs[0] != outputs[0])
        {
            break;
        }
        if (!DoInstruction(instructions[i - 1], instructions[i], outputs, registers))
        {
            i += 2;
        }
    }
    return;
    
    bool DoInstruction(byte instruction, byte operand, List<long> outputs, long[] registers)
    {
        switch (instruction)
        {
            case 0:
            {
                registers[0] = (long)(registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
                break;
            }
            case 1:
            {
                registers[1] ^= operand;
                break;
            }
            case 2:
            {
                registers[1] = GetComboOperand(operand, registers) % 8;
                break;
            }
            case 3:
            {
                if (registers[0] != 0)
                {
                    i = operand + 1;
                    var j = i;
                    while (j < instructions.Length)
                    {
                        if (!DoInstruction(instructions[j - 1], instructions[j], outputs, registers))
                        {
                            j += 2;
                        }
                    }
                    return true;
                }

                break;
            }
            case 4:
            {
                registers[1] ^= registers[2];
                break;
            }
            case 5:
            {
                outputs.Add(GetComboOperand(operand, registers) % 8);
                break;
            }
            case 6:
            {
                registers[1] = (long)(registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
                break;
            }
            case 7:
            {
                registers[2] = (long)(registers[0] / Math.Pow(2, GetComboOperand(operand, registers)));
                break;
            }
        }
        return false;
    }

    long GetComboOperand(byte operand, long[] registers)
    {
        switch (operand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            {
                return operand;
            }
            case 4:
                return registers[0];
            case 5:
                return registers[1];
            case 6:
                return registers[2];
            default:
                throw new InvalidOperationException("Invalid program");
        }
    }
}
