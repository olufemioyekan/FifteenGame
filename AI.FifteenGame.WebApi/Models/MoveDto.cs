namespace AI.FifteenGame.WebApi.Models;

/// <summary>
/// Represents a single move in the solution.
/// </summary>
public class MoveDto
{
    /// <summary>
    /// The piece number that is moved.
    /// </summary>
    public int Piece { get; set; }
    
    /// <summary>
    /// The direction the piece moves.
    /// </summary>
    public string Direction { get; set; } = string.Empty;
}
