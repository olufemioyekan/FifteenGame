using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame
{
    /// <summary>
    /// The BoardSquare class represents a sqauare on the fifteen game board.  
    /// </summary>
    public struct BoardSquare
    {
        /// <summary>
        /// The x value (column number) of this square.
        /// </summary>
        public int X { get; internal set; }
        /// <summary>
        /// The y value (row number) of this square.
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        /// Returns a new BoardSquare obtained by moving away from this sqaure in the supplied direction.  
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public BoardSquare Traverse(MoveDirection direction)
        {
            var x = 0; var y = 0;

            switch (direction)
            {
                case MoveDirection.Up:
                    y = this.Y - 1;
                    x = this.X;
                    break;
                case MoveDirection.Down:
                    y = this.Y + 1;
                    x = this.X;
                    break;
                case MoveDirection.Left:
                    x = this.X - 1;
                    y = this.Y;
                    break;
                case MoveDirection.Right:
                    x = this.X + 1;
                    y = this.Y;
                    break;
                default:
                    break;
            }

            var nextSquare = new BoardSquare { X = x, Y = y };
            if (nextSquare.X < 1 || nextSquare.X > 4 || nextSquare.Y < 1 || nextSquare.Y > 4)
                throw new InvalidOperationException("The destination square does not exist on the board");
            return nextSquare;

        }

        /// <summary>
        /// An integer representing the piece occupying this square, or null if the square is empty.  
        /// </summary>
        public int? SolutionPiece
        {
            get
            {
                if (X == 4 && Y == 4)
                    return null;
                return (4 * (Y - 1)) + X;
            }
        }

        /// <summary>
        /// Returns a custom string representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", X, Y);
        }

        /// <summary>
        /// Returns true if the two supplied squares are adjacent (in the "Manhattan" sense), otherwise, returns false.
        /// </summary>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns></returns>
        public static bool AreAdjacent(BoardSquare square1, BoardSquare square2)
        {
            var sideBySide = (Math.Abs(square1.X - square2.X) == 1) && (square1.Y == square2.Y);
            var aboveBelow = ((Math.Abs(square1.Y - square2.Y) == 1) && (square1.X == square2.X));
            return sideBySide || aboveBelow;
                
        }
    }
}
