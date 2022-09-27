using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService _commentService)
        {
            commentService = _commentService;
        }

        [HttpPost]
        public async Task<ActionResult> CommentPost(CommentDTO commentReq)
        {
            try
            {
                await commentService.CommentPost(commentReq);
                return Ok("Comment post successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult EditComment(CommentDTO commentReq)
        {
            try
            {
                commentService.EditComment(commentReq);
                return Ok("Edit comment successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                commentService.DeleteComment(id);
                return Ok("Delete comment successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCommentDetail(int id)
        {
            try
            {
                var comment = await commentService.GetCommentDetail(id);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult LikeComment(UserLikeCommentDTO userLikeCommentReq)
        {
            try
            {
                commentService.UserLikeAndDisLikeComment(userLikeCommentReq);
                return Ok("Like Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult ReplyComment(ReplyCommentDTO replyCommentReq)
        {
            try
            {
                commentService.ReplyComment(replyCommentReq);
                return Ok("Reply comment successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult EditReplyComment(ReplyCommentDTO replyCommentReq)
        {
            try
            {
                commentService.EditReplyComment(replyCommentReq);
                return Ok("Edit reply comment successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public ActionResult DeleteReplyComment(int id)
        {
            try
            {
                commentService.DeleteReplyComment(id);
                return Ok("Delete reply comment successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetAllCommentLike(int id)
        {
            try
            {
                var like = commentService.GetAllCommentLike(id);
                return Ok(like.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
