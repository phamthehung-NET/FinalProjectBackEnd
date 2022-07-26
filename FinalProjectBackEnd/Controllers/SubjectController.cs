using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService _subjectService)
        {
            subjectService = _subjectService;
        }

        [HttpGet]
        public ActionResult GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage)
        {
            keyword = keyword ?? "";
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var subjects = subjectService.GetAllSubjects(keyword, pageIndex, itemPerPage);

                return Ok(subjects.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetSubjectDetail(int id)
        {
            try
            {
                var subject = subjectService.GetSubjectDetail(id);
                return Ok(subject.FirstOrDefault());
            }
            catch(Exception e)
            {
                return BadRequest(e);
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
                return BadRequest(e);
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
                return BadRequest(e);
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
                return BadRequest(e);
            }
        }
    }
}
