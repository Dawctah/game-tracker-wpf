using System;

namespace GameInformation
{
    [Serializable]
    public class Game
    {
        private int priority;

        public string Name { get; set; }

        public int Index { get; set; }

        public DateTime DateTimeCompleted { get; set; }

        public int Priority
        {
            get => priority;
            set
            {
                priority = value;
                if (priority > 10)
                    priority = 10;
            }
        }

        public bool Owned { get; set; }

        public int HoursToBeatAvg { get; set; }

        public override string ToString()
        {
            string result = Name;
            if (Priority > 0)
                result += ", Priority: " + Priority;

            if (Priority != 0 && !Owned)
                result += " (Unowned)";

            if (DateTimeCompleted != new DateTime())
            {
                result += $" {DateTimeCompleted.Year}/{DateTimeCompleted.Month}/{DateTimeCompleted.Day}";
            }

#if DEBUG
            result += " INDEX: " + Index;
#endif

            return result;
        }

        public Game()
        {
            Owned = true;
        }
    }
}
