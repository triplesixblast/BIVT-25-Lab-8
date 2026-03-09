namespace Lab8.Purple
{
    public class Task2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;
            private int _target;
            private bool _hasJump;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks => (int[])_marks.Clone();
            public int Result
            {
                get
                {
                    if (!_hasJump) return 0;
                    int stylePoints = _marks.Sum() - _marks.Min() - _marks.Max();
                    int distancePoints = 60 + (_distance - _target) * 2;
                    return stylePoints + distancePoints;

                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
                _hasJump = false;
                _target = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || marks.Length != 5 || _hasJump) return;
                foreach (var mark in marks)
                {
                    if (mark < 0 || mark > 20) return;
                }
                _distance = distance;
                _target = target;
                Array.Copy(marks, _marks, marks.Length);
                _hasJump = true;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                var sorted = array.OrderByDescending(p => p.Result).ToArray();
                Array.Copy(sorted, array, array.Length);
            }

            public void Print()
            {
                System.Console.WriteLine($"{_name} {_surname} {string.Join(" ", _marks)} {_distance}");
            }

        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;
            private int _jumpIndex;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants => (Participant[])_participants.Clone();

            public SkiJumping(string name, int standard)
            {
                _name = name;
                _standard = standard;
                _participants = Array.Empty<Participant>();
                _jumpIndex = 0;
            }

            public void Add(Participant participant)
            {
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[^1] = participant;
            }
            public void Add(Participant[] participants)
            {
                foreach (var participant in participants)
                    Add(participant);
            }

            public void Jump(int distance, int[] marks)
            {
                if (_jumpIndex >= _participants.Length) return;
                _participants[_jumpIndex++].Jump(distance, marks, _standard);
            }

            public void Print()
            {
                System.Console.WriteLine($"Name: {_name} Standard{_standard}");
            }
        }


        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping(): base("100m", 100) {}
        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping(): base("150m", 150) {}
        }
    }
}