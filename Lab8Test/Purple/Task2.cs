using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab8Test.Purple
{
   [TestClass]
   public sealed class Task2
   {
       record InputRow(string Name, string Surname, int Distance, int[] Marks);
       record OutputRow(string Name, string Surname, int Result);

       private InputRow[] _input;
       private OutputRow[] _outputPro;
       private OutputRow[] _outputJunior;
       private OutputRow[] _outputProSorted;
       private OutputRow[] _outputJuniorSorted;

       private Lab8.Purple.Task2.Participant[] _proParticipants;
       private Lab8.Purple.Task2.Participant[] _juniorParticipants;
       private Lab8.Purple.Task2.SkiJumping _proSkiJumping;
       private Lab8.Purple.Task2.SkiJumping _juniorSkiJumping;

       [TestInitialize]
       public void LoadData()
       {
           var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
           folder = Path.Combine(folder, "Lab8Test", "Purple");

           var inputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "input.json")))!;
           var outputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "output.json")))!;

           _input = inputJson.GetProperty("Task2").Deserialize<InputRow[]>()!;

           _outputPro = outputJson.GetProperty("Task2Pro").Deserialize<OutputRow[]>()!;
           _outputJunior = outputJson.GetProperty("Task2Junior").Deserialize<OutputRow[]>()!;
           _outputProSorted = outputJson.GetProperty("Task2ProSorted").Deserialize<OutputRow[]>()!;
           _outputJuniorSorted = outputJson.GetProperty("Task2JuniorSorted").Deserialize<OutputRow[]>()!;
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab8.Purple.Task2.Participant);
           Assert.IsTrue(type.IsValueType, "Participant должен быть структурой");
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Distance")?.CanRead ?? false, "Нет свойства Distance");
           Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
           Assert.IsTrue(type.GetProperty("Result")?.CanRead ?? false, "Нет свойства Result");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Distance")?.CanWrite ?? false, "Свойство Distance должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Result")?.CanWrite ?? false, "Свойство Result должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
           Assert.IsNotNull(type.GetMethod("Jump", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(int[]), typeof(int) }, null), "Нет публичного метода Jump(int distance, int[] marks, int target)");
           Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task2.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
           Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(5, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(12, type.GetMethods().Count(f => f.IsPublic));

           var sjType = typeof(Lab8.Purple.Task2.SkiJumping);
           Assert.IsTrue(sjType.IsAbstract, "SkiJumping должен быть абстрактным классом");
           Assert.IsTrue(sjType.IsClass);
           Assert.AreEqual(0, sjType.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(sjType.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(sjType.GetProperty("Standard")?.CanRead ?? false, "Нет свойства Standard");
           Assert.IsTrue(sjType.GetProperty("Participants")?.CanRead ?? false, "Нет свойства Participants");
           Assert.IsFalse(sjType.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(sjType.GetProperty("Standard")?.CanWrite ?? false, "Свойство Standard должно быть только для чтения");
           Assert.IsFalse(sjType.GetProperty("Participants")?.CanWrite ?? false, "Свойство Participants должно быть только для чтения");
           Assert.IsNotNull(sjType.GetMethod("Add", new[] { typeof(Lab8.Purple.Task2.Participant) }), "Нет публичного метода Add(Participant jumper)");
           Assert.IsNotNull(sjType.GetMethod("Add", new[] { typeof(Lab8.Purple.Task2.Participant[]) }), "Нет публичного метода Add(Participant[] jumpers)");
           Assert.IsNotNull(sjType.GetMethod("Jump", new[] { typeof(int), typeof(int[]) }), "Нет публичного метода Jump(int distance, int[] marks)");
           Assert.IsNotNull(sjType.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(3, sjType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, sjType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(11, sjType.GetMethods().Count(f => f.IsPublic));

           var juniorType = typeof(Lab8.Purple.Task2.JuniorSkiJumping);
           Assert.IsTrue(juniorType.IsClass);
           Assert.AreEqual(sjType, juniorType.BaseType);
           Assert.IsNotNull(juniorType.GetConstructor(Type.EmptyTypes));
           Assert.AreEqual(0, juniorType.GetFields().Count(f => f.IsPublic));
           Assert.AreEqual(3, juniorType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, juniorType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(11, juniorType.GetMethods().Count(f => f.IsPublic));

           var proType = typeof(Lab8.Purple.Task2.ProSkiJumping);
           Assert.IsTrue(proType.IsClass);
           Assert.AreEqual(sjType, proType.BaseType);
           Assert.IsNotNull(proType.GetConstructor(Type.EmptyTypes));
           Assert.AreEqual(0, proType.GetFields().Count(f => f.IsPublic));
           Assert.AreEqual(3, proType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, proType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(11, proType.GetMethods().Count(f => f.IsPublic));
       }

       [TestMethod]
       public void Test_01_CreatePro()
       {
           InitPro();
           CheckStructPro(jumpExpected: false);
       }

       [TestMethod]
       public void Test_01_CreateJunior()
       {
           InitJunior();
           CheckStructJunior(jumpExpected: false);
       }

       [TestMethod]
       public void Test_02_InitPro()
       {
           InitPro();
           CheckStructPro(jumpExpected: false);
       }

       [TestMethod]
       public void Test_02_InitJunior()
       {
           InitJunior();
           CheckStructJunior(jumpExpected: false);
       }

       [TestMethod]
       public void Test_03_JumpPro()
       {
           InitPro();
           JumpPro();
           CheckStructPro(jumpExpected: true);
       }

       [TestMethod]
       public void Test_03_JumpJunior()
       {
           InitJunior();
           JumpJunior();
           CheckStructJunior(jumpExpected: true);
       }

       [TestMethod]
       public void Test_04_SortPro()
       {
           InitPro();
           JumpPro();
           Lab8.Purple.Task2.Participant.Sort(_proSkiJumping.Participants);
           Assert.AreEqual(_outputProSorted.Length, _proSkiJumping.Participants.Length);
           for (int i = 0; i < _outputProSorted.Length; i++)
           {
               Assert.AreEqual(_outputProSorted[i].Name, _proSkiJumping.Participants[i].Name);
               Assert.AreEqual(_outputProSorted[i].Surname, _proSkiJumping.Participants[i].Surname);
               Assert.AreEqual(_outputProSorted[i].Result, _proSkiJumping.Participants[i].Result);
           }
       }

       [TestMethod]
       public void Test_04_SortJunior()
       {
           InitJunior();
           JumpJunior();
           Lab8.Purple.Task2.Participant.Sort(_juniorSkiJumping.Participants);
           Assert.AreEqual(_outputJuniorSorted.Length, _juniorSkiJumping.Participants.Length);
           for (int i = 0; i < _outputJuniorSorted.Length; i++)
           {
               Assert.AreEqual(_outputJuniorSorted[i].Name, _juniorSkiJumping.Participants[i].Name);
               Assert.AreEqual(_outputJuniorSorted[i].Surname, _juniorSkiJumping.Participants[i].Surname);
               Assert.AreEqual(_outputJuniorSorted[i].Result, _juniorSkiJumping.Participants[i].Result);
           }
       }

       [TestMethod]
       public void Test_05_ArrayLinqPro()
       {
           InitPro();
           JumpPro();
           ArrayLinqPro();
           CheckStructPro(jumpExpected: true);
       }

       [TestMethod]
       public void Test_05_ArrayLinqJunior()
       {
           InitJunior();
           JumpJunior();
           ArrayLinqJunior();
           CheckStructJunior(jumpExpected: true);
       }

       [TestMethod]
       public void Test_06_Subclasses()
       {
           var junior = new Lab8.Purple.Task2.JuniorSkiJumping();
           Assert.AreEqual("100m", junior.Name);
           Assert.AreEqual(100, junior.Standard);
           CollectionAssert.AreEqual(new Lab8.Purple.Task2.Participant[0], junior.Participants);

           var pro = new Lab8.Purple.Task2.ProSkiJumping();
           Assert.AreEqual("150m", pro.Name);
           Assert.AreEqual(150, pro.Standard);
           CollectionAssert.AreEqual(new Lab8.Purple.Task2.Participant[0], pro.Participants);
       }

       private void InitPro()
       {
           _proParticipants = new Lab8.Purple.Task2.Participant[5];
           for (int i = 0; i < 5; i++)
               _proParticipants[i] = new Lab8.Purple.Task2.Participant(_input[i].Name, _input[i].Surname);
       }

       private void InitJunior()
       {
           _juniorParticipants = new Lab8.Purple.Task2.Participant[5];
           for (int i = 0; i < 5; i++)
               _juniorParticipants[i] = new Lab8.Purple.Task2.Participant(_input[i+5].Name, _input[i+5].Surname);
       }

       private void JumpPro()
       {
           _proSkiJumping = new Lab8.Purple.Task2.ProSkiJumping();
           _proSkiJumping.Add(_proParticipants);
           for (int i = 0; i < 5; i++)
               _proSkiJumping.Jump(_input[i].Distance, _input[i].Marks);
       }

       private void JumpJunior()
       {
           _juniorSkiJumping = new Lab8.Purple.Task2.JuniorSkiJumping();
           _juniorSkiJumping.Add(_juniorParticipants);
           for (int i = 0; i < 5; i++)
               _juniorSkiJumping.Jump(_input[i+5].Distance, _input[i+5].Marks);
       }

       private void ArrayLinqPro()
       {
           for (int i = 0; i < _proParticipants.Length; i++)
           {
               var marks = _proParticipants[i].Marks;
               if (marks == null) continue;

               for (int j = 0; j < marks.Length; j++)
                   marks[j] = -1;
           }
       }

       private void ArrayLinqJunior()
       {
           for (int i = 0; i < _juniorParticipants.Length; i++)
           {
               var marks = _juniorParticipants[i].Marks;
               if (marks == null) continue;

               for (int j = 0; j < marks.Length; j++)
                   marks[j] = -1;
           }
       }

       private void CheckStructPro(bool jumpExpected)
       {
           Assert.AreEqual(5, _proParticipants.Length);

           for (int i = 0; i < 5; i++)
           {
               Assert.AreEqual(_input[i].Name, _proParticipants[i].Name);
               Assert.AreEqual(_input[i].Surname, _proParticipants[i].Surname);

               if (jumpExpected)
               {
                   Assert.AreEqual(_input[i].Distance, _proSkiJumping.Participants[i].Distance);

                   var marks = _proSkiJumping.Participants[i].Marks;
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(5, marks.Length);
                   for (int j = 0; j < 5; j++)
                       Assert.AreEqual(_input[i].Marks[j], marks[j]);

                   Assert.AreEqual(_outputPro[i].Result, _proSkiJumping.Participants[i].Result);
               }
               else
               {
                   Assert.AreEqual(0, _proParticipants[i].Distance);

                   var marks = _proParticipants[i].Marks;
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(5, marks.Length);
                   for (int j = 0; j < 5; j++)
                       Assert.AreEqual(0, marks[j]);

                   Assert.AreEqual(0, _proParticipants[i].Result);
               }
           }
       }

       private void CheckStructJunior(bool jumpExpected)
       {
           Assert.AreEqual(5, _juniorParticipants.Length);

           for (int i = 0; i < 5; i++)
           {
               Assert.AreEqual(_input[i + 5].Name, _juniorParticipants[i].Name);
               Assert.AreEqual(_input[i + 5].Surname, _juniorParticipants[i].Surname);

               if (jumpExpected)
               {
                   Assert.AreEqual(_input[i + 5].Distance, _juniorSkiJumping.Participants[i].Distance);

                   var marks = _juniorSkiJumping.Participants[i].Marks;
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(5, marks.Length);
                   for (int j = 0; j < 5; j++)
                       Assert.AreEqual(_input[i + 5].Marks[j], marks[j]);

                   Assert.AreEqual(_outputJunior[i].Result, _juniorSkiJumping.Participants[i].Result);
               }
               else
               {
                   Assert.AreEqual(0, _juniorParticipants[i].Distance);

                   var marks = _juniorParticipants[i].Marks;
                   Assert.IsNotNull(marks);
                   Assert.AreEqual(5, marks.Length);
                   for (int j = 0; j < 5; j++)
                       Assert.AreEqual(0, marks[j]);

                   Assert.AreEqual(0, _juniorParticipants[i].Result);
               }
           }
       }
   }
}