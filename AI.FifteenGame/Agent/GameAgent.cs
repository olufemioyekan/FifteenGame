using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FifteenGame.Extensions;

namespace AI.FifteenGame.Agent
{
    /// <summary>
    /// The GameAgent class represents the AI for this implementation of the Fifteen Game.  The 
    /// <see cref="SolveGame"/> method generates a random Fifteen Game starting position and solves it by using the A* search algorithm.
    /// </summary>
    /// <remarks>
    /// The A* search algorithm is solved by searching <see cref="Node"/> objects for the best possible path to the puzzle solution.  Each node is assigned
    /// a cost value, defined by the function f(n) = g + h, where g is the number of moves it takes to reach the Node's position, and h is the heuristic estimate 
    /// to the solution state.
    /// 
    /// The <see cref="Frontier"/> property keeps track of all the nodes which are currently available to be evaluated in lowest-cost-first-order.  The <see cref="Explored"/> property 
    /// contains all nodes that have already been removed from the Frontier and evaluated.  The Path property maintains a list of nodes that are possibly part of the 
    /// solution path.  The 
    /// </remarks>
    /// <example>
    /// Below is the implementation of the A* search algorithm for the Fifteen Game.  
    /// <code language="CSharp" title="A* Search for Fifteen Game" source="..\AI.FifteenGame\AI\GameAgent.cs" region="Game Solution"/>
    /// </example>
    public class GameAgent
    {
        /// <summary>
        /// A list of nodes ready to be explored.
        /// </summary>
        private readonly PriorityQueue<Node> frontier = new PriorityQueue<Node>();

        /// <summary>
        /// The staring position to be solved by this instance.  
        /// </summary>
        public BoardState InitialPosition { get; private set; }
        /// <summary>
        /// The number of iterations performed so far by the A* search algorithm.  
        /// </summary>
        public int Iterations { get; private set; }
        /// <summary>
        /// Event that fires when a new BestCost node has been discoved.  
        /// </summary>
        public event EventHandler<ClosestSolutionFoundEventArgs> BestCostFound;

        private int bestCost;

        /// <summary>
        /// Creates a new GameAgent instance.
        /// </summary>
        public GameAgent()
        {
            InitialPosition = BoardStateGenerator.CreateRandomBoard();
        }
        #region Game Solution
        /// <summary>
        /// Solves the fifteen game by first generating a random position.  The solution is returned as a collection of <see cref="Node"/>
        /// objects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> SolveGame()
        {
            var initialNode = new Node(InitialPosition, null, 0);
            frontier.Push(initialNode);
            bestCost = initialNode.PathCost;
            var finalNode = FindSolutionNode(initialNode);

            return SolvedMoveList(finalNode).Reverse();
        }

        private Node FindSolutionNode(Node initialNode)
        {
            var openBest = new Dictionary<BoardState, Node>();
            var closedBestG = new Dictionary<BoardState, int>();

            openBest[initialNode.BoardState] = initialNode;

            while (frontier.Count > 0)
            {
                var current = frontier.Pop();
                openBest.Remove(current.BoardState);

                if (current.BoardState.IsSolutionState)
                    return current;

                // mark closed with best g
                closedBestG[current.BoardState] = current.MoveCount;

                foreach (var move in current.BoardState.LegalMoves)
                {
                    if (current.Move != null && move.Direction.IsOpposite(current.Move.Direction))
                        continue;

                    var nextBoard = current.BoardState.Move(move);
                    var nextG = current.MoveCount + 1;

                    // closed check
                    if (closedBestG.TryGetValue(nextBoard, out var bestClosedG) && nextG >= bestClosedG)
                        continue;

                    // open check
                    if (openBest.TryGetValue(nextBoard, out var openNode))
                    {
                        if (nextG >= openNode.MoveCount)
                            continue;

                        openNode.SetPathProperties(current, nextG);
                        frontier.Update(openNode);
                        continue;
                    }

                    var nextNode = new Node(nextBoard, move, nextG) { Parent = current };
                    frontier.Push(nextNode);
                    openBest[nextBoard] = nextNode;
                }
            }

            return null;
        }

        #endregion
        private IEnumerable<Node> SolvedMoveList(Node finalNode)
        {
            yield return finalNode;

            var node = finalNode;

            while (node.Parent != null)
            {
                yield return node.Parent;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Invokes all of the delegates currently added to the <see cref="BestCostFound"/> event.  
        /// </summary>
        protected void OnClosestSolutionFoundEventRaised(Node currentNode)
        {
            if (this.BestCostFound != null)
            {
                BestCostFound(this, new ClosestSolutionFoundEventArgs(currentNode));
            }
        }
    }

  
    /// <summary>
    /// EventArgs for the BestCostFound Event.  
    /// </summary>
    public class ClosestSolutionFoundEventArgs : EventArgs
    {
        public readonly Node Node;
      
        /// <summary>
        /// Creates a new instance of BestCostFountEventArgs.
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="node"></param>
        public ClosestSolutionFoundEventArgs(Node node)
        {
            Node = node;
        }

    }
}


