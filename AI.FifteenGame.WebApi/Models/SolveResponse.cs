namespace AI.FifteenGame.WebApi.Models;

/// <summary>
/// Response model containing the solution to a Fifteen Game puzzle.
/// </summary>
public class SolveResponse
{
    /// <summary>
    /// The sequence of moves to solve the puzzle, from start to finish.
    /// </summary>
    public List<MoveDto> Moves { get; set; } = new List<MoveDto>();
    
    /// <summary>
    /// The total number of moves in the solution.
    /// </summary>
    public int TotalMoves { get; set; }
}
