using AI.FifteenGame.Agent;
using AI.FifteenGame;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

app.MapGet("/new",  () =>
{
    var board = BoardStateGenerator.CreateRandomBoard();
    return Results.Ok(board);

});

app.MapPost("/solve",  (BoardState board) =>
{
    var agent = new GameAgent(board);
    var solution = agent.SolveGame();
    return Results.Ok(solution);

});

app.MapGet("/", () =>
{
    string welcome = "Welcome to the Fifteen Game API!  Use the /new endpoint to generate a random starting position, and the /solve endpoint to solve a given position.";

     return Results.Ok(welcome);
});

app.Run();



