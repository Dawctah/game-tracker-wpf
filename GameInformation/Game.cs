using System;

namespace GameInformation
{
    [Serializable]
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
                if (priority > 10)
                    priority = 10;
            }
        }

        public bool Owned { get; set; }

        public override string ToString()
        {
            string result = Name;
            if (Priority > 0)
                result += ", Priority: " + Priority;

            if (!Owned)
                result += " (Unowned)";

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
