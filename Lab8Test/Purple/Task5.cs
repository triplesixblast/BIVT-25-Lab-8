using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lab8Test.Purple
{
   [TestClass]
   public sealed class Task5
   {
       record InputResponse(string Animal, string CharacterTrait, string Concept);
       record TopRow(string[] Answers);
       record ReportRow(string Answer, double Percent);

       private InputResponse[] _inputResponses;
       private string[][] _outputTops;
       private ReportRow[][] _outputReports;

       private Lab8.Purple.Task5.Response[] _responses;
       private Lab8.Purple.Task5.Report _report;
       private Lab8.Purple.Task5.Research[] _researchGroups;
       private string[][] _topResponses;
       private (string, double)[][] _generalReports;

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

           _inputResponses = inputJson.GetProperty("Task5").Deserialize<InputResponse[]>()!;

           var task5Output = outputJson.GetProperty("Task5");
           _outputTops = new string[3][];
           _outputReports = new ReportRow[3][];
           _outputTops[0] = task5Output.GetProperty("Top1").Deserialize<string[]>()!;
           _outputTops[1] = task5Output.GetProperty("Top2").Deserialize<string[]>()!;
           _outputTops[2] = task5Output.GetProperty("Top3").Deserialize<string[]>()!;
           _outputReports[0] = task5Output.GetProperty("Report1").Deserialize<ReportRow[]>()!;
           _outputReports[1] = task5Output.GetProperty("Report2").Deserialize<ReportRow[]>()!;
           _outputReports[2] = task5Output.GetProperty("Report3").Deserialize<ReportRow[]>()!;
       }

       [TestMethod]
       public void Test_00_OOP()
       {
           var type = typeof(Lab8.Purple.Task5.Response);
           Assert.IsTrue(type.IsValueType, "Response должен быть структурой");
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Animal")?.CanRead ?? false, "Нет свойства Animal");
           Assert.IsTrue(type.GetProperty("CharacterTrait")?.CanRead ?? false, "Нет свойства CharacterTrait");
           Assert.IsTrue(type.GetProperty("Concept")?.CanRead ?? false, "Нет свойства Concept");
           Assert.IsFalse(type.GetProperty("Animal")?.CanWrite ?? false, "Свойство Animal должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("CharacterTrait")?.CanWrite ?? false, "Свойство CharacterTrait должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Concept")?.CanWrite ?? false, "Свойство Concept должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string), typeof(string) }, null), "Нет публичного конструктора Response(string animal, string characterTrait, string concept)");
           Assert.IsNotNull(type.GetMethod("CountVotes", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task5.Response[]), typeof(int) }, null), "Нет публичного метода CountVotes(Response[] responses, int q)");
           Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(3, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(9, type.GetMethods().Count(f => f.IsPublic));

           type = typeof(Lab8.Purple.Task5.Research);
           Assert.IsTrue(type.IsValueType, "Research должен быть структурой");
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
           Assert.IsTrue(type.GetProperty("Responses")?.CanRead ?? false, "Нет свойства Responses");
           Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
           Assert.IsFalse(type.GetProperty("Responses")?.CanWrite ?? false, "Свойство Responses должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null), "Нет публичного конструктора Research(string name)");
           Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string[]) }, null), "Нет публичного метода Add(string[] answers)");
           Assert.IsNotNull(type.GetMethod("GetTopResponses", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int) }, null), "Нет публичного метода GetTopResponses(int question)");
           Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
           Assert.AreEqual(2, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(9, type.GetMethods().Count(f => f.IsPublic));

           type = typeof(Lab8.Purple.Task5.Report);
           Assert.IsFalse(type.IsValueType, "Report должен быть классом");
           Assert.IsTrue(type.IsClass);
           Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
           Assert.IsTrue(type.GetProperty("Researches")?.CanRead ?? false, "Нет свойства Researches");
           Assert.IsFalse(type.GetProperty("Researches")?.CanWrite ?? false, "Свойство Researches должно быть только для чтения");
           Assert.IsNotNull(type.GetConstructor(Type.EmptyTypes), "Нет публичного конструктора Report()");
           Assert.IsNotNull(type.GetMethod("MakeResearch", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода MakeResearch()");
           Assert.IsNotNull(type.GetMethod("GetGeneralReport", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int) }, null), "Нет публичного метода GetGeneralReport(int question)");
           Assert.AreEqual(1, type.GetProperties().Count(f => f.PropertyType.IsPublic));
           Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
           Assert.AreEqual(7, type.GetMethods().Count(f => f.IsPublic));
       }

       [TestMethod]
       public void Test_01_InitResponses()
       {
           InitResponses();
           CheckResponses();
       }

       [TestMethod]
       public void Test_02_InitReport()
       {
           InitReport();
           CheckReport();
       }

       [TestMethod]
       public void Test_03_MakeResearch()
       {
           InitReport();
           MakeResearch();
           CheckReport(hasResearch: true);
       }

       [TestMethod]
       public void Test_04_AddToResearch()
       {
           ResetStatic();
           InitResponses();
           InitReport();
           MakeResearch();
           AddToResearch();
           CheckReport(hasResearch: true, filled: true);
       }

       [TestMethod]
       public void Test_05_GetTopResponses()
       {
           ResetStatic();
           InitResponses();
           InitReport();
           MakeResearch();
           AddToResearch();
           GetTopResponses();
           CheckTopResponses();
       }

       [TestMethod]
       public void Test_06_GetGeneralReport()
       {
           ResetStatic();
           InitResponses();
           InitReport();
           MakeResearch();
           AddToResearch();
           GetGeneralReport();
           CheckGeneralReport();
       }

       private void InitResponses()
       {
           _responses = _inputResponses.Select(r => new Lab8.Purple.Task5.Response(r.Animal, r.CharacterTrait, r.Concept)).ToArray();
       }

       private void InitReport()
       {
           _report = new Lab8.Purple.Task5.Report();
       }
       private void ResetStatic()
       {
           var type = typeof(Lab8.Purple.Task5.Report);
           var staticFields = type.GetFields(
               BindingFlags.Static |
               BindingFlags.Public |
               BindingFlags.NonPublic);
           foreach (var field in staticFields)
           {
               if (field.FieldType == typeof(int))
                   field.SetValue(null, 1);
               else if (field.FieldType == typeof(double))
                   field.SetValue(null, 0.0);
               else if (field.FieldType == typeof(bool))
                   field.SetValue(null, false);
               else
                   field.SetValue(null, null);
           }
       }
       private void MakeResearch()
       {

           _researchGroups = new Lab8.Purple.Task5.Research[3];
           for (int i = 0; i < 3; i++)
           {
               _researchGroups[i] = _report.MakeResearch();
           }
       }

       private void AddToResearch()
       {
           int chunk = _responses.Length / 3;
           for (int i = 0; i < 3; i++)
           {
               var part = _responses.Skip(i * chunk).Take(chunk).ToArray();
               for (int j = 0; j < part.Length; j++)
               {
                   _researchGroups[i].Add(new string[] { part[j].Animal, part[j].CharacterTrait, part[j].Concept });
                   _report.Researches[i].Add(new string[] { part[j].Animal, part[j].CharacterTrait, part[j].Concept });
               }
           }
       }

       private void GetTopResponses()
       {
           _topResponses = new string[3][];
           for (int i = 0; i < 3; i++)
           {
               _topResponses[i] = _researchGroups[i].GetTopResponses(i + 1);
               Console.WriteLine(string.Join(", ", _topResponses[i]));
           }
       }

       private void GetGeneralReport()
       {
           _generalReports = new (string, double)[3][];
           for (int q = 1; q <= 3; q++)
           {
               _generalReports[q - 1] = _report.GetGeneralReport(q);
           }
       }

       private void CheckResponses()
       {
           Assert.AreEqual(_inputResponses.Length, _responses.Length);
           for (int i = 0; i < _responses.Length; i++)
           {
               Assert.AreEqual(_inputResponses[i].Animal, _responses[i].Animal);
               Assert.AreEqual(_inputResponses[i].CharacterTrait, _responses[i].CharacterTrait);
               Assert.AreEqual(_inputResponses[i].Concept, _responses[i].Concept);
           }
       }

       private void CheckReport(bool hasResearch = false, bool filled = false)
       {
           Assert.IsNotNull(_report.Researches);
           if (hasResearch)
           {
               Assert.AreEqual(3, _report.Researches.Length);
               for (int i = 0; i < 3; i++)
               {
                   Assert.IsTrue(_report.Researches[i].Name.StartsWith($"No_{i + 1}_"));
                   Assert.IsNotNull(_report.Researches[i].Responses);
                   if (filled)
                   {
                       Assert.AreEqual(_responses.Length / 3, _report.Researches[i].Responses.Length);
                       int chunk = _responses.Length / 3;
                       var expectedPart = _responses.Skip(i * chunk).Take(chunk).ToArray();
                       for (int j = 0; j < expectedPart.Length; j++)
                       {
                           Assert.AreEqual(expectedPart[j].Animal, _report.Researches[i].Responses[j].Animal);
                           Assert.AreEqual(expectedPart[j].CharacterTrait, _report.Researches[i].Responses[j].CharacterTrait);
                           Assert.AreEqual(expectedPart[j].Concept, _report.Researches[i].Responses[j].Concept);
                       }
                   }
                   else
                       Assert.AreEqual(0, _report.Researches[i].Responses.Length);
               }
           }
           else
           {
               Assert.AreEqual(0, _report.Researches.Length);
           }
       }

       private void CheckTopResponses()
       {
           for (int i = 0; i < 3; i++)
           {
               CollectionAssert.AreEqual(_outputTops[i], _topResponses[i]);
           }
       }

       private void CheckGeneralReport()
       {
           for (int q = 0; q < 3; q++)
           {
               Assert.AreEqual(_outputReports[q].Length, _generalReports[q].Length);
               for (int j = 0; j < _generalReports[q].Length; j++)
               {
                   Assert.AreEqual(_outputReports[q][j].Answer, _generalReports[q][j].Item1);
                   Assert.AreEqual(_outputReports[q][j].Percent, _generalReports[q][j].Item2, 0.0001);
               }
           }
       }
   }
}