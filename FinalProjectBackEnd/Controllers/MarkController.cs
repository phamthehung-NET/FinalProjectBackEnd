using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService markService;

        public MarkController(IMarkService _markService)
        {
            markService = _markService;
        }

        [HttpGet]
        public ActionResult GetAllMarks(int? from, int? to, int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;
            try
            {
                var mark = markService.GetAllMarks(from, to, pageIndex, itemPerPage);
                return Ok(mark.Items);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult AddMark(MarkDTO markReq)
        {
            try
            {
                markService.AddMark(markReq);
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

        [HttpGet]
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
    }
}
