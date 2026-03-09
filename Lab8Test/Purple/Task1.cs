using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab8Test.Purple
{
   [TestClass]
   public sealed class Task1
   {
       record TaskInput(InputRow[] Participants, JudgeRow[] Judges);
       record JudgeRow(string Name, int[] Marks);
       record InputRow(string Name, string Surname, double[] Coefs, int[][] Marks);
       record OutputRow(string Name, string Surname, double TotalScore);

       private TaskInput _input;
       private InputRow[] _inputRow;
       private OutputRow[] _output;
       private OutputRow[] _outputSorted;
       private Lab8.Purple.Task1.Participant[] _participants;
       private Lab8.Purple.Task1.Judge[] _judges;
       private Lab8.Purple.Task1.Competition _competition;


       [TestInitialize]
       public void LoadData()
       {
           var folder = Directory.GetParent(Directory.GetCurrentDirectory())
               .Parent.Parent.Parent.FullName;

           folder = Path.Combine(folder, "Lab8Test", "Purple");

           var input = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "input.json")))!;

           var output = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "output.json")))!;

           _input = input.GetProperty("Task1").Deserialize<TaskInput>()!;
           _output = output.GetProperty("Task1").Deserialize<OutputRow[]>()!;
           _outputSorted = output.GetProperty("Task1Sorted").Deserialize<OutputRow[]>()!;

           _judges = new Lab8.Purple.Task1.Judge[_input.Judges.Length];

           for (int i = 0; i < _input.Judges.Length; i++)
           {
               _judges[i] = new Lab8.Purple.Task1.Judge(
                   _input.Judges[i].Name,
                   _input.Judges[i].Marks
               );
           }
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab8.Purple.Task1.Participant);
           Assert.IsFalse(type.IsValueType, "Participant должен быть классом");
           Assert.IsTrue(type.IsClass);
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Coefs")?.CanRead ?? false, "Нет свойства Coefs");
           Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
           Assert.IsTrue(type.GetProperty("TotalScore")?.CanRead ?? false, "Нет свойства TotalScore");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Coefs")?.CanWrite ?? false, "Свойство Coefs должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("TotalScore")?.CanWrite ?? false, "Свойство TotalScore должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
           Assert.IsNotNull(type.GetMethod("SetCriterias", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double[]) }, null), "Нет публичного метода SetCriterias(double[] coefs)");
           Assert.IsNotNull(type.GetMethod("Jump", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int[]) }, null), "Нет публичного метода Jump(int[] marks)");
           Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task1.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
           Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(5, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(13, type.GetMethods().Count(f => f.IsPublic));

           var judgeType = typeof(Lab8.Purple.Task1.Judge);
           Assert.IsTrue(judgeType.IsClass, "Judge должен быть классом");
           Assert.AreEqual(0, judgeType.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(judgeType.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsFalse(judgeType.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsNotNull(judgeType.GetConstructor(new[] { typeof(string), typeof(int[]) }), "Нет публичного конструктора Judge(string name, int[] marks)");
           Assert.IsNotNull(judgeType.GetMethod("CreateMark", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода CreateMark()");
           Assert.IsNotNull(judgeType.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(1, judgeType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, judgeType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(7, judgeType.GetMethods().Count(f => f.IsPublic));

           var compType = typeof(Lab8.Purple.Task1.Competition);
           Assert.IsTrue(compType.IsClass, "Competition должен быть классом");
           Assert.AreEqual(0, compType.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(compType.GetProperty("Judges")?.CanRead ?? false, "Нет свойства Judges");
           Assert.IsTrue(compType.GetProperty("Participants")?.CanRead ?? false, "Нет свойства Participants");
           Assert.IsFalse(compType.GetProperty("Judges")?.CanWrite ?? false, "Свойство Judges должно быть только для чтения");
           Assert.IsFalse(compType.GetProperty("Participants")?.CanWrite ?? false, "Свойство Participants должно быть только для чтения");
           Assert.IsNotNull(compType.GetConstructor(new[] { typeof(Lab8.Purple.Task1.Judge[]) }), "Нет публичного конструктора Competition(Judge[] judges)");
           Assert.IsNotNull(compType.GetMethod("Add", new[] { typeof(Lab8.Purple.Task1.Participant) }), "Нет публичного метода Add(Participant jumper)");
           Assert.IsNotNull(compType.GetMethod("Add", new[] { typeof(Lab8.Purple.Task1.Participant[]) }), "Нет публичного метода Add(Participant[] jumpers)");
           Assert.IsNotNull(compType.GetMethod("Evaluate", new[] { typeof(Lab8.Purple.Task1.Participant) }), "Нет публичного метода Evaluate(Participant jumper)");
           Assert.IsNotNull(compType.GetMethod("Sort", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Sort()");
           Assert.AreEqual(2, compType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, compType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(10, compType.GetMethods().Count(f => f.IsPublic));
       }

       [TestMethod]
       public void Test_01_Create()
       {
           Init();
           CheckStruct(false, false);
       }

       [TestMethod]
       public void Test_02_Coefs()
       {
           Init();
           SetCriterias();
           CheckStruct(true, false);
       }

       [TestMethod]
       public void Test_03_Jumps()
       {
           Init();
           SetCriterias();
           Jump();
           CheckStruct(true, true);
       }

       [TestMethod]
       public void Test_04_Sort()
       {
           Init();
           SetCriterias();
           Jump();

           _competition.Sort();

           for (int i = 0; i < _outputSorted.Length; i++)
           {
               Assert.AreEqual(_outputSorted[i].Name, _competition.Participants[i].Name);
               Assert.AreEqual(_outputSorted[i].Surname, _competition.Participants[i].Surname);
               Assert.AreEqual(_outputSorted[i].TotalScore,
                   _competition.Participants[i].TotalScore, 0.01);
           }
       }

       [TestMethod]
       public void Test_05_Judge()
       {
           int[] marks = { 1, 2, 3 };
           var judge = new Lab8.Purple.Task1.Judge("Judge", marks);

           Assert.AreEqual(1, judge.CreateMark());
           Assert.AreEqual(2, judge.CreateMark());
           Assert.AreEqual(3, judge.CreateMark());
           Assert.AreEqual(1, judge.CreateMark());
       }

       private void Init()
       {
           _participants = new Lab8.Purple.Task1.Participant[_input.Participants.Length];

           for (int i = 0; i < _input.Participants.Length; i++)
           {
               _participants[i] = new Lab8.Purple.Task1.Participant(
                   _input.Participants[i].Name,
                   _input.Participants[i].Surname
               );
           }
       }

       private void SetCriterias()
       {
           for (int i = 0; i < _input.Participants.Length; i++)
           {
               _participants[i].SetCriterias(_input.Participants[i].Coefs);
           }
       }

       private void Jump()
       {
           _competition = new Lab8.Purple.Task1.Competition(_judges);

           foreach (var p in _participants)
               _competition.Add(p);

           // ещё 3 прыжка (1 уже сделан в Add)
           for (int r = 1; r < 4; r++)
           {
               foreach (var p in _competition.Participants)
                   _competition.Evaluate(p);
           }
       }

       private void CheckStruct(bool coefsExpected, bool jumpsExpected)
       {
           Assert.AreEqual(_input.Participants.Length, _participants.Length);

           for (int i = 0; i < _participants.Length; i++)
           {
               var input = _input.Participants[i];
               var p = _participants[i];

               Assert.AreEqual(input.Name, p.Name);
               Assert.AreEqual(input.Surname, p.Surname);

               var coefs = p.Coefs;
               Assert.AreEqual(4, coefs.Length);

               if (coefsExpected)
               {
                   for (int c = 0; c < 4; c++)
                       Assert.AreEqual(input.Coefs[c], coefs[c], 0.0001);
               }

               var marks = p.Marks;
               Assert.AreEqual(4, marks.GetLength(0));
               Assert.AreEqual(7, marks.GetLength(1));

               if (jumpsExpected)
               {
                   for (int r = 0; r < 4; r++)
                       for (int c = 0; c < 7; c++)
                           Assert.AreEqual(input.Marks[r][c], marks[r, c], message: $"{input.Name} at index {r},{c}");

                   Assert.AreEqual(_output[i].TotalScore, p.TotalScore, 0.01);
               }
           }
       }
   }
}