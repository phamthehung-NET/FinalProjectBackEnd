using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IClassService classService;

        public ClassController(IClassService _classService)
        {
            classService = _classService;
        }

        [HttpGet]
        public ActionResult GetAllClass(string keyword, int? pageIndex, int? itemPerPage, int? sy, int? grade)
        {
            keyword = keyword ?? "";
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;
            sy = sy ?? DateTime.Now.Year;
            grade = grade ?? 10;

            try
            {
                var classes = classService.GetAllClasses(keyword, pageIndex, itemPerPage, sy, grade);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult AddClass(ClassDTO classDTO)
        {
            try
            {
                var result = classService.AddClass(classDTO);
                return Ok("Add new Class successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> EditClass(ClassDTO classReq)
        {
            try
            {
                var result = await classService.EditClass(classReq);
                return Ok("Edit successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetClassDetail(int id)
        {
            try
            {
                var classData = classService.GetClassDetail(id);
                return Ok(classData.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet]
        public ActionResult GetTeacherSubject()
        {
            try
            {
                var subjects = classService.GetTeacherSubject();
                return Ok(subjects.ToList());
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpGet]
        public ActionResult GetHomeRoomTeacher(int? id)
        {
            try
            {
                var teachers = classService.GetHomeRoomTeacher(id);
                return Ok(teachers.ToList());
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpGet]
        public ActionResult GetStudentForClass(int? sy, int? id)
        {
            try
            {
                var students = classService.GetStudentForClass(sy, id);
                return Ok(students.ToList());
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        [HttpDelete]
        public ActionResult DeleteClass(int id)
        {
            try
            {
                classService.DeleteClass(id);
                return Ok("Delete Class succeessfully");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
