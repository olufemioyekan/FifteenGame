using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FifteenGame.Extensions
{
    /// <summary>
    /// Contains extension methods for the types in the AI.FifteenGame namespace.   
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Compares two MoveDirection instances and returns a boolean value indicating if they represent opposite directions.  
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="newDirection"></param>
        /// <returns></returns>
        public static bool IsOpposite(this MoveDirection direction, MoveDirection newDirection)
        {
            var map = new Dictionary<MoveDirection, MoveDirection>
            {
                { MoveDirection.Up, MoveDirection.Down}
                , { MoveDirection.Down, MoveDirection.Up}
                , { MoveDirection.Left, MoveDirection.Right}
                , { MoveDirection.Right, MoveDirection.Left}
            };

            return map[direction] == newDirection;
        }
    }
}
