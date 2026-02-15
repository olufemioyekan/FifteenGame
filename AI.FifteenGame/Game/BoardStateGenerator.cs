using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FifteenGame.Agent;
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
            var iterations = 150;
            var solutionPosition = CreateSolutionBoard();
            var random = new Random();
            var current = solutionPosition;
            var prevEmpty = current.EmptySquare;

            while (iterations > 0)
            {
                var moves = current.LegalMoves.ToList();
                var move = moves[random.Next(moves.Count)];
                var next = current.Move(move);

                if (next.EmptySquare.Equals(prevEmpty))
                    continue;

                prevEmpty = current.EmptySquare; 
                current = next;
                iterations--;
            }
            return current;

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

            for (int y = 1; y <= 4; y++)
                for (int x = 1; x <= 4; x++)
                {
                    var sq = new BoardSquare { X = x, Y = y };
                    map.Add(sq, sq.SolutionPiece);
                }

            return new BoardState(map);
        }
    }
}
