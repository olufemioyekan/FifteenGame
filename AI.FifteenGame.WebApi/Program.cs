using AI.FifteenGame.Agent;
using AI.FifteenGame;
using AI.FifteenGame.WebApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

app.MapGet("/new", () =>
{
    var board = BoardStateGenerator.CreateRandomBoard();
    return Results.Ok(board);

});

app.MapPost("/solve", (SolveRequest request) =>
{
    try
    {
        // Convert the request DTO to BoardState
        var squareMap = new Dictionary<BoardSquare, int?>();
        foreach (var kvp in request.Board)
        {
            try
            {
                var square = BoardSquareFactory.Parse(kvp.Key);
                squareMap[square] = kvp.Value;
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = $"Invalid board format: {ex.Message}" });
            }
        }
        
        // Validate the board has exactly 16 squares
        if (squareMap.Count != 16)
        {
            return Results.BadRequest(new { error = $"Invalid board. Expected 16 squares, but got {squareMap.Count}" });
        }
        
        var boardState = new BoardState(squareMap);
        var agent = new GameAgent(boardState);
        var solution = agent.SolveGame();
        
        if (solution == null)
        {
            return Results.Problem("The solver was unable to find a solution for the given board state. The board may be unsolvable.");
        }
        
        // Convert the solution to response DTO
        var moves = solution
            .Where(node => node.Move != null)
            .Select(node => new MoveDto
            {
                Piece = node.Move!.Piece,
                Direction = node.Move!.Direction.ToString()
            })
            .ToList();
        
        var response = new SolveResponse
        {
            Moves = moves,
            TotalMoves = moves.Count
        };
        
        return Results.Ok(response);
    }
    catch (InvalidOperationException ex)
    {
        // Handle known operational errors with specific messages
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception)
    {
        // Log the exception here if logging is configured
        return Results.Problem(title: "An error occurred while solving the puzzle", detail: "Please check that the board state is valid and try again.");
    }

});

app.MapGet("/", () =>
{
    string welcome = "Welcome to the Fifteen Game API!  Use the /new endpoint to generate a random starting position, and the /solve endpoint to solve a given position.";

     return Results.Ok(welcome);
});

app.Run();



