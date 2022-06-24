using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService _subjectService)
        {
            subjectService = _subjectService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SubjectDTO>> GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var subjects = subjectService.GetAllSubjects(keyword, pageIndex, itemPerPage);

                return Ok(new { subjects = subjects });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddSubject(SubjectDTO subject)
        {
            try
            {
                var result = subjectService.AddSubject(subject);
                return Ok("Add new Subject successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult EditSubject(SubjectDTO subject)
        {
            try
            {
                var result = subjectService.EditSubject(subject);
                return Ok("Edit Subject successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult GetSubjectDetail(int id)
        {
            try
            {
                var subject = subjectService.GetSubjectDetail(id);
                return Ok(new { subject = subject.FirstOrDefault() });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteSubject(int id)
        {
            try
            {
                var result = subjectService.DeleteSubject(id);
                return Ok("Delete Subject successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult GetTeacherForSubject()
        {
            try
            {
                var teacher = subjectService.GetTeacherForSubject();
                return Ok(teacher);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult RemoveTeacherFromSubject(TeacherSubjectDTO teacherSubject)
        {
            try
            {
                var result = subjectService.RemoveTeacherFromSubject(teacherSubject);
                return Ok("Remove Teacher successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
