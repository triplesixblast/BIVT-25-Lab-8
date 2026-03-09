namespace Lab8.Purple
{
    public class Task1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumpIndex;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs => (double[])_coefs.Clone();
            public int[,] Marks => (int[,])_marks.Clone();
            public double TotalScore
            {
                get
                {
                    double total = 0;
                    for (int jump = 0; jump < 4; jump++)
                    {
                        int[] row = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            row[j] = _marks[jump, j];
                        }
                        total += CalculateJumpScore(row, _coefs[jump]);
                    }
                    return total;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4];
                for (int i = 0; i < 4; i++) { _coefs[i] = 2.5; }
                _marks = new int[4, 7];
                _jumpIndex = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || coefs.Length != 4) return;
                for (int i = 0; i < 4; i++)
                {
                    if (coefs[i] < 2.5 || coefs[i] > 3.5) return;
                    _coefs[i] = coefs[i];
                } 
            }

            public void Jump(int[] marks)
            {
                if (marks == null || marks.Length != 7 || _jumpIndex >= 4) return;
                for (int j = 0; j < 7; j++)
                {
                    if (marks[j] < 0 || marks[j] > 6) return;
                    _marks[_jumpIndex, j] = marks[j];
                }
                _jumpIndex++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                var sorted = array.OrderByDescending(p => p.TotalScore).ToArray();
                Array.Copy(sorted, array, array.Length);
            }

            private static double CalculateJumpScore(int[] marks, double coef)
            {
                int min = marks[0], max = marks[0], sum = 0;
                for (int i = 0; i < 7; i++)
                {
                    int m = marks[i];
                    sum += m;
                    if (m < min) min = m;
                    if (m > max) max = m;
                }
                int clearedSum = sum - max - min;
                return clearedSum * coef;
            }

            public void Print()
            {
                System.Console.WriteLine($"{_surname} {_name} | Total: {TotalScore}");
                for (int i = 0; i < 4; i++)
                {
                    System.Console.WriteLine($"Jump {i + 1} (coef {_coefs[i]}): ");
                    for (int j = 0; j < 7; j++)
                    {
                        Console.Write(_marks[i, j]);
                        if (j < 6) Console.Write(" ");
                    }
                    System.Console.WriteLine();
                }
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _markIndex;

            public string Name => _name;

            public Judge(string name, int[] marks)
            {
                _name = name;
                _marks = marks;
                _markIndex = 0;
            }

            public int CreateMark()
            {
                int mark = _marks[_markIndex++];
                if (_markIndex >= _marks.Length) _markIndex = 0;
                return mark;
            }

            public void Print()
            {
                System.Console.WriteLine($"Name: {_name}; Marks: {string.Join(" ", _marks)}");
            }
        }

        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => (Judge[])_judges.Clone();
            public Participant[] Participants => (Participant[])_participants.Clone();

            public Competition(Judge[] judges)
            {
                _judges = judges;
                _participants = Array.Empty<Participant>();
            }

            public void Evaluate(Participant jumper)
            {
                int[] marks = new int[_judges.Length];
                for (int i = 0; i < marks.Length; i++)
                {
                    marks[i] = _judges[i].CreateMark();
                }
                jumper.Jump(marks);
            }

            public void Add(Participant participant)
            {
                Array.Resize(ref _participants, _participants.Length+1);
                _participants[^1] = participant;
                Evaluate(participant);
            }
            public void Add(Participant[] participants)
            {
                foreach(var participant in participants)
                    Add(participant);
            }

            public void Sort()
            {
                Participant.Sort(_participants);
            }
        }
    }
}