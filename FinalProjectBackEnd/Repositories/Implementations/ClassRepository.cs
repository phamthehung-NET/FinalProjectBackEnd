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
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<CustomUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;

        public ClassRepository(ApplicationDbContext _context, UserManager<CustomUser> _userManager, IHttpContextAccessor _httpContextAccessor, IUserRepository _userRepository)
        {
            context = _context;
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
            userRepository = _userRepository;
        }

        public bool AddClass(ClassDTO classReq)
        {
            var classroom = new Classroom
            {
                Name = classReq.ClassName,
                Grade = classReq.Grade,
                CreatedAt = DateTime.Now,
                SchoolYear = classReq.SchoolYear,
                HomeroomTeacher = classReq.HomeRoomTeacherId,
            };
            context.Classrooms.Add(classroom);
            context.SaveChanges();
            if (classroom.Id != 0)
            {
                classReq.TeacherSubjects.ToList().ForEach(ts =>
                {
                    var tsdb = context.TeacherSubjects.FirstOrDefault(x => x.SubjectId == ts.SubjectId && x.TeacherId.Equals(ts.TeacherId));
                    var cts = new ClassTeacherSubject
                    {
                        ClassId = classroom.Id,
                        TeacherSubjectId = tsdb.Id,
                    };
                    context.ClassTeacherSubjects.Add(cts);
                });
                classReq.Students.ToList().ForEach(student =>
                {
                    var sc = new StudentClass
                    {
                        StudentId = student.Id,
                        ClassId = classroom.Id,
                    };
                    context.StudentClasses.Add(sc);
                });
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteClass(int id)
        {
            var classroom = context.Classrooms.FirstOrDefault(x => x.Id == id);
            var studentClass = context.StudentClasses.Where(x => x.ClassId == id);
            var teacherSubject = context.ClassTeacherSubjects.Where(x => x.ClassId == id);
            if (classroom != null)
            {
                context.Classrooms.Remove(classroom);
                context.RemoveRange(studentClass);
                context.RemoveRange(teacherSubject);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> EditClass(ClassDTO classReq)
        {
            var editor = await userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name);
            var classDb = context.Classrooms.FirstOrDefault(x => x.Id == classReq.Id);
            if (classDb != null)
            {
                var classTeacherSubjects = context.ClassTeacherSubjects.Where(x => x.ClassId == classReq.Id);
                var studentClasses = context.StudentClasses.Where(x => x.ClassId == classReq.Id);
                classDb.Name = classReq.ClassName;
                classDb.SchoolYear = classReq.SchoolYear;
                classDb.Grade = classReq.Grade;
                classDb.UpdatedAt = DateTime.Now;
                classDb.UpdatedBy = editor.Id;
                classDb.HomeroomTeacher = classReq.HomeRoomTeacherId;
                context.ClassTeacherSubjects.RemoveRange(classTeacherSubjects);
                context.StudentClasses.RemoveRange(studentClasses);
                classReq.TeacherSubjects.ToList().ForEach(ts =>
                {
                    var tsdb = context.TeacherSubjects.FirstOrDefault(x => x.SubjectId == ts.SubjectId && x.TeacherId.Equals(ts.TeacherId));
                    var cts = new ClassTeacherSubject
                    {
                        ClassId = classReq.Id,
                        TeacherSubjectId = tsdb.Id,
                    };
                    context.ClassTeacherSubjects.Add(cts);
                });
                classReq.Students.ToList().ForEach(student =>
                {
                    var sc = new StudentClass
                    {
                        StudentId = student.Id,
                        ClassId = classReq.Id,
                    };
                    context.StudentClasses.Add(sc);
                });
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Pagination<ClassDTO> GetAllClasses(string keyword, int? pageIndex, int? itemPerPage, int? sy, int? grade)
        {
            var classes = GetClassData(null);
            if (!String.IsNullOrEmpty(keyword))
            {
                classes = classes.Where(x => x.ClassName.Contains(keyword));
            }
            if (sy != null)
            {
                classes = classes.Where(x => x.SchoolYear == sy);
            }
            if (grade != null)
            {
                classes = classes.Where(x => x.Grade == grade);
            }

            var pagination = HelperFunctions.GetPaging<ClassDTO>(pageIndex, itemPerPage, classes.ToList());

            return pagination;
        }

        public IQueryable<ClassDTO> GetClassDetail(int? id)
        {
            var classData = GetClassData(id);
            return classData;
        }

        public IQueryable<dynamic> GetStudentForClass(int? sy, int? id)
        {
            var studentInClass = context.StudentClasses.Select(x => x.StudentId);
            var students = userRepository.GetUSerWithRole(Roles.Student, null, null)
                .Where(x => x.SchoolYear == sy)
                .Select(x => new { x.Id, x.FullName });
            if (studentInClass.Any())
            {
                if (id != null)
                {
                    var classStudent = context.StudentClasses.Where(x => x.ClassId == id).Select(x => x.StudentId).ToList();
                    students = students.Where(x => !studentInClass.Contains(x.Id) || classStudent.Contains(x.Id));
                }
                else
                {
                    students = students.Where(x => !studentInClass.Contains(x.Id));
                }
            }
            return students;
        }

        public IQueryable<SubjectDTO> GetTeacherSubject()
        {
            var subjects = (from s in context.Subjects
                            join ts in context.TeacherSubjects on s.Id equals ts.SubjectId
                            join t in context.UserInfos on ts.TeacherId equals t.UserId
                            select new
                            {
                                SubjectId = ts.SubjectId,
                                SubjectName = s.Name,
                                TeacherName = t.FullName,
                                TeacherId = t.UserId,
                            }).GroupBy(x => new { x.SubjectId, x.SubjectName })
                          .Select(x => new SubjectDTO
                          {
                              Id = x.Key.SubjectId,
                              Name = x.Key.SubjectName,
                              Teacher = x.Where(x => !String.IsNullOrEmpty(x.TeacherId)).Select(x => new { Id = x.TeacherId, FullName = x.TeacherName })
                          });
            return subjects;
        }

        public bool IsClassExisted(ClassDTO classReq)
        {
            var classDb = context.Classrooms.FirstOrDefault(x => x.Name.Equals(classReq.ClassName) && x.SchoolYear == classReq.SchoolYear && x.Grade == classReq.Grade);
            if (classDb != null)
            {
                if (classReq.Id != null && classDb.Id == classReq.Id)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public IQueryable<dynamic> GetHoomeRoomTeacher(int? id)
        {
            IQueryable<dynamic> teacher;
            var teacherInClass = context.Classrooms.Select(x => x.HomeroomTeacher);
            if (id != null)
            {
                var classTeacher = context.Classrooms.Select(x => new { Id = x.Id, HomeRoomTeacher = x.HomeroomTeacher })
                    .FirstOrDefault(x => x.Id == id);
                teacher = userRepository.GetUSerWithRole(Roles.Teacher, null, null)
                    .Where(x => !teacherInClass.Contains(x.Id) || x.Id == classTeacher.HomeRoomTeacher)
                    .Select(x => new { x.Id, x.FullName });
            }
            else
            {
                teacher = userRepository.GetUSerWithRole(Roles.Teacher, null, null)
                    .Where(x => !teacherInClass.Contains(x.Id))
                    .Select(x => new { x.Id, x.FullName });
            }

            return teacher;
        }

        private IQueryable<ClassDTO> GetClassData(int? id)
        {
            var classes = (from c in context.Classrooms
                           join ui in context.UserInfos on c.HomeroomTeacher equals ui.UserId into homeRoomTeacher
                           from ui in homeRoomTeacher.DefaultIfEmpty()
                           join cts in context.ClassTeacherSubjects on c.Id equals cts.ClassId into classTeacherSubjects
                           from cts in classTeacherSubjects.DefaultIfEmpty()
                           join ts in context.TeacherSubjects on cts.TeacherSubjectId equals ts.Id into teacherSubjects
                           from ts in teacherSubjects.DefaultIfEmpty()
                           join tc in context.UserInfos on ts.TeacherId equals tc.UserId into teachersInSubject
                           from tc in teachersInSubject.DefaultIfEmpty()
                           join s in context.Subjects on ts.SubjectId equals s.Id into subjects
                           from s in subjects.DefaultIfEmpty()
                           join sc in context.StudentClasses on c.Id equals sc.ClassId into studentClasses
                           from sc in studentClasses.DefaultIfEmpty()
                           join st in context.UserInfos on sc.StudentId equals st.UserId into students
                           from st in students.DefaultIfEmpty()
                           join su in context.Users on sc.StudentId equals su.Id into studentAccounts
                           from su in studentAccounts.DefaultIfEmpty()
                           join sui in context.UserInfos on su.Id equals sui.UserId
                           select new
                           {
                               Id = c.Id,
                               ClassName = c.Name,
                               HomeRoomTeacherName = ui.FullName,
                               HomeRoomTeacherId = ui.UserId,
                               Grade = c.Grade,
                               CreatedAt = c.CreatedAt,
                               SchoolYear = c.SchoolYear,
                               UpdatedAt = c.UpdatedAt,
                               UpdatedBy = c.UpdatedBy,
                               TeacherId = tc.UserId,
                               TeacherName = tc.FullName,
                               SubjectId = s.Id,
                               SubjectName = s.Name,
                               StudentName = sui.FullName,
                               User = su,
                               Dob = ui.DoB,
                               Status = ui.Status,
                           }).GroupBy(x => new { x.Id, x.ClassName, x.HomeRoomTeacherName, x.HomeRoomTeacherId, x.Grade, x.CreatedAt, x.SchoolYear, x.UpdatedAt, x.UpdatedBy })
                            .Select(x => new ClassDTO
                            {
                                Id = x.Key.Id,
                                ClassName = x.Key.ClassName,
                                HomeRoomTeacherName = x.Key.HomeRoomTeacherName,
                                HomeRoomTeacherId = x.Key.HomeRoomTeacherId,
                                Grade = x.Key.Grade,
                                CreatedAt = x.Key.CreatedAt,
                                SchoolYear = x.Key.SchoolYear,
                                UpdatedAt = x.Key.UpdatedAt,
                                UpdatedBy = x.Key.UpdatedBy,
                                TeacherSubjects = x.Select(j => new TeacherSubjectDTO
                                {
                                    SubjectId = j.SubjectId,
                                    SubjectName = j.SubjectName,
                                    TeacherId = j.TeacherId,
                                    TeacherName = j.TeacherName
                                }).Distinct(),
                                Students = x.Where(x => !String.IsNullOrEmpty(x.User.Id)).Select(j => new UserDTO
                                {
                                    Id = j.User.Id,
                                    UserName = j.User.UserName,
                                    DoB = j.Dob,
                                    Status = j.Status,
                                    FullName = j.StudentName
                                }).Distinct()
                            });
            if (id != null)
            {
                classes = classes.Where(x => x.Id == id);
            }
            return classes;
        }

        public bool UpGradeClass(int classId)
        {
            var classroom = context.Classrooms.FirstOrDefault(x => x.Id == classId);
            var classNameArray = classroom.Name.ToCharArray();
            string classExtension;
            if (classNameArray.Length == 4)
            {
                classExtension = classNameArray[classNameArray.Length - 2].ToString() + classNameArray[classNameArray.Length - 1].ToString();
            }
            else
            {
                classExtension = classNameArray[classNameArray.Length - 3].ToString() + classNameArray[classNameArray.Length - 2].ToString() + classNameArray[classNameArray.Length - 1].ToString();
            }
            var newGrade = classroom.Grade + 1;
            var newName = newGrade + classExtension;
            var classTs = from cts in context.ClassTeacherSubjects
                                where cts.ClassId == classroom.Id
                                select new ClassTeacherSubject
                                {
                                    Id = cts.Id,
                                    ClassId = cts.ClassId,
                                    TeacherSubjectId = cts.TeacherSubjectId,
                                };
            var studentClass = context.StudentClasses.Where(x => x.ClassId == classroom.Id).Select(x => x.StudentId);
            if (classTs != null || studentClass != null)
            {
                var newClass = new Classroom()
                {
                    Name = newName,
                    Grade = newGrade,
                    SchoolYear = classroom.SchoolYear + 1,
                    HomeroomTeacher = classroom.HomeroomTeacher,
                    CreatedAt = DateTime.Now,
                };
                var currentClass = context.Classrooms.Where(x => x.Name.Equals(newClass.Name) && x.SchoolYear == newClass.SchoolYear).FirstOrDefault();
                if (classroom.Grade < 12 && classroom.SchoolYear < DateTime.Now.Year && currentClass == null)
                {
                    context.Classrooms.Add(newClass);
                    context.SaveChanges();
                    foreach (var student in studentClass)
                    {
                        var newStudentClass = new StudentClass()
                        {
                            ClassId = newClass.Id,
                            StudentId = student,
                        };
                        context.StudentClasses.Add(newStudentClass);
                    }
                    foreach (var teacherSubject in classTs)
                    {
                        var newClassTeacherSubject = new ClassTeacherSubject()
                        {
                            ClassId = newClass.Id,
                            TeacherSubjectId = teacherSubject.TeacherSubjectId,
                        };
                        context.ClassTeacherSubjects.Add(newClassTeacherSubject);
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
