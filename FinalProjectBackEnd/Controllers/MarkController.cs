using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService markService;

        public MarkController(IMarkService _markService)
        {
            markService = _markService;
        }

        [HttpGet]
        public ActionResult GetAllMarks(int? sy, int? month, int? classId, int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;
            sy = sy ?? DateTime.Now.Year;
            month = month ?? DateTime.Now.Month;

            try
            {
                var mark = markService.GetAllMarks(sy, month, classId, pageIndex, itemPerPage);
                return Ok(mark);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult AddMark()
        {
            try
            {
                markService.AddMark();
                return Ok("Add mark successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult EditMark(MarkDTO markReq)
        {
            try
            {
                markService.EditMark(markReq);
                return Ok("Add mark successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetMarkDetail(int id)
        {
            try
            {
                var mark = markService.GetMarkDetail(id);
                return Ok(mark.FirstOrDefault());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult WarningPost(PostMarkDTO req)
        {
            try
            {
                var mark = markService.WarningPost(req);
                return Ok("Warning Post success");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public ActionResult DeleteMarkHistory(int id)
        {
            try
            {
                var mark = markService.DeleteMarkHistory(id);
                return Ok("Delete Mark History success");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetClassBySchoolYear(int? sy)
        {
            sy = sy ?? DateTime.Now.Year;
            try
            {
                var classrooms = markService.GetClassBySchoolYear(sy);
                return Ok(classrooms.ToList());
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
