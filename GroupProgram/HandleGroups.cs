using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroupProgram
{
    class HandleGroups
    {
        public List<string> listOfNames;
        public bool avoidRecurringGroups;
        private static string filePathToPreviousGroups = "previousGroups.txt";
        public HandleGroups(List<string> listOfNames)
        {
            
            this.listOfNames = listOfNames;
            AimToAvoidRecurringGroups();
            
        }
        
        public HandleGroups(string filePath)
        {
            this.listOfNames = ReadInListFromFile(filePath);
            AimToAvoidRecurringGroups();
        }

        public List<Group> DevideMembersEqually()
        {

            List<string> inList = RandomizedList();

            Console.Write("How many groups?: ");
            int numberOfGroups = int.Parse(Console.ReadLine());

         

            int[] numberOfMembersInEachGroup = SetFairGroupDistribution(listOfNames.Count , numberOfGroups);



            return MakeAListOutOfThese(numberOfMembersInEachGroup, inList);
        }

        public List<Group> DevideMembersByGroupSize()
        {
            int desiredGroupSize = GetInteger("What is your desired group size?");
            List<string> randomizedList = RandomizedList();
            int numberOfMembers = listOfNames.Count;
            int bestNumberOfGroups = Convert.ToInt32(Math.Round((double)numberOfMembers / (double)desiredGroupSize));

            int[] membersPerGroup = SetFairGroupDistribution(numberOfMembers, bestNumberOfGroups);
            
            return MakeAListOutOfThese(membersPerGroup, randomizedList);
        }

        public List<string> RandomizedList()
        {
            return RandomizeListMethod(listOfNames);
        }

        public List<Group> ForceToOddGroups()
        {
            int numberOfGroups = GetNumberOfGroups();
            return ForceToOddGroupsMethod(listOfNames , numberOfGroups);
        }

        public List<Group> ForceToOddGroups(List<string> inList)
        {
            int numberOfGroups = GetNumberOfGroups();
            return ForceToOddGroupsMethod(inList, numberOfGroups);
        }

        public List<Group> ManuallyPickGroupSize()
        {
            List<List<string>> outList = new List<List<string>>();
            //todo: continue here
            int[] groupDivision = SetManualGroupDivision();
            Console.Write("Do you want to shuffle the name list? (y/n): ");
            
            while (true)
            {
                string input = Console.ReadLine();
                if(input.Length== 1)
                {
                    char reply = Char.ToUpper(Char.Parse(input));
                    if (reply == 'N')
                    {
                        return MakeAListOutOfThese(groupDivision, listOfNames);
                    }else
                    {
                        return MakeAListOutOfThese(groupDivision, RandomizedList());
                    }
                    
                }
                else
                {
                    Console.WriteLine("One character only!");
                }
            }
            
          
        }

        private void AimToAvoidRecurringGroups()
        {
            char avoidRecurringGroupsReply = Char.ToUpper(GetCharacter("Do you want to avoid recurring groups?: "));
            avoidRecurringGroups = (avoidRecurringGroupsReply == 'Y');

            if ((avoidRecurringGroups == false) && (File.Exists(filePathToPreviousGroups)))
            {
                char deleteListFileReply = Char.ToUpper(GetCharacter("Do you want to erase file of previous groups?: "));
                if (deleteListFileReply == 'Y')
                {
                    File.Delete(filePathToPreviousGroups);
                }
            }

        }

        private char GetCharacter(string question)
        {
            while (true)
            {
                Console.Write(question);
                string input = Console.ReadLine();
                if (input.Length == 1)
                {
                    return input[0];
                }
            }

        }

        private List<string> ReadInListFromFile(string pathName)
        {
            string[] inText = System.IO.File.ReadAllLines(pathName);
            return new List<string>(inText);
        }

        private List<Group> MakeAListOutOfThese(int[] groupDistribution , List<string> randomizedNameList)
        {
            List<Group> outList = new List<Group>();
            Random random = new Random();
            int counter = 0;
            for (int i = 0; i < groupDistribution.Length; i++)
            {
                outList.Add(new Group());
                outList[i].NumberOfMembers = groupDistribution[i];
                outList[i].Names = new string[outList[i].NumberOfMembers];
                outList[i].GroupLeader = random.Next(outList[i].NumberOfMembers);
                for (int j = 0; j < groupDistribution[i]; j++)
                {
                    outList[i].Names[j] = randomizedNameList[counter++];
                }
               
            }

            if (avoidRecurringGroups && File.Exists(filePathToPreviousGroups))
            {
                ShuffleRecurringGroups(outList , groupDistribution);
            }

            return outList;
        }

        private List<Group> ForceToOddGroupsMethod(List<string> inList , int numberOfGroups)
        {
            
            int[] groupDivision = new int[numberOfGroups];
            bool isOddDivisionPossible = IsOddDivisionPossible(inList, numberOfGroups);

            groupDivision = DivideToOddGroups(inList.Count, numberOfGroups);

            return MakeAListOutOfThese(groupDivision, RandomizedList());

           
        }

        private int[] DivideToOddGroups(int numberOfMembers , int numberOfGroups)
        {
            int[] groupDivision = SetFairGroupDistribution(numberOfMembers , numberOfGroups);

            bool foundEven = false;
            int whereEven = 0;
            
            for (int i = 0; i < groupDivision.Length; i++)
            {
                if (groupDivision[i] % 2 == 0)
                {
                    if (foundEven)
                    {
                        groupDivision[whereEven]--;
                        groupDivision[i]++;
                        foundEven = false;
                    } else
                    {
                        foundEven = true;
                        whereEven = i;
                    }
                }

            }
            if (foundEven)
            {
                Console.WriteLine("Could not divide groups to have odd numbers in each group. One group will have even numbers");
            }

            return groupDivision;

        }

        private bool IsOddDivisionPossible(List<string> inList , int numberOfGroups)
        {
            int numberOfMembers = inList.Count;
            if (((numberOfGroups % 2) != 0) && ((numberOfMembers % 2) != 0))
            {
                return true;
            } else if (((numberOfGroups % 2) == 0) && (numberOfMembers % 2) == 0){
                return true;
            }
            // Todo: check for more posible divisions
            return false;
        }

        private int GetNumberOfGroups()
        {
            return GetInteger("How many groups do you want?: ");
        }

        private static int GetInteger(string question)
        {
            int outNumber;
            while (true)
            {
                Console.Write(question);
                string inText = Console.ReadLine();
                if (Int32.TryParse(inText , out outNumber))
                {
                    return outNumber;
                } else
                {
                    Console.WriteLine("You must enter an integer!");
                }
            }

            
        }

        private int[] SetFairGroupDistribution(int numberOfMembers , int numberOfGroups)
        {
            double membersPerGroupAverage = (double)numberOfMembers / (double)numberOfGroups;
            int[] numbersPerGroup = new int[numberOfGroups];
            int remainingMembers = numberOfMembers;
            int floorMembersPerGroup = Convert.ToInt32(Math.Floor(membersPerGroupAverage));
            for (int counter = 0; counter < numberOfGroups; counter++)
            {
                int membersInThisGroup = floorMembersPerGroup;
                if (((double)remainingMembers % (double)(floorMembersPerGroup) != 0) )
                {
                    membersInThisGroup++;
                }
               
                numbersPerGroup[counter] = PutPeopleInGroups(remainingMembers, membersInThisGroup);
                remainingMembers -= numbersPerGroup[counter];
                
            }
            return numbersPerGroup;
        }     
        
        private int PutPeopleInGroups(int remainingMembers , int desiredNumber)
        {
            if (remainingMembers > desiredNumber)
            {
                return desiredNumber;
            }
            else
            {
                return remainingMembers;
            }
        }

        private int[] SetManualGroupDivision()
        {
            int remainingMembers = listOfNames.Count;
            int groupNumber = 0;
            List<int> groupDivisionList = new List<int>();

            while (remainingMembers > 0)
            {
                Console.WriteLine($"There are currently {remainingMembers} members left");
                int membersInThisGroup = AskForMembersInOneGroup(remainingMembers, groupNumber++);
                groupDivisionList.Add(membersInThisGroup);
                remainingMembers -= membersInThisGroup;
            }
            int[] groupDivision = groupDivisionList.ToArray();
            return groupDivision;
        }

        private int AskForMembersInOneGroup(int remainingMembers , int groupNumber)
        {
            int membersInThisGroup = 0;
            while (true)
            {
                Console.Write($"How many members in group {groupNumber + 1}?: ");
                membersInThisGroup = Convert.ToInt32(Console.ReadLine());
                if (membersInThisGroup > remainingMembers)
                {
                    Console.WriteLine("There are not that many left!");
                }
                else
                {
                    return membersInThisGroup;
                }
            }
        }

        private List<string> RandomizedList(List<string> inList)
        {
            return RandomizeListMethod(inList);
        }

        private static List<string> RandomizeListMethod(List<string> inList)
        {
            List<string> ListToPickFrom = new List<string>(inList);
            
            Random random = new Random();
            List<string> outList = new List<string>();

            for (int i = ListToPickFrom.Count - 1; i >= 0; i--)
            {
                
                int randomItem = random.Next(ListToPickFrom.Count);
                outList.Add(ListToPickFrom[randomItem]);

                ListToPickFrom.RemoveAt(randomItem);
            }

            return outList;
        }
        
        private void ShuffleRecurringGroups(List<Group> list , int[] groupDistribution)
        {
            int safetyCounter = 0;
            while (true)
            {
                int[] recurringMembersInGroup = NumberOfRecurringMemebersInGroup(list);
                bool[] recurrances = AnyTotalRecurrance(recurringMembersInGroup, groupDistribution);
                {
                    if (recurrances.Contains(true))
                    {
                        int maxGroup = Array.IndexOf(recurringMembersInGroup, recurringMembersInGroup.Max());
                        int minGroup = Array.IndexOf(recurringMembersInGroup, recurringMembersInGroup.Min());
                        SwapTwoMembers(list , minGroup , maxGroup);
                    }
                    else
                    {
                        break;
                    }
                }
                if (safetyCounter++ > 50)
                {
                    Console.WriteLine("Program is hung in avoiding recurring groups method! Now leaving method");
                    break;
                }
            }
            
        }

        private void SwapTwoMembers(List<Group> list, int minGroup , int maxGroup)
        {
            Random random = new Random();
            int toMinIndex = random.Next(list[maxGroup].NumberOfMembers);
            int toMaxIndex = random.Next(list[minGroup].NumberOfMembers);

            string toMinName = list[maxGroup].Names[toMinIndex];
            string toMaxName = list[minGroup].Names[toMaxIndex];

            List<string> maxGroupNames = list[maxGroup].Names.ToList();
            List<string> minGroupNames = list[minGroup].Names.ToList();

            maxGroupNames.RemoveAt(toMinIndex);
            minGroupNames.RemoveAt(toMaxIndex);

            maxGroupNames.Add(toMaxName);
            minGroupNames.Add(toMinName);

            list[maxGroup].Names = maxGroupNames.ToArray();
            list[minGroup].Names = minGroupNames.ToArray();

            Console.WriteLine("Swapping members to avoid recurring groups");
        }

        private bool[] AnyTotalRecurrance(int[] recurrance , int[] groupDistribution)
        {
            bool[] isRecurrant = new bool[recurrance.Length];
            for (int i = 0; i < recurrance.Length; i++)
            {
                if (recurrance[i] == groupDistribution[i])
                {
                    isRecurrant[i] = true;
                }
                else
                {
                    isRecurrant[i] = false;
                }
            }
            return isRecurrant;
        }

        private static int[] NumberOfRecurringMemebersInGroup(List<Group> groupList)
        {
            int numberOfGroups = groupList.Count;
            int[] recurringMembers = new int[numberOfGroups];

            List<List<string>> previousGroups = GetPreviousGroups();

            for (int i = 0; i < numberOfGroups; i++)
            {
                recurringMembers[i] = NumberOfRecurringMembers(groupList[i] , previousGroups);
            }

            return recurringMembers;
        }

        private static int NumberOfRecurringMembers(Group group , List<List<string>> previousGroups)
        {
            int recurringMembers = 0;
            for (int i = 0; i < previousGroups.Count; i++)
            {
                int recurringMembersNow = CountRecurringMembers(group, previousGroups[i]);
                if (recurringMembers < recurringMembersNow)
                {
                    recurringMembers = recurringMembersNow;
                }
            }
            return recurringMembers;
        }

        private static int CountRecurringMembers(Group group , List<string> checkGroup)
        {
            int totalRecurringMembers = 0;
            foreach (string name in group.Names)
            {
                if (checkGroup.Contains(name))
                {
                    totalRecurringMembers++;
                }
            }
            return totalRecurringMembers;
        }

        private static List<List<string>> GetPreviousGroups()
        {
            List<List<string>> outList = new List<List<string>>();
            string[] previousGroupsString = File.ReadAllLines(filePathToPreviousGroups);
            int icounter = 0;
            outList.Add(new List<string>());
            for (int i = 0; i < previousGroupsString.Length; i++)
            {
                if (previousGroupsString[i] == "")
                {
                    icounter++;
                    outList.Add(new List<string>());
                }
                else
                {
                    outList[icounter].Add(previousGroupsString[i]);
                }
            }
            return outList;
        }
    }
}
