using AI.FifteenGame.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AI.FifteenGame
{
    /// <summary>
    /// The BoardState Class represents a single position in the Fifteen Game.  
    /// </summary>
    public class BoardState: IEquatable<BoardState>
    {
        #region Properties

        /// <summary>
        /// A Dictionary representing a map of the squares and their piece values for his instance.  The keys are represented
        /// by <see cref="BoardSquare"/> objects, and the values represent the piece number (null is used for the empty square).
        /// </summary>
        [JsonIgnore]
        public IDictionary<BoardSquare, int?> SquareMap { get; private set; }

        public IDictionary<string, int?> Board => SquareMap.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);


       
        /// <summary>
        /// A <see cref="BoardSquare"/> representing the empty square of this instance.
        /// </summary>
        public BoardSquare EmptySquare { get; private set; }

        /// <summary>
        /// A collection of <see cref="GameMove"/> objects representing all of the legal moves for this instance.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<GameMove> LegalMoves
        {
            get
            {

                foreach (var square in SquareMap.Keys)
                {
                    if (BoardSquare.AreAdjacent(square, EmptySquare))
                    {
                        var direction = GetMoveDirection(square);
                        yield return new GameMove(SquareMap[square].Value, square, direction);
                    }
                }
            }
        }

        /// <summary>
        /// A collection containing all of the squares in this position that are not occupied by their solution piece.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<BoardSquare> MisplacedSquares
        {
            get
            {
                return SquareMap.Where(kvp => kvp.Value != kvp.Key.SolutionPiece && kvp.Value != null).Select(kvp => kvp.Key);
            }
        }

        private int? _misplacedPieces = null;

        /// <summary>
        /// The number of squares that do not have their solution piece in the position represented by this instance.
        /// </summary>
        public int MisplacedPieces
        {
            get
            {
                if (!_misplacedPieces.HasValue)
                {
                    _misplacedPieces = MisplacedSquares.Count();
                }
                return _misplacedPieces.Value;
            }
        }

        private int? _totalDistance = null;

        /// <summary>
        /// The total Manhattan Distance between all the misplaced pieces in the <see cref="MisplacedSquares"/> collecions to their 
        /// solution positions.
        /// </summary>
        public int TotalDistanceRemaining
        {
            get
            {
                if (_totalDistance == null)
                {
                    int total = 0;

                    foreach (var kvp in SquareMap)
                    {
                        var currentSquare = kvp.Key;
                        var piece = kvp.Value;

                        if (piece == null) continue; // ignore blank

                        // compute goal square for this piece
                        int goalX = ((piece.Value - 1) % 4) + 1;
                        int goalY = ((piece.Value - 1) / 4) + 1;

                        total += Math.Abs(currentSquare.X - goalX) + Math.Abs(currentSquare.Y - goalY);
                    }

                    _totalDistance = total;
                }

                return _totalDistance.Value;
            }
        }

        /// <summary>
        /// Returns a boolean value indicating if this state is the same as the solution state for the game.  
        /// </summary>
        public bool IsSolutionState
        {
            get
            {
                return SquareMap.All(kvp => kvp.Key.SolutionPiece == kvp.Value);
            }
        }

        #endregion
        /// <summary>
        /// Creates a new instance with the given square-piece map.
        /// </summary>
        /// <param name="boardState"></param>
        public BoardState(IDictionary<BoardSquare, int?> boardState)
        {
            SquareMap = new Dictionary<BoardSquare, int?>(boardState);
            EmptySquare = SquareMap.Single(kvp => kvp.Value == null).Key;
        }

        /// <summary>
        /// Returns a new BoardState object which represents the supplied move applied to the current position.  
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public BoardState Move(GameMove move)
        {
            var square = move.StartingSquare;
            var direction = move.Direction;
            if (!SquareMap.ContainsKey(square))
                throw new InvalidOperationException("The piece to move does not exist on the board.");

            var piece = SquareMap[square];

            if (piece == null)
                throw new InvalidOperationException("No piece exists to move from the given square.");

            var candidateSquare = square.Traverse(direction);
            var candidateIsEmpty = SquareMap[candidateSquare] == null;

            if (!candidateIsEmpty)
                throw new InvalidOperationException("A game piece must move to an empty square.");

            var newState = new BoardState(SquareMap);
            newState.SquareMap[candidateSquare] = piece;
            newState.SquareMap[square] = null;
            newState.EmptySquare = square;
            return newState;
        }
        /// <summary>
        /// Returns a string representation of this instance.  
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Empty {0} Distance {1} Misplaced {2}", EmptySquare, TotalDistanceRemaining, MisplacedPieces);
        }
        /// <summary>
        /// Checks for equality between this instance and another BoardState based on the position of the boards.  
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BoardState other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (SquareMap.Count != other.SquareMap.Count) return false;

            // order-independent
            foreach (var kvp in SquareMap)
            {
                if (!other.SquareMap.TryGetValue(kvp.Key, out var v)) return false;
                if (kvp.Value != v) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj is BoardState bs && Equals(bs);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Ensure order-independent hashing by sorting squares by coordinates
                foreach (var kvp in SquareMap.OrderBy(p => p.Key.X).ThenBy(p => p.Key.Y))
                {
                    hash = hash * 23 + kvp.Key.GetHashCode();
                    hash = hash * 23 + (kvp.Value?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }


       
        
        private MoveDirection GetMoveDirection(BoardSquare square)
        {
            if (square.X == EmptySquare.X)
                return square.Y == EmptySquare.Y + 1 ? MoveDirection.Up : MoveDirection.Down;
            if (square.Y == EmptySquare.Y)
                return square.X == EmptySquare.X + 1 ? MoveDirection.Left : MoveDirection.Right;
            throw new InvalidOperationException("The square selected to move is not adjacent to the empty square.");
        } 


    }
}

