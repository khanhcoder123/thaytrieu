using Microsoft.AspNetCore.Mvc;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class UserController : Controller
    {
        private readonly TranningDBContext _dbContext;

        public UserController(TranningDBContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        public IActionResult TSIndex(string SearchString)
        {

            UserModel userModel = new UserModel();
            userModel.UserDetailLists = new List<UserDetail>();

            var data = from m in _dbContext.Users
                       select m;

            data = data.Where(m => m.deleted_at == null && m.role_id == 2);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.full_name.Contains(SearchString) || m.phone.Contains(SearchString));
            }

            data.ToList();

            foreach (var item in data)
            {
                userModel.UserDetailLists.Add(new UserDetail
                {
                    id = item.id,
                    role_id = item.role_id,
                    extra_code = item.extra_code,
                    username = item.username,
                    password = item.password,
                    email = item.email,
                    phone = item.phone,
                    gender = item.gender,
                    status = item.status,
                    full_name = item.full_name,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(userModel);
        }

        [HttpGet]
        public IActionResult TSAdd()
        {

            UserDetail user = new UserDetail();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TSAdd(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userData = new User()
                    {
                        username = user.username,
                        role_id = user.role_id,
                        password = user.password,
                        extra_code = user.extra_code,
                        full_name = user.full_name,
                        email = user.email,
                        phone = user.phone,
                        status = user.status,
                        gender = user.gender,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    _dbContext.Users.Add(userData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    
                    TempData["saveStatus"] = false;
                    
                }
                return RedirectToAction(nameof(UserController.TSIndex), "User");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult TSUpdate(int id = 0)
        {
            UserDetail user = new UserDetail();
            var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
            if (data != null)
            {
                user.id = data.id;
                user.role_id = data.role_id;
                user.username = data.username;
                user.extra_code = data.extra_code;
                user.full_name = data.full_name;
                user.email = data.email;
                user.status = data.status;
                user.phone = data.phone;
                user.gender = data.gender;
            }

            return View(user);
        }


        [HttpPost]
        public IActionResult TSUpdate(UserDetail user)
        {
            try
            {

                var data = _dbContext.Users.Where(m => m.id == user.id).FirstOrDefault();


                if (data != null)
                {
                    // gan lai du lieu trong db bang du lieu tu form model gui len
                    data.role_id = user.role_id;
                    data.username = user.username;
                    data.extra_code = user.extra_code;
                    data.full_name = user.full_name;
                    data.email = user.email;
                    data.status = user.status;
                    data.phone = user.phone;
                    data.gender = user.gender;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            return RedirectToAction(nameof(UserController.TSIndex), "User");
        }

        [HttpGet]
        public IActionResult TSDelete(int id = 0)
        {
            try
            {
                var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
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
            return RedirectToAction(nameof(UserController.TSIndex), "User");
        }

        [HttpGet]
        public IActionResult TrainerIndex(string SearchString)
        {

            UserModel userModel = new UserModel();
            userModel.UserDetailLists = new List<UserDetail>();

            var data = from m in _dbContext.Users
                       select m;

            data = data.Where(m => m.deleted_at == null && m.role_id == 3);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.full_name.Contains(SearchString) || m.phone.Contains(SearchString));
            }

            data.ToList();

            foreach (var item in data)
            {
                userModel.UserDetailLists.Add(new UserDetail
                {
                    id = item.id,
                    role_id = item.role_id,
                    extra_code = item.extra_code,
                    username = item.username,
                    password = item.password,
                    email = item.email,
                    phone = item.phone,
                    gender = item.gender,
                    status = item.status,
                    full_name = item.full_name,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(userModel);
        }

        [HttpGet]
        public IActionResult TrainerAdd()
        {

            UserDetail user = new UserDetail();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrainerAdd(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userData = new User()
                    {
                        username = user.username,
                        role_id = user.role_id,
                        password = user.password,
                        extra_code = user.extra_code,
                        full_name = user.full_name,
                        email = user.email,
                        phone = user.phone,
                        status = user.status,
                        gender = user.gender,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    _dbContext.Users.Add(userData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {

                    TempData["saveStatus"] = false;

                }
                return RedirectToAction(nameof(UserController.TrainerIndex), "User");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult TrainerUpdate(int id = 0)
        {
            UserDetail user = new UserDetail();
            var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
            if (data != null)
            {
                user.id = data.id;
                user.role_id = data.role_id;
                user.username = data.username;
                user.extra_code = data.extra_code;
                user.full_name = data.full_name;
                user.email = data.email;
                user.status = data.status;
                user.phone = data.phone;
                user.gender = data.gender;
            }

            return View(user);
        }


        [HttpPost]
        public IActionResult TrainerUpdate(UserDetail user)
        {
            try
            {

                var data = _dbContext.Users.Where(m => m.id == user.id).FirstOrDefault();


                if (data != null)
                {
                    // gan lai du lieu trong db bang du lieu tu form model gui len
                    data.role_id = user.role_id;
                    data.username = user.username;
                    data.extra_code = user.extra_code;
                    data.full_name = user.full_name;
                    data.email = user.email;
                    data.status = user.status;
                    data.phone = user.phone;
                    data.gender = user.gender;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            return RedirectToAction(nameof(UserController.TrainerIndex), "User");
        }

        [HttpGet]
        public IActionResult TrainerDelete(int id = 0)
        {
            try
            {
                var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
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
            return RedirectToAction(nameof(UserController.TrainerIndex), "User");
        }

        [HttpGet]
        public IActionResult TraineeIndex(string SearchString)
        {

            UserModel userModel = new UserModel();
            userModel.UserDetailLists = new List<UserDetail>();

            var data = from m in _dbContext.Users
                       select m;

            data = data.Where(m => m.deleted_at == null && m.role_id == 4);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.full_name.Contains(SearchString) || m.phone.Contains(SearchString));
            }

            data.ToList();

            foreach (var item in data)
            {
                userModel.UserDetailLists.Add(new UserDetail
                {
                    id = item.id,
                    role_id = item.role_id,
                    extra_code = item.extra_code,
                    username = item.username,
                    password = item.password,
                    email = item.email,
                    phone = item.phone,
                    gender = item.gender,
                    status = item.status,
                    full_name = item.full_name,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(userModel);
        }

        [HttpGet]
        public IActionResult TraineeAdd()
        {

            UserDetail user = new UserDetail();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TraineeAdd(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userData = new User()
                    {
                        username = user.username,
                        role_id = user.role_id,
                        password = user.password,
                        extra_code = user.extra_code,
                        full_name = user.full_name,
                        email = user.email,
                        phone = user.phone,
                        status = user.status,
                        gender = user.gender,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    _dbContext.Users.Add(userData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {

                    TempData["saveStatus"] = false;

                }
                return RedirectToAction(nameof(UserController.TraineeIndex), "User");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult TraineeUpdate(int id = 0)
        {
            UserDetail user = new UserDetail();
            var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
            if (data != null)
            {
                user.id = data.id;
                user.role_id = data.role_id;
                user.username = data.username;
                user.extra_code = data.extra_code;
                user.full_name = data.full_name;
                user.email = data.email;
                user.status = data.status;
                user.phone = data.phone;
                user.gender = data.gender;
            }

            return View(user);
        }


        [HttpPost]
        public IActionResult TraineeUpdate(UserDetail user)
        {
            try
            {

                var data = _dbContext.Users.Where(m => m.id == user.id).FirstOrDefault();


                if (data != null)
                {
                    // gan lai du lieu trong db bang du lieu tu form model gui len
                    data.role_id = user.role_id;
                    data.username = user.username;
                    data.extra_code = user.extra_code;
                    data.full_name = user.full_name;
                    data.email = user.email;
                    data.status = user.status;
                    data.phone = user.phone;
                    data.gender = user.gender;
                    data.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            return RedirectToAction(nameof(UserController.TraineeIndex), "User");
        }

        [HttpGet]
        public IActionResult TraineeDelete(int id = 0)
        {
            try
            {
                var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
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
            return RedirectToAction(nameof(UserController.TraineeIndex), "User");
        }


    }
}
