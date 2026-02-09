using System;
using System.Collections.Generic;

namespace AI.FifteenGame
{
    /// <summary>
    /// Helper class for creating BoardSquare instances from external assemblies.
    /// </summary>
    public static class BoardSquareFactory
    {
        /// <summary>
        /// Creates a BoardSquare from X and Y coordinates.
        /// </summary>
        /// <param name="x">The X coordinate (1-4)</param>
        /// <param name="y">The Y coordinate (1-4)</param>
        /// <returns>A new BoardSquare instance</returns>
        public static BoardSquare Create(int x, int y)
        {
            if (x < 1 || x > 4 || y < 1 || y > 4)
                throw new ArgumentException("Board coordinates must be between 1 and 4");
                
            return new BoardSquare { X = x, Y = y };
        }
        
        /// <summary>
        /// Parses a BoardSquare from a string in "X:Y" format.
        /// </summary>
        /// <param name="squareString">The square string in "X:Y" format (e.g., "1:1")</param>
        /// <returns>A new BoardSquare instance</returns>
        public static BoardSquare Parse(string squareString)
        {
            var parts = squareString.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
            {
                throw new ArgumentException($"Invalid square format: {squareString}. Expected format: 'X:Y' (e.g., '1:1')");
            }
            
            return Create(x, y);
        }
    }
}
