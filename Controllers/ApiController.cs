using APIToDoList.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Globalization;
using BCrypt;

namespace APIToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : Controller
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        DataBaseContext db = new DataBaseContext();



        ////////////////////////////

        [HttpPost("Registr/{UserName}/{Password}/{RepitPassword}")]  //Регистрация
        public async Task<ActionResult<UserT>> RegistrUser(string UserName, string Password, string RepitPassword)
        {
            if (db.UserTs.FirstOrDefault(z => z.Username == UserName) != null)
            {
                return BadRequest("Пользователь с таким именем уже существует");
            }
            else if (UserName == "" || Password == "" || RepitPassword == "")
            {
                return BadRequest("Заполните все поля");
            }
            else if (Password != RepitPassword)
            {
                return BadRequest("Пароли не совпадают");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password, salt);

            UserT NewUser = new UserT
            {
                Username = UserName,
                PasswordHash = hashedPassword
            };

            db.UserTs.Add(NewUser);
            db.SaveChanges();
            return Ok(NewUser);
        }



        ////////////////////////////
        
        [HttpGet("Auth/{Username}/{Password}")] // Авторизация
        public async Task<ActionResult<UserT>> AuthUser(string Username, string Password)
        {
            if (Username == null || Password == null)
            {
                return BadRequest("Заполните поля");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password, salt);

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(Password, hashedPassword);

            var GetUser = db.UserTs.FirstOrDefault(z => z.Username == Username && isPasswordCorrect == true);

            if (GetUser == null)
            {
                return NotFound("Не верные данные");
            }

            return Ok(GetUser);
        }



        ////////////////////////////

        [HttpGet("User/{Username}")] // Получение инфорамации о пользователе
        public async Task<ActionResult<UserT>> GetUserinfo(string Username)
        {
            if (Username == "")
            {
                return BadRequest("Укажите имя пользователя");
            }

            UserT GetUser = db.UserTs.FirstOrDefault(z => z.Username == Username);

            if (GetUser == null)
            {
                return NotFound("Пользователь не найден");
            }

            return Ok(GetUser);
        }



        ////////////////////////////

        [HttpGet("Task/{IdUser}")] // Получение списка задач
        public async Task<ActionResult<TaskT>> GetTask(int IdUser)
        {
            if (IdUser == null)
            {
                return BadRequest("Заполните поле");
            }

            var GetTask = db.TaskTs.Where(z => z.UserId == IdUser).ToList();

            if (GetTask == null)
            {
                return NotFound("Задачи не найдены");
            }

            return Ok(GetTask);
        }



        ////////////////////////////

        [HttpPost("CreateTask/{IdUser}/{Name}/{Description}/{Deadline}/{Priority}/{Done}")] // Создание новой задачи
        public async Task<ActionResult<TaskT>> CreateTask(int IdUser, string Name, string Description, string Deadline, int Priority, bool Done)
        {
            if (IdUser == null || Name == "" || Description == "" || Deadline == null || Priority == null || Done == null)
            {
                return BadRequest("Заполните поля");
            }
            try
            {
                DateOnly date = DateOnly.Parse(Deadline, CultureInfo.InvariantCulture);

                TaskT NewTask = new TaskT
                {
                    UserId = IdUser,
                    Name = Name,
                    Description = Description,
                    Deadline = date,
                    Priority = Priority,
                    Done = Done
                };

                db.Add(NewTask);
                db.SaveChanges();
                return Ok(NewTask);
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync("Произошла ошибка: " + ex.Message);
                return BadRequest();
            }
        }



        ////////////////////////////
        
        [HttpPut("Update/{IdTask}/{Name}/{Description}")] // Обновление задачи
        public async Task<ActionResult<TaskT>> UpdateTask(int IdTask, string Name, string Description)
        {
            if (IdTask == null || Name == "" || Description == "")
            {
                return BadRequest("Заполните поля");
            }

            var GetTask = db.TaskTs.FirstOrDefault(z => z.Id == IdTask);

            if (GetTask == null)
            {
                return NotFound("Задача не найдена");
            }

            GetTask.Name = Name;
            GetTask.Description = Description;
            db.SaveChanges();

            return Ok(GetTask);
        }



        ////////////////////////////

        [HttpDelete("Delete/{IdTask}")] // Удаление задачи
        public async Task<ActionResult<TaskT>> DeleteTask(int IdTask)
        {
            if (IdTask == null)
            {
                return BadRequest("Заполните поля");
            }

            var GetTask = db.TaskTs.FirstOrDefault(z => z.Id == IdTask);

            if (GetTask == null)
            {
                return NotFound("Задача не найдена");
            }

            db.TaskTs.Remove(GetTask);
            db.SaveChanges();

            return Ok();
        }



        ////////////////////////////

        [HttpGet(template: "Test")]
        public async Task<ActionResult> Test()
        {
            return Ok("Усппешно");

        }
    }
}
