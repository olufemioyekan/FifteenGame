using AI.FifteenGame.Agent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("***Fifteen Game***");
            var solution = BoardStateGenerator.CreateSolutionBoard();
            var solutionBoardText = RenderBoard(solution);
            sb.AppendLine(solutionBoardText);
            System.Console.WriteLine("Press enter to start the game.");
            System.Console.ReadKey();


            var watch = new Stopwatch();
            watch.Start();
            var solutionText = SolveFifteenGame();
            sb.AppendLine(solutionText);
            watch.Stop();
            System.Console.WriteLine("Solution reached after {0} seconds.", watch.Elapsed.TotalSeconds);


            System.Console.ReadKey();
            System.IO.File.WriteAllText("GameSoution.txt", sb.ToString());
            System.Console.WriteLine(sb);


        }

        private static string SolveFifteenGame()
        {
            var sb = new StringBuilder();
            var agent = new GameAgent();
            sb.AppendLine("Initial Position:");
            sb.AppendLine("********************");
            var initialDiagram = (RenderBoard(agent.InitialPosition));
            sb.Append(initialDiagram);
            System.Console.WriteLine(initialDiagram);
            agent.BestCostFound += HandleBestCostFound;
            var solutionNodes = agent.SolveGame();
            var moves = solutionNodes.Select(x => x.Move);
            int currentMove = 0;
            var total = solutionNodes.Count();
            foreach (var node in solutionNodes.Skip(1))
            {
                currentMove++;
                sb.AppendFormat("{0}{1}.  {2}", Environment.NewLine, currentMove, node.Move);
                sb.AppendLine(RenderBoard(node.BoardState));
            }

            return sb.ToString();
        }

        private static void HandleBestCostFound(object sender, ClosestSolutionFoundEventArgs args)
        {
            var sb = new StringBuilder();
            var agent = (GameAgent)sender;
            System.Console.WriteLine("New Best Cost Found: {0} at move {1} after {2} iterations.",
                args.Node.HeuristicCost, args.Node.MoveCount, agent.Iterations);
        }

        private static string RenderBoard(BoardState state)
        {
            var sb = new StringBuilder();
            var renderer = new BoardStateTextRenderer(state);
            var boardText = renderer.RenderAsText();
            sb.AppendLine(string.Empty);
            sb.AppendLine(boardText);
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }



    }
}
