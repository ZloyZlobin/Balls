using System;

namespace Balls.Game
{
    public class StatisticSystem
    {
        public static int InGame { get; set; }
        public static int Started { get; set; }
        public static int Missed { get; set; }
        public static int Canceled { get; set; }
        public static int Collisions { get; set; }

        public static event Action OnReset;

        public static void Reset()
        {
            if (OnReset != null)
                OnReset();

            InGame = 0;
            Started = 0;
            Missed = 0;
            Canceled = 0;
            Collisions = 0;
        }
    }
}
