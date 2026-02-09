namespace AI.FifteenGame.WebApi.Models;

/// <summary>
/// Request model for solving a Fifteen Game board position.
/// </summary>
public class SolveRequest
{
    /// <summary>
    /// A dictionary representing the board state where keys are square positions (e.g., "1:1", "1:2") 
    /// and values are the piece numbers (null for the empty square).
    /// </summary>
    public Dictionary<string, int?> Board { get; set; } = new Dictionary<string, int?>();
}
