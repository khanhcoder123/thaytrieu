using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TopicController : Controller
    {
        private readonly TranningDBContext _dbContext;
        private readonly ILogger<TopicController> _logger;

        public TopicController(TranningDBContext context, ILogger<TopicController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            var data = _dbContext.Topics
                .Where(m => m.deleted_at == null)
                .Select(item => new TopicDetail
                {
                    course_id = item.course_id,
                    id = item.id,
                    name = item.name,
                    description = item.description,
                    videos = item.videos,
                    status = item.status,
                    attach_file = item.attach_file,
                    documents = item.documents,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                })
                .ToList();

            // Apply additional search filter if needed
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString)).ToList();
            }

            TopicModel topicModel = new TopicModel();
            topicModel.TopicDetailLists = data;

            return View(topicModel);
        }

        [HttpGet]
        public IActionResult TrainerIndex(string SearchString)
        {
            var data = _dbContext.Topics
                .Where(m => m.deleted_at == null)
                .Select(item => new TopicDetail
                {
                    course_id = item.course_id,
                    id = item.id,
                    name = item.name,
                    description = item.description,
                    videos = item.videos,
                    status = item.status,
                    attach_file = item.attach_file,
                    documents = item.documents,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                })
                .ToList();

            // Apply additional search filter if needed
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString)).ToList();
            }

            TopicModel topicModel = new TopicModel();
            topicModel.TopicDetailLists = data;

            return View(topicModel);
        }


        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            PopulateCategoryDropdown();
            PopulateCategoryDropdown1();
            return View(topic);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        string uniqueFileName = await UploadFile(topic.photo);
                        string file = await UploadFile(topic.file);
                        var topicData = new Topic()
                        {
                            course_id = topic.course_id,
                            name = topic.name,
                            description = topic.description,
                            videos = uniqueFileName,
                            status = topic.status,
                            documents = topic.documents,
                            attach_file = file,
                            created_at = DateTime.Now
                        };

                        _dbContext.Topics.Add(topicData);
                        _dbContext.SaveChanges();
                        TempData["saveStatus"] = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while processing a valid model state.");
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(Index));
                }

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                    }
                }

                PopulateCategoryDropdown();
                PopulateCategoryDropdown1();
                return View(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the request.");
                TempData["saveStatus"] = false;
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            string uniqueFileName;
            try
            {
                string pathUploadServer = "wwwroot\\uploads\\images";
                string fileName = file.FileName;
                fileName = Path.GetFileName(fileName);
                string uniqueStr = Guid.NewGuid().ToString();
                fileName = uniqueStr + "-" + fileName;
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileName);
                var stream = new FileStream(uploadPath, FileMode.Create);
                await file.CopyToAsync(stream);
                uniqueFileName = fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file upload.");
                uniqueFileName = ex.Message.ToString();
            }
            return uniqueFileName;
        }

        private void PopulateCategoryDropdown()
        {
            try
            {
                var courses = _dbContext.Courses
                    .Where(m => m.deleted_at == null)
                    .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                    .ToList();

                if (courses != null)
                {
                    ViewBag.Stores = courses;
                }
                else
                {
                    _logger.LogError("Course is null");
                    ViewBag.Stores = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while populating category dropdown.");
                ViewBag.Stores = new List<SelectListItem>();
            }
        }

        private void PopulateCategoryDropdown1()
        {
            try
            {
                var users = _dbContext.Users
                    .Where(m => m.deleted_at == null && m.role_id == 3)
                    .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name })
                    .ToList();

                if (users != null)
                {
                    ViewBag.Stores1 = users;
                }
                else
                {
                    _logger.LogError("User is null");
                    ViewBag.Stores1 = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while populating category dropdown.");
                ViewBag.Stores1 = new List<SelectListItem>();
            }
        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.Topics.FirstOrDefault(m => m.id == id);

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
            var data = _dbContext.Topics.FirstOrDefault(m => m.id == id);

            if (data != null)
            {
                // Set ViewBag.Stores to populate the dropdown in the view
                PopulateCategoryDropdown();
                PopulateCategoryDropdown1();

                // Map the data to the TopicDetail model
                var topic = new TopicDetail
                {
                    id = data.id,
                    course_id = data.course_id,
                    name = data.name,
                    description = data.description,
                    status = data.status
                    // Map other properties as needed
                };

                return View(topic);
            }
            else
            {
                TempData["UpdateStatus"] = false;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TopicDetail topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _dbContext.Topics.FirstOrDefault(m => m.id == topic.id);

                    if (data != null)
                    {
                        data.name = topic.name;
                        data.description = topic.description;
                        data.status = topic.status;

                        // Update the file fields if a new file is provided
                        if (topic.file != null)
                        {
                            data.attach_file = await UploadFile(topic.file);
                        }

                        if (topic.photo != null)
                        {
                            data.videos = await UploadFile(topic.photo);
                        }

                        data.updated_at = DateTime.Now;

                        _dbContext.SaveChanges();
                        TempData["UpdateStatus"] = true;
                    }
                    else
                    {
                        TempData["UpdateStatus"] = false;
                    }

                    return RedirectToAction(nameof(Index));
                }

                // If ModelState is not valid, repopulate the dropdown and return to the view
                PopulateCategoryDropdown();
                PopulateCategoryDropdown1();
                return View(topic);
            }
            catch (Exception ex)
            {
                TempData["UpdateStatus"] = false;
                // Log the exception if needed: _logger.LogError(ex, "An error occurred while updating the topic.");
                return RedirectToAction(nameof(Index));
            }
        }

    }
}