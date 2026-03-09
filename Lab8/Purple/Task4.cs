namespace Lab8.Purple
{
    public class Task4
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;
            private int _timeIndex;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _timeIndex = 0;
            }

            public void Run(double time)
            {
                if (_timeIndex == 0 && time >= 0)
                {
                    _time = time;
                    _timeIndex++;
                }
            }

            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                var sorted = array.OrderBy(s => s.Time).ToArray();
                Array.Copy(sorted, array, array.Length);
            }

            public void Print()
            {
                System.Console.WriteLine($"{_name} {_surname}: {_time}");
            }
        }

        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname): base(name, surname){}
            public SkiMan(string name, string surname, double time): base(name, surname)
            {
                Run(time);
            }
        }
        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname): base(name, surname){}
            public SkiWoman(string name, string surname, double time): base(name, surname)
            {
                Run(time);
            }
        }

        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            public Group(string name)
            {
                _name = name;
                _sportsmen = Array.Empty<Sportsman>();
            }
            public Group(Group group)
            {
                _name = group.Name;
                _sportsmen = new Sportsman[group.Sportsmen.Length];
                Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
            }

            public void Add(Sportsman sportsman)
            {
                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[^1] = sportsman;
            }
            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null) return;
                foreach (var sportsman in sportsmen)
                    Add(sportsman);
            }
            public void Add(Group group)
            {
                Add(group.Sportsmen);
            }

            public void Sort()
            {
                var sorted = _sportsmen.OrderBy(s => s.Time).ToArray();
                Array.Copy(sorted, _sportsmen, _sportsmen.Length);
            }

            public static Group Merge(Group group1, Group group2)
            {
                var finalists = new Group("Финалисты");
                finalists.Add(group1);
                finalists.Add(group2);
                finalists.Sort();
                return finalists;
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = _sportsmen.Where(s => s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                Sportsman.Sort(_sportsmen);
                Split(out Sportsman[] men, out Sportsman[] women);
                
                Sportsman[] result = new Sportsman[_sportsmen.Length];

                int menIndex = 0, womenIndex = 0, resultIndex = 0;
                bool takeMan = _sportsmen[0] is SkiMan;
                while(menIndex < men.Length && womenIndex < women.Length)
                {
                    if (takeMan)
                        result[resultIndex++] = men[menIndex++];
                    else
                        result[resultIndex++] = women[womenIndex++];
                    takeMan = !takeMan;
                }
                while(menIndex < men.Length)
                    result[resultIndex++] = men[menIndex++];
                while(womenIndex < women.Length)
                    result[resultIndex++] = women[womenIndex++];
                _sportsmen = result;
            }

            public void Print()
            {
                System.Console.WriteLine(_name);
                var list = Sportsmen;
                for (int i = 0; i < list.Length; i++)
                {
                    System.Console.WriteLine($"{i+1} ");
                    list[i].Print();
                }
            }
        }
    }
}