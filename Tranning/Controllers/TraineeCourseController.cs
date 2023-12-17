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

            var data = _dbContext.TraineeCourses.
                Join(_dbContext.Courses, tr => tr.course_id, c => c.id, (tr, c) => new {tr, c})
                .Join(_dbContext.Users, trr => trr.tr.trainee_id, u => u.id, (trr, u) => new {trr, u})
                .Select(m => new TraineeCourseDetail
                {
                    course_id = m.trr.tr.course_id,
                    trainee_id = m.trr.tr.trainee_id,
                    created_at = m.trr.tr.created_at,
                    updated_at = m.trr.tr.updated_at,
                    CourseName = m.trr.c.name,
                    TraineeName = m.u.full_name,
                    deleted_at = m.trr.tr.deleted_at
                })
                .Where(m => m.deleted_at == null);
            traineecourseModel.TraineeCourseDetailLists = data.ToList();
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

            return RedirectToAction(nameof(TraineeCourseController.Index), "TraineeCourse");
        }

        [HttpGet]
        public IActionResult Update(int trainee_id = 0, int course_id = 0)
        {
            var existingData = _dbContext.TraineeCourses
                .Where(tc => tc.trainee_id == trainee_id && tc.course_id == course_id)
                .FirstOrDefault();

            if (existingData == null)
            {
                return NotFound();
            }

            var traineeCourseDetail = new TraineeCourseDetail
            {
                course_id = existingData.course_id,
                trainee_id = existingData.trainee_id,
                // Include other properties as needed
            };

            var courseList = _dbContext.Courses
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;

            var traineeList = _dbContext.Users
                .Where(m => m.deleted_at == null && m.role_id == 4)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = traineeList;

            return View(traineeCourseDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TraineeCourseDetail updatedData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingData = _dbContext.TraineeCourses
                        .Where(tc => tc.trainee_id == updatedData.trainee_id && tc.course_id == updatedData.course_id)
                        .FirstOrDefault();

                    if (existingData != null)
                    {
                        existingData.course_id = updatedData.course_id;
                        existingData.trainee_id = updatedData.trainee_id;
                        // Include other properties as needed

                        _dbContext.SaveChanges(true);

                        TempData["UpdateStatus"] = true;
                    }
                    else
                    {
                        TempData["UpdateStatus"] = false;
                    }
                }
                catch (Exception ex)
                {
                    TempData["UpdateStatus"] = false;
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

            return View(updatedData);
        }

    }
}
