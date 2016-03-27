using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FifteenGame.Extensions;

namespace AI.FifteenGame
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
        public PriorityQueue<Node> Frontier { get; private set; }
        /// <summary>
        /// A list of the nodes that have already been explored by this instance.  
        /// </summary>
        public IList<Node> Explored { get; private set; }
        /// <summary>
        /// A list of nodes that might belong to the solution path.
        /// </summary>
        public IList<Node> Path { get; private set; }
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
            Frontier = new PriorityQueue<Node>(Node.Default);
            Explored = new List<Node>();
            Path = new List<Node>();
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
            Path.Add(initialNode);
            Frontier.Push(initialNode);
            bestCost = initialNode.PathCost;
            var finalNode = FindSolutionNode(initialNode);
            return SolvedMoveList(finalNode).Reverse();
        }

    

        private Node FindSolutionNode(Node initialNode)
        {
            Node finalNode = initialNode;
            while (Frontier.Count > 0)
            {
                var exploredNode = Frontier.Pop();
                Explored.Add(exploredNode);
                Iterations++;

                var board = exploredNode.BoardState;
                var candidates = board.LegalMoves;
                if (exploredNode.HeuristicCost < bestCost)
                {
                    bestCost = exploredNode.HeuristicCost;
                    OnClosestSolutionFoundEventRaised(exploredNode);
                }

                foreach (var candidate in candidates)
                {
                    var moveCount = exploredNode.MoveCount + 1;
                    var candidateBoard = board.Move(candidate);
                    var candidateNode = new Node(candidateBoard, candidate, moveCount);
                    candidateNode.Parent = exploredNode;

                    if (candidateBoard.IsSolutionState)
                        return candidateNode;
                    if (exploredNode.Move != null && candidate.Direction.IsOpposite(exploredNode.Move.Direction))
                        continue;

                    var closedMatch = Explored.FirstOrDefault(x => x.Equals(candidateNode));
                    if (closedMatch != null && closedMatch.PathCost <= candidateNode.PathCost)
                    {
                        closedMatch.SwapPathProperties(candidateNode.Parent, moveCount);
                        continue;
                    }

                    var frontierMatch = Frontier.GetItem(candidateNode);
                    if (frontierMatch != null)
                    {
                        frontierMatch.SwapPathProperties(candidateNode.Parent, moveCount);
                        Frontier.Update(frontierMatch);
                        continue;
                    }

                    Frontier.Push(candidateNode);

                    Path.Add(candidateNode);
                }
            }

            return finalNode;
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


