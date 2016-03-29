using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FifteenGame.Extensions;

namespace AI.FifteenGame
{
    /// <summary>
    /// The BoardStateGenerator class generates random starting positions for the fifteen game.  
    /// </summary>
    public static class BoardStateGenerator
    {
       /// <summary>
       /// Returns a BoardState object representing a random starting position for the fifteen game.  
       /// </summary>
       /// <returns></returns>
        public static BoardState CreateRandomBoard()
        {
            var iterations = 25;
            var solutionPosition = CreateSolutionBoard();
            Node currentNode = new Node(solutionPosition, null, 0);
            while (iterations > 0)
            {
                var random = new Random();
                var legalMoves = currentNode.BoardState.LegalMoves.ToList();
                var moveIndex = random.Next(0, legalMoves.Count);
                var move = legalMoves[moveIndex];
                var newBoard = currentNode.BoardState.Move(move);
                var newNode= new Node(newBoard, move, 0);
                newNode.Parent = currentNode;
                if (newNode.Parent.Move != null && newNode.Move.Direction.IsOpposite(newNode.Parent.Move.Direction))
                    continue;
                
                currentNode = newNode;
                --iterations;
            }

            return currentNode.BoardState;

        }
        /// <summary>
        /// Returns a BoardState object in the fifteen game solution position.  
        /// </summary>
        /// <returns></returns>
        public static BoardState CreateSolutionBoard()
        {
            var rows = Enumerable.Range(1, 4);
            var columns = Enumerable.Range(1, 4);
            var map = new Dictionary<BoardSquare, int?>();

            foreach (var column in columns)
            {
                foreach (var row in rows)
                {
                    var gs = new BoardSquare { X = row, Y = column };
                    map.Add(gs, gs.SolutionPiece);
                }
            }

            return new BoardState(map);
        }
    }
}
