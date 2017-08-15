using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProgram
{
    class Member
    {
        public string Name { get; set; }
        Gender Gender;

        public Member(string name, Gender gender)
        {
            Name = name;
            Gender = gender;
        }
    }
}
