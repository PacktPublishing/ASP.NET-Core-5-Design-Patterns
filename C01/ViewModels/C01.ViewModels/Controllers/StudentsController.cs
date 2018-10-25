using C01.ViewModels.Services;
using C01.ViewModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C01.ViewModels.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentService _studentService = new StudentService();

        public async Task<IActionResult> IndexAsync()
        {
            // Get data from the data store
            var students = await _studentService.ReadAllAsync();

            // Create the ViewModel, based on the data
            var viewModel = new StudentListViewModel
            {
                Students = students.Select(student => new StudentListItemViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    ClassCount = student.Classes.Count()
                })
            };

            // Return the View
            return View(viewModel);
        }
    }
}
