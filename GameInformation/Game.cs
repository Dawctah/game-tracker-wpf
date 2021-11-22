using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInformation
{
    [Serializable]
    public class Game
    {
        private int priority;
        public string Name { get; set; }

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

        public override string ToString()
        {
            if (Priority > 0)
                return Name + ", Priority: " + Priority;
            else
                return Name;
        }
    }
}
