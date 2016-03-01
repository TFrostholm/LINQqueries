using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LINQquires
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
                new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 92, 81, 60}},
                new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
                new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {88, 94, 65, 91}},
                new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {97, 89, 85, 82}},
                new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {35, 72, 91, 70}},
                new Student {First="Fadi", Last="Fakhouri", ID=116, Scores= new List<int> {99, 86, 90, 94}},
                new Student {First="Hanying", Last="Feng", ID=117, Scores= new List<int> {93, 92, 80, 87}},
                new Student {First="Hugo", Last="Garcia", ID=118, Scores= new List<int> {92, 90, 83, 78}},
                new Student {First="Lance", Last="Tucker", ID=119, Scores= new List<int> {68, 79, 88, 92}},
                new Student {First="Terry", Last="Adams", ID=120, Scores= new List<int> {99, 82, 81, 79}},
                new Student {First="Eugene", Last="Zabokritski", ID=121, Scores= new List<int> {96, 85, 91, 60}},
                new Student {First="Michael", Last="Tucker", ID=122, Scores= new List<int> {94, 92, 91, 91} },
                new Student {First="Toke", Last="Frostholm", ID = 123, Scores = new List<int> {99, 90, 98, 95} }
            };

            // return student whose score in the first test was over 90 and scores in third test was less than 80
            IEnumerable<Student> studentQuery =
                from student in students
                where student.Scores[0] > 90 && student.Scores[3] < 80
                orderby student.Scores[0] descending // Sorts students by score on the first test
                select student;

            Console.WriteLine("Students with a score higher than 90 in test 1 and lower than 80 in test 2. Ordered by score in test 3:");
            foreach (Student student in studentQuery)
            {
                Console.WriteLine("{0}, {1}, {2}", student.Last, student.First, student.Scores[0]);
            }
            Console.WriteLine("");

            // Grouping students by the first letter of their last name as key
            IEnumerable<IGrouping<char, Student>> studentQuery2 =
                from student in students
                group student by student.Last[0];

            Console.WriteLine("Grouping students by first letter of their last name:");
            foreach (var studentGroup in studentQuery2)
            {
                Console.WriteLine(studentGroup.Key);
                foreach (Student student in studentGroup)
                {
                    Console.WriteLine("   {0}, {1}", student.Last, student.First);
                }
            }
            Console.WriteLine("");

            // Using var instead of having to code IEnumerables of IGrouping. Does exactly the same as above code
            var studentQuery3 =
                from student in students
                group student by student.Last[0];

            Console.WriteLine("Does the same as above");
            foreach (var groupOfStudents in studentQuery3)
            {
                Console.WriteLine(groupOfStudents.Key);
                foreach (var student in groupOfStudents)
                {
                    Console.WriteLine("    {0}, {1}", student.Last, student.First);
                }
            }
            Console.WriteLine("");

            // Using the 'into' keyword provide identifier so we can use orderby after the group clause
            var studentQuery4 =
                from student in students
                group student by student.Last[0]
                into studentGroup
                orderby studentGroup.Key
                select studentGroup;

            Console.WriteLine("Sorts students in alphabetical order, but otherwise same as above");
            foreach (var groupOfStudents in studentQuery4)
            {
                Console.WriteLine(groupOfStudents.Key);
                foreach (var student in groupOfStudents)
                {
                    Console.WriteLine("    {0}, {1}", student.Last, student.First);
                }
            }
            Console.WriteLine("");

            // Using method syntax in query expression
            // Some queries can only be expressed bu using method syntax.
            var studentQuery6 =
                from student in students
                let totalScore = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
                select totalScore;

            double averageScore = studentQuery6.Average();
            Console.WriteLine("Class average score = {0}", averageScore);

            //Transforming a project in the select clause
            IEnumerable<string> studentQuery7 =
                from student in students
                where student.Last == "Garcia"
                select student.First;

            Console.WriteLine("The Garcias in the class are:");
            foreach (string s in studentQuery7)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("");

            // Getting students with a score greater than the class average
            var studentQuery8 =
                from student in students
                let x = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
                where x > averageScore
                select new {id = student.ID, score = x};

            Console.WriteLine("Students in class with a score greater than class average:");
            foreach (var item in studentQuery8)
            {
                Console.WriteLine("Student ID: {0}, Score: {1}", item.id, item.score);
            }

            Console.ReadKey();
        }
    }
}
