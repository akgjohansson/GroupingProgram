using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroupProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            HandleGroups groups = GetListOfNames();
            
            List<Group> groupList = SetGroupList(groups);
            
            Printer(groupList);

            SaveList(groupList,"previousGroups.txt");

        }

        private static void SaveList(List<Group> groupList , string fileName)
        {
            for (int i = 0; i < groupList.Count; i++)
            {
                File.AppendAllLines(fileName , groupList[i].Names);
                File.AppendAllText(fileName, Environment.NewLine);
            }
            
        }

        private static int GetInteger(string question)
        {
            int outNumber;
            while (true)
            {
                Console.Write(question);
                string inText = Console.ReadLine();
                if (Int32.TryParse(inText, out outNumber))
                {
                    return outNumber;
                }
                else
                {
                    Console.WriteLine("You must enter an integer!");
                }
            }


        }

        private static List<Group> SetGroupList(HandleGroups groups)
        {
            int input = GetInteger($"Do you want to sort groups by {Environment.NewLine}(1): As is natural{Environment.NewLine}(2): Into odd groups, if possible{Environment.NewLine}(3): By specifying desired group size{Environment.NewLine}(4): By manually setting group sizes{Environment.NewLine}{Environment.NewLine}Your choise: ");
            List < List<string> > outList = new List<List<string>>();
            switch (input)
            {
                case 1:
                    return groups.DevideMembersEqually();
                case 2:
                    return groups.ForceToOddGroups();
                case 3:
                    return groups.DevideMembersByGroupSize();
                case 4:
                    return groups.ManuallyPickGroupSize();
                default:
                    return null;
                    
            }
        }

        private static string UserString()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter persons you want to devide into groups. Seperate with ',': ");
            Console.ForegroundColor = ConsoleColor.White;
            string names = Console.ReadLine();
            return names;
        }
        private static List<string> ListMaker(string names)
        {
            string[] nameArray = names.Split('.'); // TODO Rename nameArray & NameList
            List<string> nameList = new List<string>(nameArray);
            return nameList;
        }
        private static void Printer(List<Group> list)
        {
            //int groupNumber = 1;
            foreach (Group names in list)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(names.GroupName);
                

                for (int i = 0; i < names.NumberOfMembers; i++)
                {
                    if (names.IsGroupLeader[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.WriteLine(names.Names[i]);
                }
                Console.Write(Environment.NewLine);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        // TODO Take out this code:
        //private static List<string> OrderByAlphabetical(List<string> nameList)
        //{
        //    List<string> alphaList = new List<string>(nameList);
        //    alphaList.Sort();
        //    for (int i = 0; i < alphaList.Count; i++)
        //    {
        //        nameList[i] = alphaList[i];
        //    }
        //    return nameList;
        //}
        static HandleGroups GetListOfNames()
        {
            HandleGroups groups;
            Console.WriteLine("Do you want to add members via a text file? [y/n]: ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                groups = GetListOfNamesMethod();
            }
            else
            {
                string userString = UserString();
                groups = GetListOfNamesMethod(ListMaker(userString));
            }
            return groups;
        }

        private static HandleGroups GetListOfNamesMethod()
        {
            HandleGroups groups = new HandleGroups(@"..\..\..\nameList.txt");
            return groups;
        }

        private static HandleGroups GetListOfNamesMethod(List<string> nameList)
        {
            HandleGroups groups = new HandleGroups(nameList);
            return groups;
        }
    }
}
