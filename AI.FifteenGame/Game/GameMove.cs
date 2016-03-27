using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame
{
    /// <summary>
    /// Represents a move in the fifteen game.  
    /// </summary>
    public class GameMove
    {
        /// <summary>
        /// The piece that is moved by this instance.
        /// </summary>
        public readonly int Piece;
        /// <summary>
        /// The starting sqaure of this instance.  
        /// </summary>
        public readonly BoardSquare StartingSquare;
        /// <summary>
        /// The direction of this move.  
        /// </summary>
        public readonly MoveDirection Direction;
        /// <summary>
        /// The square the piece involved with this move occupies after the move occurs.  
        /// </summary>
        public readonly BoardSquare EndSquare;

        /// <summary>
        /// Creates a new GameMove instance.  
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="square"></param>
        /// <param name="direction"></param>
        public GameMove(int piece, BoardSquare square, MoveDirection direction)
        {
            Piece = piece;
            StartingSquare = square;
            Direction = direction;
            EndSquare = StartingSquare.Traverse(direction);
        }

        /// <summary>
        /// Returns a custom string representation of this instance.  
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Move {0} {1}", Piece, Direction);
        }
    }
}
