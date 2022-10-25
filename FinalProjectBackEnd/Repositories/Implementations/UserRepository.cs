using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Controllers;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserRepository(UserManager<CustomUser> _userManager, ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            userManager = _userManager;
            context = _context;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<bool> AddStudent(UserDTO userDTO)
        {
            string userName = HelperFunctions.handleUserName(userDTO.FullName, userDTO.DoB, userDTO.SchoolYear);
            var account = new CustomUser
            {
                Email = userDTO.Email,
                UserName = userName,
                PhoneNumber = userDTO.PhoneNumber,
            };
            var result = await userManager.CreateAsync(account, Account.DefaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(account, Roles.Student);
                var userInfo = new UserInfo
                {
                    UserId = account.Id,
                    FullName = userDTO.FullName,
                    DoB = userDTO.DoB,
                    SchoolYear = userDTO.SchoolYear,
                    GraduateYear = userDTO.GraduateYear,
                    Avatar = HelperFunctions.UploadBase64File(userDTO.Avatar, userDTO.FileName, ImageDirectories.Student),
                    Status = StudentStatus.Learning,
                    StudentRole = StudentRole.Normal,
                    IsFirstLogin = true
                };
                context.UserInfos.Add(userInfo);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> AddTeacher(UserDTO userDTO)
        {
            var account = new CustomUser
            {
                Email = userDTO.Email,
                UserName = userDTO.UserName,
                PhoneNumber = userDTO.PhoneNumber,
            };
            var result = await userManager.CreateAsync(account, Account.DefaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(account, Roles.Teacher);
                var userInfo = new UserInfo
                {
                    UserId = account.Id,
                    FullName = userDTO.FullName,
                    DoB = userDTO.DoB,
                    Address = userDTO.Address,
                    StartDate = userDTO.StartDate,
                    EndDate = userDTO.EndDate,
                    Avatar = HelperFunctions.UploadBase64File(userDTO.Avatar, userDTO.FileName, ImageDirectories.Teacher),
                    IsDeleted = false,
                    IsFirstLogin = true
                };
                context.UserInfos.Add(userInfo);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteStudent(string id)
        {
            var student = context.UserInfos.FirstOrDefault(x => x.UserId == id);
            if (student != null)
            {
                student.Status = StudentStatus.DroppedOut;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteTeacher(string id)
        {
            var teacher = context.UserInfos.FirstOrDefault(x => x.UserId == id);
            if (teacher != null)
            {
                teacher.IsDeleted = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditStudent(UserDTO userDTO)
        {
            if(CheckUserRole(userDTO.Id, Roles.StudentRoleId))
            {
                var student = context.Users.FirstOrDefault(x => x.Id == userDTO.Id);
                var studentInfo = context.UserInfos.FirstOrDefault(x => x.UserId == userDTO.Id);
                if (student != null && studentInfo != null)
                {
                    student.Email = userDTO.Email;
                    student.PhoneNumber = userDTO.PhoneNumber;
                    studentInfo.DoB = userDTO.DoB != null ? userDTO.DoB : studentInfo.DoB;
                    studentInfo.GraduateYear = userDTO.GraduateYear;
                    studentInfo.Status = userDTO.Status;
                    studentInfo.Avatar = userDTO.Avatar != null ? HelperFunctions.UploadBase64File(userDTO.Avatar, userDTO.FileName, ImageDirectories.Student) : studentInfo.Avatar;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool EditTeacher(UserDTO userDTO)
        {
            if(CheckUserRole(userDTO.Id, Roles.TeacherRoleId))
            {
                var teacher = context.Users.FirstOrDefault(x => x.Id == userDTO.Id);
                var teacherInfo = context.UserInfos.FirstOrDefault(x => x.UserId == userDTO.Id);
                if (teacher != null && teacherInfo != null)
                {
                    teacher.Email = userDTO.Email;
                    teacher.PhoneNumber = userDTO.PhoneNumber;
                    teacherInfo.DoB = userDTO.DoB != null ? userDTO.DoB : teacherInfo.DoB;
                    teacherInfo.Address = userDTO.Address;
                    teacherInfo.StartDate = userDTO.StartDate != null ? userDTO.StartDate : teacherInfo.StartDate;
                    teacherInfo.EndDate = userDTO.EndDate != null ? userDTO.EndDate : teacherInfo.EndDate;
                    teacherInfo.Avatar = userDTO.Avatar != null ? HelperFunctions.UploadBase64File(userDTO.Avatar, userDTO.FileName, ImageDirectories.Teacher): teacherInfo.Avatar;
                    teacherInfo.IsDeleted = userDTO.IsDeleted != null ? userDTO.IsDeleted : false;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public Pagination<UserDTO> GetAllStudents(string keyword, int? filter, int? sy, int? padeIndex, int? itemPerPage)
        {
            var students = GetUSerWithRole(Roles.Student, null, null);
            if (!String.IsNullOrEmpty(keyword))
            {
                students = students.Where(x => x.FullName.Contains(keyword));
            }
            if(sy != null)
            {
                students = students.Where(x => x.SchoolYear == sy);
            }
            if (filter != null)
            {
                students = students.Where(x => x.Status == filter);
            }

            var paginateItem = HelperFunctions.GetPaging(padeIndex, itemPerPage, students.ToList());

            return paginateItem;
        }

        public Pagination<UserDTO> GetAllTeachers(string keyword, bool? filter, int? padeIndex, int? itemPerPage)
        {
            var teachers = GetUSerWithRole(Roles.Teacher, null, null);
            if (!String.IsNullOrEmpty(keyword))
            {
                teachers = teachers.Where(x => x.FullName.Contains(keyword));
            }
            if(filter != null)
            {
                teachers = teachers.Where(x => x.IsDeleted == filter);
            }

            var paginateItem = HelperFunctions.GetPaging<UserDTO>(padeIndex, itemPerPage, teachers.ToList());

            return paginateItem;
        }

        public IQueryable<UserDTO> GetStudentDetail(string id)
        {
            var student = GetUSerWithRole(Roles.Student, id, null);
            return student;
        }

        public IQueryable<UserDTO> GetTeacherDetail(string id)
        {
            var teacher = GetUSerWithRole(Roles.Teacher, id, null);
            return teacher;
        }

        public bool CheckUserNameExisted(string userName)
        {
            return context.Users.Any(x => x.UserName == userName);
        }

        public IQueryable<UserDTO> GetUSerWithRole(string role, string id, int? studentRole)
        {

            var users = from u in context.Users
                        join ur in context.UserRoles on u.Id equals ur.UserId
                        join r in context.Roles on ur.RoleId equals r.Id
                        join ui in context.UserInfos on u.Id equals ui.UserId
                        join sc in context.StudentClasses on u.Id equals sc.StudentId into studentClass
                        from sc in studentClass.DefaultIfEmpty()
                        join c in context.Classrooms on sc.ClassId equals c.Id into classroom
                        from c in classroom.DefaultIfEmpty()
                        select new UserDTO
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            FullName = ui.FullName,
                            Address = ui.Address,
                            DoB = ui.DoB,
                            PhoneNumber = u.PhoneNumber,
                            SchoolYear = ui.SchoolYear,
                            GraduateYear = ui.GraduateYear,
                            Avatar = ui.Avatar,
                            Status = ui.Status,
                            FollowerCount = context.UserFollows.Where(uf => uf.FollowerId.Equals(u.Id)).Count(),
                            FolloweeCount = context.UserFollows.Where(uf => uf.FolloweeId.Equals(u.Id)).Count(),
                            PostCount = context.Posts.Where(p => p.AuthorId.Equals(u.Id)).Count(),
                            StudentRole = ui.StudentRole,
                            StartDate = ui.StartDate,
                            EndDate = ui.EndDate,
                            IsDeleted = ui.IsDeleted,
                            ClassName = c.Name,
                            Role = r.Name,
                            IsFirstLogin = ui.IsFirstLogin,
                        };
            if (!String.IsNullOrEmpty(role))
            {
                users = users.Where(x => x.Role.Equals(role));
            }
            if (!String.IsNullOrEmpty(id))
            {
                users = users.Where(x => x.Id.Equals(id));
            }
            if(studentRole != null)
            {
                users = users.Where(x => x.StudentRole == studentRole);
            }
            return users;
        }

        public Pagination<UserDTO> GetAllSecretaryStudents(int? padeIndex, int? itemPerPage)
        {
            var secretary = GetUSerWithRole(Roles.Student, null, StudentRole.Secretary);

            var paginateItem = HelperFunctions.GetPaging<UserDTO>(padeIndex, itemPerPage, secretary.ToList());

            return paginateItem;
        }

        public Pagination<UserDTO> GetAllMonitorStudents(int? padeIndex, int? itemPerPage)
        {
            var monitor = GetUSerWithRole(Roles.Student, null, StudentRole.Monitor);

            var paginateItem = HelperFunctions.GetPaging<UserDTO>(padeIndex, itemPerPage, monitor.ToList());

            return paginateItem;
        }

        public bool UpdateStudentRole (StudentRoleDTO req)
        {
            if(CheckUserRole(req.Id, Roles.StudentRoleId))
            {
                var student = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(req.Id));
                if (student != null)
                {
                    student.StudentRole = req.Role;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool CheckUserRole(string userId, int roleId)
        {
            var user = context.UserRoles.Where(x => x.UserId.Equals(userId) && x.RoleId == roleId.ToString());
            return user.Any();
        }

        public bool UserUpdateProfile(UserDTO req)
        {
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId == user.Id);
            user.PhoneNumber = req.PhoneNumber != null ? req.PhoneNumber : user.PhoneNumber;
            userInfo.FullName = req.FullName != null ? req.FullName : userInfo.FullName;
            userInfo.Address = req.Address != null ? req.Address : userInfo.Address;
            userInfo.DoB = req.DoB != null ? req.DoB : userInfo.DoB;
            var status = context.SaveChanges();
            return status > 0;
        }

        public bool ChangeFirstLoginPassword(ChangePasswordModel req)
        {
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            var token = userManager.GeneratePasswordResetTokenAsync(user).Result;
            var result = userManager.ResetPasswordAsync(user, token, req.Password).Result;
            if (result.Succeeded)
            {
                var userInfo = context.UserInfos.FirstOrDefault(x => user.Id.Equals(x.UserId));
                userInfo.IsFirstLogin = false;
                context.SaveChanges();
                return true;
            }
            throw new Exception(result.Errors.First().Description);
        }

        public bool ChangeUserAvatar(UserDTO req)
        {
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            var userInfo = context.UserInfos.FirstOrDefault(x => user.Id.Equals(x.UserId));
            string Directory = "";
            switch (req.Role)
            {
                case "Teacher":
                    Directory = ImageDirectories.Teacher;
                    break;
                case "Student":
                    Directory = ImageDirectories.Teacher;
                    break;
                case "Admin":
                    Directory = ImageDirectories.Admin;
                    break;
                default:
                    break;
            }
            userInfo.Avatar = HelperFunctions.UploadBase64File(req.Avatar, req.FileName, Directory);
            var status = context.SaveChanges();

            return status > 0;
        }

        public bool FollowUser(string userId)
        {
            var currentUserId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));
            if(userId != null)
            {
                var userFollow = new UserFollow()
                {
                    FolloweeId = userId,
                    FollowerId = currentUserId,
                };
                context.UserFollows.Add(userFollow);
                context.SaveChanges();
                return userFollow.Id > 0 ? true : false;
            }
            return false;
        }

        public IQueryable<string> GetFollowedUser()
        {
            var currentUserId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            return context.UserFollows.Where(x => x.FollowerId.Equals(currentUserId)).Select(x => x.FolloweeId);
        }
    }
}
