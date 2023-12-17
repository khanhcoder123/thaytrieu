using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TrainerTopicController : Controller
    {
        private readonly TranningDBContext _dbContext;
        public TrainerTopicController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            TrainerTopicModel trainertopicModel = new TrainerTopicModel();
            trainertopicModel.TrainerTopicDetailLists = new List<TrainerTopicDetail>();

            var data = _dbContext.TrainerTopics
                .Where(m => m.deleted_at == null)
                .Join(
                    _dbContext.Users,
                    trainerTopic => trainerTopic.trainer_id,
                    trainer => trainer.id,
                    (trainerTopic, trainer) => new
                    {
                        TrainerTopic = trainerTopic,
                        TrainerName = trainer.full_name
                    })
                .Join(
                    _dbContext.Topics,
                    result => result.TrainerTopic.topic_id,
                    topic => topic.id,
                    (result, topic) => new
                    {
                        result.TrainerTopic,
                        result.TrainerName,
                        TopicName = topic.name
                    })
                .ToList();

            foreach (var item in data)
            {
                trainertopicModel.TrainerTopicDetailLists.Add(new TrainerTopicDetail
                {
                    topic_id = item.TrainerTopic.topic_id,
                    trainer_id = item.TrainerTopic.trainer_id,
                    trainerName = item.TrainerName,
                    topicName = item.TopicName,
                    created_at = item.TrainerTopic.created_at,
                    updated_at = item.TrainerTopic.updated_at
                });
            }

            ViewData["CurrentFilter"] = SearchString;
            return View(trainertopicModel);
        }


        [HttpGet]
        public IActionResult Add()
        {
            TrainerTopicDetail trainertopic = new TrainerTopicDetail();
            var topicList = _dbContext.Topics
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = topicList;

            var trainerList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 3)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = trainerList;

            return View(trainertopic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TrainerTopicDetail trainertopic)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var trainertopicData = new TrainerTopic()
                    {
                        topic_id = trainertopic.topic_id,
                        trainer_id = trainertopic.trainer_id,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    _dbContext.TrainerTopics.Add(trainertopicData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }

                catch (Exception ex)
                {

                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(TrainerTopicController.Index), "TrainerTopic");
            }


            var topicList = _dbContext.Topics
                      .Where(m => m.deleted_at == null)
                      .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = topicList;

            var trainerList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 3)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = trainerList;


            Console.WriteLine(ModelState.IsValid);
            foreach (var key in ModelState.Keys)
            {
                var error = ModelState[key].Errors.FirstOrDefault();
                if (error != null)
                {
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }
            return View(trainertopic);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.TrainerTopics.FirstOrDefault(m => m.id == id);

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
            TrainerTopicDetail trainertopic = new TrainerTopicDetail();
            var data = _dbContext.TrainerTopics.Where(m => m.id == trainertopic.id).FirstOrDefault();
            if (data != null)
            {
                trainertopic.id = data.id;
                trainertopic.topic_id = data.topic_id;
                trainertopic.trainer_id = data.trainer_id;
            }

            var topicList = _dbContext.Topics
                  .Where(m => m.deleted_at == null)
                  .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = topicList;

            var trainerList = _dbContext.Users
              .Where(m => m.deleted_at == null && m.role_id == 3)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.full_name }).ToList();
            ViewBag.Stores1 = trainerList;

            return View(trainertopic);
        }
        [HttpPost]
        public IActionResult Update(TrainerTopicDetail trainertopic)
        {

            try
            {
                var data = _dbContext.TrainerTopics.Where(m => m.id == trainertopic.id).FirstOrDefault();

                if (data != null)
                {
                    data.topic_id = trainertopic.topic_id;
                    data.trainer_id = trainertopic.trainer_id;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["UpdateStatus"] = true;

                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch
            {
                TempData["UpdateStatus"] = false;
            }
            return RedirectToAction(nameof(TrainerTopicController.Index), "TrainerTopic");

        }
    }
    }
