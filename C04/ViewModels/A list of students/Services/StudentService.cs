using ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public class StudentService
    {
        public Task<IEnumerable<Student>> ReadAllAsync()
        {
            return Task.FromResult(FakeData.Students.AsEnumerable());
        }

        private class FakeData
        {
            public static List<Student> Students { get; }

            static FakeData()
            {
                var mathClass = new Class { Id = 1, Name = "Math" };
                var scienceClass = new Class { Id = 2, Name = "Science" };
                var computerClass = new Class { Id = 3, Name = "Computer" };
                Students = new List<Student>
                {
                    new Student
                    {
                        Id = 1,
                        Name = "Maddie Powers",
                        Classes = new List<Class>{ mathClass, scienceClass }
                    },
                    new Student
                    {
                        Id = 2,
                        Name = "Harper Black",
                        Classes = new List<Class>{ mathClass, scienceClass, computerClass }
                    },
                    new Student
                    {
                        Id = 3,
                        Name = "Allen York",
                        Classes = new List<Class>{ mathClass, computerClass }
                    },
                    new Student
                    {
                        Id = 4,
                        Name = "Lillie Adkins",
                        Classes = new List<Class>{ scienceClass, computerClass }
                    }
                };
            }
        }
    }
}
