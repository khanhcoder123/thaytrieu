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

            var data = _dbContext.TraineeCourses
                .Where(m => m.deleted_at == null)
                .Join(
                    _dbContext.Users,
                    traineeCourse => traineeCourse.trainee_id,
                    trainee => trainee.id,
                    (traineeCourse, trainee) => new
                    {
                        TraineeCourse = traineeCourse,
                        TraineeName = trainee.full_name
                    })
                .Join(
                    _dbContext.Courses,
                    result => result.TraineeCourse.course_id,
                    course => course.id,
                    (result, course) => new
                    {
                        result.TraineeCourse,
                        result.TraineeName,
                        CourseName = course.name
                    })
                .ToList();

            foreach (var item in data)
            {
                traineecourseModel.TraineeCourseDetailLists.Add(new TraineeCourseDetail
                {
                    id = item.TraineeCourse.id,
                    course_id = item.TraineeCourse.course_id,
                    trainee_id = item.TraineeCourse.trainee_id,
                    traineeName = item.TraineeName,
                    courseName = item.CourseName,
                    created_at = item.TraineeCourse.created_at,
                    updated_at = item.TraineeCourse.updated_at
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
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.TraineeCourses.FirstOrDefault(m => m.id == id);

                if (data != null)
                {
                    // Soft delete by updating the deleted_at field
                    data.deleted_at = DateTime.Now;
                    _dbContext.SaveChanges();
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteStatus"] = false;
                // Log the exception if needed: _logger.LogError(ex, "An error occurred while deleting the topic.");
            }

            return RedirectToAction(nameof(Index), new { SearchString = "" });
        }

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            TraineeCourseDetail traineecourse = new TraineeCourseDetail();
            var data = _dbContext.TraineeCourses.Where(m => m.id == traineecourse.id).FirstOrDefault();
            if (data != null)
            {
                traineecourse.id = data.id;
                traineecourse.trainee_id = data.trainee_id;
                traineecourse.course_id = data.course_id;
                
            }

            var courseList = _dbContext.Courses
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 4)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;
            ViewBag.SelectedCourseId = data.course_id;

            return View(traineecourse);
        }

        [HttpPost]
        public IActionResult Update(TraineeCourseDetail traineecourse)
        {
            
            try
            {
                var data = _dbContext.TraineeCourses.Where(m => m.id == traineecourse.id).FirstOrDefault();

                if (data != null)
                {
                    Console.WriteLine(traineecourse.course_id.ToString());

                    Console.WriteLine(traineecourse.trainee_id.ToString());
                    data.course_id = traineecourse.course_id;
                    data.trainee_id = traineecourse.trainee_id;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["UpdateStatus"] = true;

                }
                else
                {
                    Console.WriteLine(traineecourse.id.ToString());
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(traineecourse.id.ToString());
                TempData["UpdateStatus"] = false;
                return Ok(new { Status = "Error", Message = ex.Message });
            }
            return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourse");

        }
    }
}
