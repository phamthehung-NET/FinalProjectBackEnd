using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        public PostController(IPostService _postService)
        {
            postService = _postService;
        }

        [HttpGet]
        public ActionResult GetAllPosts(string keyword, int? pageIndex, int? itemPerPage)
        {
            keyword = keyword ?? "";
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var posts = postService.GetAllPosts(keyword, pageIndex, itemPerPage);
                return Ok(posts.Items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult AddPost(PostDTO req)
        {
            try
            {
                var result = postService.AddPost(req);
                return Ok("Add Post Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult EditPost(PostDTO req)
        {
            try
            {
                var result = postService.EditPost(req);
                return Ok("Edit successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public ActionResult DeletePost(int id)
        {
            try
            {
                postService.DeletePost(id);
                return Ok("Delete post successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetPostDetail(int id)
        {
            try
            {
                var post = postService.GetPostDetail(id);
                return Ok(post.FirstOrDefault());
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult LikePost(UserLikePostDTO userLikePostReq)
        {
            try
            {
                postService.UserLikeAndDisLike(userLikePostReq);
                return Ok("Like Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetUserLikePosts(int id)
        {
            try
            {
                var userLikePost = postService.GetAllUserLikePost(id);
                return Ok(userLikePost);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetAllComments(int id)
        {
            try
            {
                var comments = postService.GetAllComments(id);
                return Ok(comments);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
