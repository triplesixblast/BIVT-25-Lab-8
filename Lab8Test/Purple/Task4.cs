using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab8Test.Purple
{
   [TestClass]
   public sealed class Task4
   {
       record InputRow(string Name, string Surname, double Time);
       record OutputRow(string Name, string Surname, double Time);

       private InputRow[][] _inputGroups;
       private OutputRow[] _output;
       private OutputRow[] _outputShufled;

       private Lab8.Purple.Task4.Sportsman[][] _sportsmanGroups;
       private Lab8.Purple.Task4.Group[] _group;

       [TestInitialize]
       public void LoadData()
       {
           var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
           folder = Path.Combine(folder, "Lab8Test", "Purple");

           var inputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "input.json"))
           )!;
           var outputJson = JsonSerializer.Deserialize<JsonElement>(
               File.ReadAllText(Path.Combine(folder, "output.json"))
           )!;

           var task4Input = inputJson.GetProperty("Task4");

           int groupCount = task4Input.EnumerateObject().Count();
           _inputGroups = new InputRow[groupCount][];
           int g = 0;
           foreach (var groupProp in task4Input.EnumerateObject())
           {
               _inputGroups[g] = groupProp.Value.Deserialize<InputRow[]>()!;
               g++;
           }

           _output = outputJson.GetProperty("Task4").GetProperty("Финалисты").Deserialize<OutputRow[]>()!;
           _outputShufled = outputJson.GetProperty("Task4").GetProperty("Shufled").Deserialize<OutputRow[]>()!;

           _sportsmanGroups = new Lab8.Purple.Task4.Sportsman[_inputGroups.Length][];
           _group = new Lab8.Purple.Task4.Group[_inputGroups.Length];
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab8.Purple.Task4.Sportsman);
           Assert.IsFalse(type.IsValueType, "Sportsman должен быть классом");
           Assert.IsTrue(type.IsClass);
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
           Assert.IsTrue(type.GetProperty("Time")?.CanRead ?? false, "Нет свойства Time");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Time")?.CanWrite ?? false, "Свойство Time должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(new[] { typeof(string), typeof(string) }), "Нет публичного конструктора Sportsman(string name, string surname)");
           Assert.IsNotNull(type.GetMethod("Run", new[] { typeof(double) }), "Нет публичного метода Run(double time)");
           Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task4.Sportsman[]) }, null), "Нет публичного статического метода Sort(Sportsman[] array)");
           Assert.IsNotNull(type.GetMethod("Print"), "Нет публичного метода Print()");
           Assert.AreEqual(3, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(10, type.GetMethods().Count(f => f.IsPublic));

           var manType = typeof(Lab8.Purple.Task4.SkiMan);
           Assert.IsTrue(manType.IsClass);
           Assert.AreEqual(type, manType.BaseType);
           Assert.IsNotNull(manType.GetConstructor(new[] { typeof(string), typeof(string) }));
           Assert.IsNotNull(manType.GetConstructor(new[] { typeof(string), typeof(string), typeof(double) }), "Нет публичного конструктора SkiMan(string name, string surname, double time)");
           Assert.AreEqual(0, manType.GetFields().Count(f => f.IsPublic));
           Assert.AreEqual(3, manType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(2, manType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(9, manType.GetMethods().Count(f => f.IsPublic));

           var womanType = typeof(Lab8.Purple.Task4.SkiWoman);
           Assert.IsTrue(womanType.IsClass);
           Assert.AreEqual(type, womanType.BaseType);
           Assert.IsNotNull(womanType.GetConstructor(new[] { typeof(string), typeof(string) }));
           Assert.IsNotNull(womanType.GetConstructor(new[] { typeof(string), typeof(string), typeof(double) }), "Нет публичного конструктора SkiWoman(string name, string surname, double time)");
           Assert.AreEqual(0, womanType.GetFields().Count(f => f.IsPublic));
           Assert.AreEqual(3, womanType.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(2, womanType.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(9, womanType.GetMethods().Count(f => f.IsPublic));

           type = typeof(Lab8.Purple.Task4.Group);
           Assert.IsFalse(type.IsValueType, "Group должен быть классом");
           Assert.IsTrue(type.IsClass);
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Sportsmen")?.CanRead ?? false, "Нет свойства Sportsmen");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Sportsmen")?.CanWrite ?? false, "Свойство Sportsmen должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(new[] { typeof(string) }), "Нет публичного конструктора Group(string name)");
           Assert.IsNotNull(type.GetConstructor(new[] { typeof(Lab8.Purple.Task4.Group) }), "Нет публичного конструктора Group(Group group)");
           Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab8.Purple.Task4.Sportsman) }), "Нет публичного метода Add(Sportsman elem)");
           Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab8.Purple.Task4.Sportsman[]) }), "Нет публичного метода Add(Sportsman[] array)");
           Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab8.Purple.Task4.Group) }), "Нет публичного метода Add(Group group)");
           Assert.IsNotNull(type.GetMethod("Sort"), "Нет публичного метода Sort()");
           Assert.IsNotNull(type.GetMethod("Merge", BindingFlags.Static | BindingFlags.Public), "Нет публичного статического метода Merge(Group group1, Group group2)");
           Assert.IsNotNull(type.GetMethod("Split"), "Нет публичного метода Split(out Sportsman[] men, out Sportsman[] women)");
           Assert.IsNotNull(type.GetMethod("Shuffle"), "Нет публичного метода Shuffle()");
           Assert.AreEqual(2, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(2, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(14, type.GetMethods().Count(f => f.IsPublic));
       }

       [TestMethod]
       public void Test_01_InitSportsmen()
       {
           InitSportsmen();
           InitMen();
           InitWomen();
           CheckSportsmen(hasRun: false);
       }

       [TestMethod]
       public void Test_02_Run()
       {
           InitMen();
           InitWomen();
           RunSportsmen();
           CheckSportsmen(hasRun: true);
       }

       [TestMethod]
       public void Test_03_InitGroups()
       {
           InitMen();
           InitWomen();
           InitGroups();
           CheckGroups();
       }

       [TestMethod]
       public void Test_04_AddOne()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           CheckGroups(filled: true);
       }

       [TestMethod]
       public void Test_05_AddSeveral()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddSeveral();
           CheckGroups(filled: true);
       }

       [TestMethod]
       public void Test_06_SortGroups()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           //AddSeveral();
           SortGroups();
           CheckGroups(filled: true, sorted: true);
       }

       [TestMethod]
       public void Test_07_MergeGroups()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           //AddSeveral();
           SortGroups();
           var merged = Lab8.Purple.Task4.Group.Merge(_group[0], _group[1]);
           var allSportsmen = merged.Sportsmen;
           Assert.AreEqual("Финалисты", merged.Name);
           Assert.AreEqual(_output.Length, allSportsmen.Length);
           for (int i = 0; i < _output.Length; i++)
           {
               Assert.AreEqual(_output[i].Name, allSportsmen[i].Name);
               Assert.AreEqual(_output[i].Surname, allSportsmen[i].Surname);
               Assert.AreEqual(_output[i].Time, allSportsmen[i].Time, 0.0001);
           }
       }

       [TestMethod]
       public void Test_08_AddGroup()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           var extraGroup = new Lab8.Purple.Task4.Group(_group[0]);
           var sportsmen = extraGroup.Sportsmen;
           Assert.IsNotNull(sportsmen);
           Assert.AreEqual(_inputGroups[0].Length, sportsmen.Length);
           for (int i = 0; i < _inputGroups[0].Length; i++)
           {
               Assert.AreEqual(_inputGroups[0][i].Name, sportsmen[i].Name);
               Assert.AreEqual(_inputGroups[0][i].Surname, sportsmen[i].Surname);
               Assert.AreEqual(_inputGroups[0][i].Time, sportsmen[i].Time);
           }
       }

       [TestMethod]
       public void Test_09_ArrayLinq()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           //AddSeveral();
           ArrayLinq();
           CheckGroups(filled: true, shifted: true);
       }

       [TestMethod]
       public void Test_10_Split()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddOne();
           AddSeveral();
           foreach (var grp in _group)
           {
               grp.Split(out var men, out var women);
               Assert.AreEqual(grp.Sportsmen.Count(s => s is Lab8.Purple.Task4.SkiMan), men.Length);
               Assert.AreEqual(grp.Sportsmen.Count(s => s is Lab8.Purple.Task4.SkiWoman), women.Length);
               foreach (var m in men)
                   Assert.IsTrue(m is Lab8.Purple.Task4.SkiMan);
               foreach (var w in women)
                   Assert.IsTrue(w is Lab8.Purple.Task4.SkiWoman);
           }
       }

       [TestMethod]
       public void Test_11_Shuffle()
       {
           InitMen();
           InitWomen();
           InitGroups();
           RunSportsmen();
           AddBoth();
           //AddSeveral();
           _group[0].Shuffle();
           for (int i = 0; i < _group[0].Sportsmen.Length; i++)
           {
               Assert.AreEqual(_outputShufled[i].Name, _group[0].Sportsmen[i].Name);
               Assert.AreEqual(_outputShufled[i].Surname, _group[0].Sportsmen[i].Surname);
               Assert.AreEqual(_outputShufled[i].Time, _group[0].Sportsmen[i].Time);
           }
       }

       [TestMethod]
       public void Test_12_SecondConstructor()
       {
           var man = new Lab8.Purple.Task4.SkiMan("Test", "Man", 100.0);
           Assert.AreEqual(100.0, man.Time, 0.0001);

           var woman = new Lab8.Purple.Task4.SkiWoman("Test", "Woman", 200.0);
           Assert.AreEqual(200.0, woman.Time, 0.0001);
       }

       private void InitSportsmen()
       {
           for (int g = 0; g < _inputGroups.Length; g++)
           {
               _sportsmanGroups[g] = new Lab8.Purple.Task4.Sportsman[_inputGroups[g].Length];
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i] = new Lab8.Purple.Task4.Sportsman(_inputGroups[g][i].Name, _inputGroups[g][i].Surname);
               }
           }
       }
       private void InitMen()
       {
           for (int g = 0; g < _inputGroups.Length; g += 2)
           {
               _sportsmanGroups[g] = new Lab8.Purple.Task4.Sportsman[_inputGroups[g].Length];
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i] = new Lab8.Purple.Task4.SkiMan(_inputGroups[g][i].Name, _inputGroups[g][i].Surname);
               }
           }
       }
       private void InitWomen()
       {
           for (int g = 1; g < _inputGroups.Length; g += 2)
           {
               _sportsmanGroups[g] = new Lab8.Purple.Task4.Sportsman[_inputGroups[g].Length];
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i] = new Lab8.Purple.Task4.SkiWoman(_inputGroups[g][i].Name, _inputGroups[g][i].Surname);
               }
           }
       }

       private void RunSportsmen()
       {
           for (int g = 0; g < _inputGroups.Length; g++)
           {
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   _sportsmanGroups[g][i].Run(_inputGroups[g][i].Time);
               }
           }
       }

       private void InitGroups()
       {
           for (int g = 0; g < _group.Length; g++)
               _group[g] = new Lab8.Purple.Task4.Group($"Group{g + 1}");
       }

       private void AddOne()
       {
           for (int g = 0; g < _group.Length; g++)
           {
               for (int i = 0; i < _sportsmanGroups[g].Length; i++)
                   _group[g].Add(_sportsmanGroups[g][i]);
           }
       }

       private void AddSeveral()
       {
           for (int g = 0; g < _group.Length; g++)
               _group[g].Add(_sportsmanGroups[g]);
       }

       private void AddBoth()
       {
           for (int g = 0; g < _group.Length; g++)
           {
               _group[g].Add(_sportsmanGroups[g]);
               _group[g].Add(_sportsmanGroups[(g + 1) % 4]);
           }
       }


       private void SortGroups()
       {
           for (int g = 0; g < _group.Length; g++)
               _group[g].Sort();
       }

       private void ArrayLinq()
       {
           for (int g = 0; g < _group.Length; g++)
           {
               var sportsmen = _group[g].Sportsmen;
               var first = sportsmen[0];
               for (int i = 0; i < sportsmen.Length - 1; i++)
                   sportsmen[i] = sportsmen[i + 1];
               sportsmen[^1] = first;
           }
       }

       private void CheckSportsmen(bool hasRun = false)
       {
           for (int g = 0; g < _sportsmanGroups.Length; g++)
           {
               Assert.AreEqual(_inputGroups[g].Length, _sportsmanGroups[g].Length);
               for (int i = 0; i < _inputGroups[g].Length; i++)
               {
                   Assert.AreEqual(_inputGroups[g][i].Name, _sportsmanGroups[g][i].Name);
                   Assert.AreEqual(_inputGroups[g][i].Surname, _sportsmanGroups[g][i].Surname);
                   if (hasRun)
                       Assert.AreEqual(_inputGroups[g][i].Time, _sportsmanGroups[g][i].Time, 0.0001);
                   else
                       Assert.AreEqual(0.0, _sportsmanGroups[g][i].Time, 0.0001);
               }
           }
       }

       private void CheckGroups(bool filled = false, bool sorted = false, bool shifted = false)
       {
           for (int g = 0; g < _group.Length; g++)
           {
               Assert.AreEqual($"Group{g + 1}", _group[g].Name);
               var sportsmen = _group[g].Sportsmen;
               Assert.IsNotNull(sportsmen);

               if (filled)
               {
                   Assert.AreEqual(_inputGroups[g].Length, sportsmen.Length);
                   if (sorted)
                   {
                       var expected = _inputGroups[g].OrderBy(x => x.Time).ToArray();
                       for (int i = 0; i < _inputGroups[g].Length; i++)
                       {
                           Assert.AreEqual(expected[i].Name, sportsmen[i].Name);
                           Assert.AreEqual(expected[i].Surname, sportsmen[i].Surname);
                           Assert.AreEqual(expected[i].Time, sportsmen[i].Time);
                       }

                   }
                   else
                   {
                       var shift = shifted ? 1 : 0;
                       for (int i = 0; i < _inputGroups[g].Length; i++)
                       {
                           Assert.AreEqual(_inputGroups[g][(i + shift) % sportsmen.Length].Name, sportsmen[i].Name);
                           Assert.AreEqual(_inputGroups[g][(i + shift) % sportsmen.Length].Surname, sportsmen[i].Surname);
                           Assert.AreEqual(_inputGroups[g][(i + shift) % sportsmen.Length].Time, sportsmen[i].Time);
                       }
                   }
               }
               else
                   Assert.AreEqual(0, sportsmen.Length);
           }
       }
   }
}