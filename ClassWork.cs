namespace ClassWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Profession profession = new Profession("teacher", 100);
            Teacher teacher = new Teacher("math teacher", 200, "maths", false);
            Profession profession1 = new Teacher("physics teacher", 150, "physics", true);

            Profession[] professions = new Profession[] { teacher, profession1};
            // for (int i = 0; i < professions.Length; i++)
            // {
            //     if (professions[i] is Teacher)
            //     {
            //         Teacher t = professions[i] as Teacher;
            //         System.Console.WriteLine(t.Subject);
            //     }
            // }
            for (int i = 0; i < professions.Length; i++)
            {
                Teacher t = professions[i] as Teacher;
                if (t != null)
                {
                    System.Console.WriteLine(t.Subject);
                }
            }
        }
    }

    public abstract class Profession
    {
        protected string _name;
        protected int _salary;

        public virtual string Name => _name;
        public abstract string LALA { get; }

        public Profession(string name, int salary)
        {
            _name = name;
            _salary = salary;
        }

        public virtual void Greeting()
        {
            System.Console.WriteLine($"hello {_name}");
        }

        public void Print()
        {
            System.Console.WriteLine($"Profession: {_name}, Salary: {_salary}");
        }

        public abstract void Bye();
    }

    public class Teacher : Profession
    {
        private string _subject;
        private bool _isHomeroomTeacher;

        public string Subject => _subject;
        public override string LALA => "";

        public Teacher(string name, int number, string subject) : base(name, number)
        {
            _subject = subject;
            _isHomeroomTeacher = false;
        }

        public Teacher(string name, int number, string position, bool isClassroomTeacher) : this(name, number, position)
        {
            _isHomeroomTeacher = isClassroomTeacher;
        }

        public override void Greeting()
        {
            System.Console.WriteLine($"{_name}");
        }

        public new void Print()
        {
            System.Console.WriteLine($"Teacher: {_name}, {_salary}, {_subject}");
        }

        public override void Bye()
        {
            System.Console.WriteLine("Bye!");
        }
    }
}
