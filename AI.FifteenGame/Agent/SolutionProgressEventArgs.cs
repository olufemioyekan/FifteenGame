using System;
using System.IO;

namespace AI.FifteenGame.Agent
{
    /// <summary>
    /// EventArgs for the BestCostFound Event.  
    /// </summary>
    public class SolutionProgressEventArgs : EventArgs
    {
        public readonly Node Node;
        public readonly int FrontierSize;
        public readonly int OpenCount;
        public readonly int ClosedCount;
        public readonly TimeSpan ElapsedTime;

        /// <summary>
        /// Creates a new instance of BestCostFountEventArgs.
        /// </summary>
        /// <param name="node"></param>
        public SolutionProgressEventArgs(Node node)
        {
            Node = node;
        }

        public SolutionProgressEventArgs(Node node, int frontierSize, int openCount, int closedCount, TimeSpan elapsedTime)
        {
            Node = node;
            FrontierSize = frontierSize;
            OpenCount = openCount;
            ClosedCount = closedCount;
            ElapsedTime = elapsedTime;
        }

    }
}


