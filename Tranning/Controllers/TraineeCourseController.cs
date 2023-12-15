using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TraineeCourseController : Controller
    {
        private readonly TranningDBContext _dbContext;
        public TraineeCourseController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            TraineeCourseModel traineecourseModel = new TraineeCourseModel();
            traineecourseModel.TraineeCourseDetailLists = new List<TraineeCourseDetail>();

            var data = _dbContext.TraineeCourses.Where(m => m.deleted_at == null);



            var traineecourses = data.ToList();

            foreach (var item in traineecourses)
            {
                traineecourseModel.TraineeCourseDetailLists.Add(new TraineeCourseDetail
                {
                    course_id = item.course_id,
                    trainee_id = item.trainee_id,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }

            ViewData["CurrentFilter"] = SearchString;
            return View(traineecourseModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TraineeCourseDetail traineecourse = new TraineeCourseDetail();
            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;

            return View(traineecourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TraineeCourseDetail traineecourse)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var traineecourseData = new TraineeCourse()
                    {
                        course_id = traineecourse.course_id,
                        trainee_id = traineecourse.trainee_id,                        
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };

                    _dbContext.TraineeCourses.Add(traineecourseData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourse");
            }                         
                               
       
            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;

            
            Console.WriteLine(ModelState.IsValid);
            foreach (var key in ModelState.Keys)
            {
                var error = ModelState[key].Errors.FirstOrDefault();
                if (error != null)
                {
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }
            return View(traineecourse);
        }

        [HttpGet]
        public IActionResult Delete(int trainee_id = 0, int course_id = 0)
        {
            try
            {
                var data = _dbContext.TraineeCourses
                    .Where(tc => tc.trainee_id == trainee_id && tc.course_id == course_id)
                    .FirstOrDefault();

                if (data != null)
                {
                    data.deleted_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch
            {
                TempData["DeleteStatus"] = false;
            }

            return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourseController");
        }
    }
}
