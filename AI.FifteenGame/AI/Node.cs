using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame
{
    /// <summary>
    /// The Node class stores information related to a particular <see cref="BoardState"/> object to be used
    /// with the A* search algorithm.  It is also used to help generate random board positions.  <see cref="BoardStateGenerator"/>.
    /// </summary>
    /// <remarks>
    /// A Node object represents an "attempt" by the A* path finding alogrithm to solve the fifteen game.  The algorithm is 
    /// defined by the function f(n) = g + h.  
    /// For this problem space, <see cref="MoveCount"/> is g and <see cref="HeuristicCost"/> is h.  
    /// See <see cref="GameAgent"/> for more information.  
    /// </remarks>
    public class Node : IEquatable<Node>, IComparer<Node>, IIndexedObject
    { 

        public static Node Default
        {
            get { return new Node(null, null, 0); }
        }
        /// <summary>
        /// The BoardState associated with this instance.
        /// </summary>
        public readonly BoardState BoardState;

        /// <summary>
        /// The GameMove that resulted in the current BoardState.
        /// </summary>
        public readonly GameMove Move;

        /// <summary>
        /// The PathCost of this instance.
        /// </summary>
        public int PathCost
        {
            get { return MoveCount + HeuristicCost; }
        }
        /// <summary>
        /// The total number of moves made to reach the current board state from an arbitrary initial starting position.  
        /// </summary>
        public int MoveCount { get; private set; }

        /// <summary>
        /// The previous Node in this instance's path.
        /// </summary>
        public Node Parent { get; internal set; }

        /// <summary>
        /// The total number of misplaced pieces in this instance's board position. 
        /// </summary>
        public int Misplaced { get { return this.BoardState.MisplacedPieces; } }

        /// <summary>
        /// The total Manhattan Distance to their goal squares of all the misplaced pieces in this instance's board position.  
        /// </summary>
        public int TotalDistance { get { return this.BoardState.TotalDistanceRemaining; } }

        /// <summary>
        /// The HeuristicCost of this instance.  The heuristic used is max(Misplaced, TotalDistance).  
        /// </summary>
        public int HeuristicCost
        {
            get { return Misplaced >= TotalDistance ? Misplaced : TotalDistance; }
        }

        /// <summary>
        /// Maintains an index for this instance for use with <see cref="PriorityQueue{Node}" collections./>
        /// </summary>
        public int Index { get; set; }
     
        /// <summary>
        /// Creates a new Node instance.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="move"></param>
        /// <param name="moveCount"></param>
        public Node(BoardState board, GameMove move, int moveCount)
        {
            BoardState = board;
            Move = move;
            MoveCount = moveCount;
        }

        /// <summary>
        /// Compares a Node object with the current instance for equality.  For the purposes of searching, two Nodes are considered equal if 
        /// their BoardState properties contain the same elements. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Equals(Node node)
        {
            return this.BoardState.Equals(node.BoardState);
        }

        /// <summary>
        /// Replaces this instance's Parent and MoveCount properties with the supplied values. 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="moveCount"></param>
        public void SwapPathProperties(Node parent, int moveCount)
        {
            Parent = parent;
            MoveCount = moveCount;
        }

        /// <summary>
        /// Returns a custom string representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Cost {0} MoveCount {1} Misplaced {2} Total Distance {3}", PathCost, MoveCount, Misplaced, TotalDistance);
        }
        /// <summary>
        /// Compares two Node objects based on their PathCost value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Node x, Node y)
        {
            if (x.PathCost < y.PathCost)
                return -1;
            else if (x.PathCost > y.PathCost)
                return 1;
            return 0;
        }
    }
}
