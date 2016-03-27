using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame
{
    /// <summary>
    /// Class responsible for creating a simple text representation of a single board state.  
    /// </summary>
    public class BoardStateTextRenderer
    {
        /// <summary>
        /// The BoardState representing the position of this instance.  
        /// </summary>
        public readonly BoardState BoardState;
        /// <summary>
        /// Creates a new instance with the given BoardState position.  
        /// </summary>
        /// <param name="boardState"></param>
        public BoardStateTextRenderer(BoardState boardState)
        {
            BoardState = boardState;
        }
        /// <summary>
        /// Creates a string representing a diagram of the psotion for this instance.  
        /// </summary>
        /// <returns></returns>
        public string RenderAsText()
        {
            var sb = new StringBuilder();
            var orderedBoard = BoardState.SquareMap.OrderBy(kvp => kvp.Key.Y).ThenBy(kvp => kvp.Key.X);
            int square = 0;
            foreach (var kvp in orderedBoard)
            {
                var squareText = RenderSquareText(kvp.Key, kvp.Value).TrimEnd(Environment.NewLine.ToCharArray());
                sb.Append(squareText);
                square++;
                
                if (square % 4 == 0)
                {
                    sb.Append("|");
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
            
        }

        private string RenderSquareText(BoardSquare square, int? piece)
        {
            return string.Format("|  {0} ",
                piece.HasValue && piece.Value > 9 ? piece.Value.ToString() : 
                !piece.HasValue ? "  " :
                piece.Value + " ");
        }
    }
}
