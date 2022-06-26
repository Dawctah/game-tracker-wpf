namespace Core.Models
{
    public class Game
    {
        private int priority;

        public string Name { get; set; }
        public int Index { get; set; }
        public int Priority
        {
            get => priority;
            set
            {
                priority = value;

                if (priority > 20)
                {
                    priority = 20;
                }
                else if (priority < 0)
                {
                    priority = 0;
                }
            }
        }
        public bool Owned { get; set; }
        public float HoursToBeatAvg { get; set; }
        public DateTime DateCompleted { get; set; }

        public Game()
        {
            Name = string.Empty;
            Index = 0;
            Priority = 0;
            Owned = true;
            HoursToBeatAvg = 0;
            DateCompleted = new DateTime();
        }

        public Game(string name)
        {
            Name = name;
            Index = 0;
            Priority = 0;
            Owned = true;
            HoursToBeatAvg = 0;
            DateCompleted = new DateTime();
        }

        public Game(string name, DateTime dateCompleted)
        {
            Name = name;
            Index = 0;
            Priority = 0;
            Owned = true;
            HoursToBeatAvg = 0;
            DateCompleted = dateCompleted;
        }

        public Game(string name, int priority)
        {
            Name = name;
            Index = 0;
            Priority = priority;
            Owned = true;
            HoursToBeatAvg = 0;
            DateCompleted = new DateTime();
        }

        public Game(string name, int index, int priority, bool owned, float hoursToBeatAvg)
        {
            Name = name;
            Index = index;
            Priority = priority;
            Owned = owned;
            HoursToBeatAvg = hoursToBeatAvg;
        }

        public override string ToString()
        {
            string result = Name;
            if (Priority > 0)
                result += ", Priority: " + Priority;

            if (Priority != 0 && !Owned)
                result += " (Unowned)";

            if (DateCompleted != new DateTime())
            {
                result += $" {DateCompleted.Year}/{DateCompleted.Month}/{DateCompleted.Day}";
            }

#if DEBUG
            result += " INDEX: " + Index;
#endif

            return result;
        }
    }
}