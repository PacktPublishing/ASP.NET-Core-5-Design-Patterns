using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C01.ViewModels.ViewModels
{
    public class CreateStudentViewModel : StudentFormViewModel
    {

    }

    public class EditStudentViewModel : StudentFormViewModel
    {
        public int Id { get; set; }
        public IEnumerable<string> Classes { get; set; }
    }

    public class StudentFormViewModel
    {
        public string Name { get; set; }
    }
}
