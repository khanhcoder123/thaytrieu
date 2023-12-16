using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Index(string searchString)
        {
            var trainerTopicModel = new TrainerTopicModel
            {
                TrainerTopicDetailLists = _dbContext.TrainerTopics
                    .Where(tt => tt.deleted_at == null)
                    .Join(_dbContext.Topics, tt => tt.topic_id, t => t.id, (tt, t) => new { tt, t })
                    .Join(_dbContext.Users, ttt => ttt.tt.trainer_id, u => u.id, (ttt, u) => new TrainerTopicDetail
                    {
                        topic_id = ttt.tt.topic_id,
                        trainer_id = ttt.tt.trainer_id,
                        created_at = ttt.tt.created_at,
                        updated_at = ttt.tt.updated_at,
                        TopicName = ttt.t.name,
                        TrainerName = u.full_name,
                        deleted_at = ttt.tt.deleted_at
                    })
                    .Where(m => m.deleted_at == null)
                    .ToList()
            };

            ViewData["CurrentFilter"] = searchString;
            return View(trainerTopicModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var trainerTopic = new TrainerTopicDetail();
            var topicList = _dbContext.Topics
                .Where(t => t.deleted_at == null)
                .Select(t => new SelectListItem { Value = t.id.ToString(), Text = t.name })
                .ToList();

            var traineeList = _dbContext.Users
                .Where(u => u.deleted_at == null && u.role_id == 4)
                .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                .ToList();

            ViewBag.Stores = topicList;
            ViewBag.Stores1 = traineeList;

            return View(trainerTopic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TrainerTopicDetail trainerTopic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var trainerTopicData = new TrainerTopic
                    {
                        topic_id = trainerTopic.topic_id,
                        trainer_id = trainerTopic.trainer_id,
                        created_at = DateTime.UtcNow
                    };

                    _dbContext.TrainerTopics.Add(trainerTopicData);
                    _dbContext.SaveChanges();
                    TempData["saveStatus"] = true;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["saveStatus"] = false;
                    // Log the exception for debugging purposes
                    // LogException(ex);
                }
            }

            PopulateDropdowns();
            return View(trainerTopic);
        }

        [HttpGet]
        public IActionResult Delete(int topic_id = 0, int trainer_id = 0)
        {
            try
            {
                var data = _dbContext.TrainerTopics
                    .FirstOrDefault(tt => tt.topic_id == topic_id && tt.trainer_id == trainer_id);

                if (data != null)
                {
                    data.deleted_at = DateTime.UtcNow;
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
                // Log the exception for debugging purposes
                // LogException(ex);
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            ViewBag.Stores = _dbContext.Topics
                .Where(t => t.deleted_at == null)
                .Select(t => new SelectListItem { Value = t.id.ToString(), Text = t.name })
                .ToList();

            ViewBag.Stores1 = _dbContext.Users
                .Where(u => u.deleted_at == null && u.role_id == 4)
                .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                .ToList();
        }

        // Consider adding a LogException method for logging exceptions
        // private void LogException(Exception ex)
        // {
        //     // Your logging implementation
        // }
    }
}
