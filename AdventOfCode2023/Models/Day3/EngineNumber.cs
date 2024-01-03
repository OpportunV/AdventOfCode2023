namespace AdventOfCode2023.Models.Day3;

public record EngineNumber(int Value, int Row, int Index, int Length)
{
    public int EndIndex => Index + Length - 1;
    
    public bool IsPartNumber { get; set; }
}