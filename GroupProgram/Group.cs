using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProgram
{
    class Group
    {
        public string[] Names { get; set; }
        public int NumberOfMembers { get; set; }
        public int GroupLeader;
        public bool[] IsGroupLeader
        {
            get
            {
                bool[] isGroupLeader = new bool[NumberOfMembers];
                for (int i = 0; i < NumberOfMembers; i++)
                {
                    isGroupLeader[i] = (i == GroupLeader);
                }
                return isGroupLeader;
            }
        }
    }
}
