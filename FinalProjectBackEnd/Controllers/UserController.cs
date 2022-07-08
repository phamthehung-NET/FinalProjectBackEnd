using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllStudent(string keyword, int? filter, int? sy, int? pageIndex, int? itemPerPage)
        {
            keyword = keyword ?? "";
            filter = filter ?? 0;
            sy = sy ?? DateTime.Now.Year;
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var students = userService.GetAllStudents(keyword, filter, sy, pageIndex, itemPerPage);

                return Ok(students);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddStudent(UserDTO student)
        {
            try
            {
                await userService.AddStudent(student);
                return Ok("Add student successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult EditStudent(UserDTO student)
        {
            try
            {
                userService.EditStudent(student);
                return Ok("Edit student successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetStudentDetail(string id)
        {
            try
            {
                var student = userService.GetStudentDetail(id);
                return Ok(student.FirstOrDefault());
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpDelete]
        public ActionResult DropOutStudent(string id)
        {
            try
            {
                userService.DeleteStudent(id);
                return Ok("Drop student out successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllTeacher(string keyword, bool? filter, int? pageIndex, int? itemPerPage)
        {
            keyword = keyword ?? "";
            filter = filter ?? false;
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var teachers = userService.GetAllTeachers(keyword, filter, pageIndex, itemPerPage);

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddTeacher(UserDTO teacher)
        {
            try
            {
                await userService.AddTeacher(teacher);
                return Ok("Add teacher successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult EditTeacher(UserDTO teacher)
        {
            try
            {
                userService.EditTeacher(teacher);
                return Ok("Edit teacher successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetTeacherDetail(string id)
        {
            try
            {
                var teacher = userService.GetTeacherDetail(id);
                return Ok(teacher.FirstOrDefault());
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpDelete]
        public ActionResult DeleteTeacher(string id)
        {
            try
            {
                userService.DeleteTeacher(id);
                return Ok("Delete teacher successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllMonitorStudent(int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var monitor = userService.GetAllMonitorStudents(pageIndex, itemPerPage);
                return Ok(monitor);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAllSecretaryStudent(int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var secretary = userService.GetAllSecretaryStudents(pageIndex, itemPerPage);
                return Ok(secretary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult UpdateStudentRoles(StudentRoleDTO req)
        {
            try
            {
                var result = userService.UpdateStudentRole(req);
                return Ok("Update successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
