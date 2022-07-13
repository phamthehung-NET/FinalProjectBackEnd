using FinalProjectBackEnd.Controllers;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository userRepository;

        public SubjectRepository(ApplicationDbContext _context, IUserRepository _userRepository)
        {
            context = _context;
            userRepository = _userRepository;
        }

        public bool AddSubject(SubjectDTO subject)
        {
            var newSubject = new Subject
            {
                Name = subject.Name,
            };
            context.Subjects.Add(newSubject);
            context.SaveChanges();

            subject.TeacherIds.ForEach(x =>
            {
                var teacherSubject = new TeacherSubject
                {
                    SubjectId = newSubject.Id,
                    TeacherId = x,
                };
                context.TeacherSubjects.Add(teacherSubject);
            });

            context.SaveChanges();

            if (newSubject.Id != 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckNameExisted(SubjectDTO subject)
        {
            var subjectDb = context.Subjects.FirstOrDefault(x => x.Name == subject.Name);
            if (subjectDb != null)
            {
                if (subject.Id != null && subjectDb.Id == subject.Id)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool DeleteSubject(int id)
        {
            var subjectDb = context.Subjects.FirstOrDefault(x => x.Id == id);
            var teacherSubjects = GetTeacherSubjectsBySubject(id);
            if (subjectDb != null)
            {
                context.Subjects.Remove(subjectDb);
                context.TeacherSubjects.RemoveRange(teacherSubjects);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditSubject(SubjectDTO subject)
        {
            var subjectDb = context.Subjects.FirstOrDefault(x => x.Id == subject.Id);
            var teacherSubjects = GetTeacherSubjectsBySubject(subject.Id);
            if (subjectDb != null)
            {
                subjectDb.Name = subject.Name;
                if (!teacherSubjects.Any())
                {
                    subject.TeacherIds.ForEach(x =>
                    {
                        var teacherSubject = new TeacherSubject
                        {
                            SubjectId = subject.Id,
                            TeacherId = x,
                        };
                        context.TeacherSubjects.Add(teacherSubject);
                    });
                }
                else
                {
                    context.TeacherSubjects.RemoveRange(teacherSubjects);
                    subject.TeacherIds.ForEach(x =>
                    {
                        var teacherSubject = new TeacherSubject
                        {
                            SubjectId = subject.Id,
                            TeacherId = x,
                        };
                        context.TeacherSubjects.Add(teacherSubject);
                    });
                }
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Pagination<SubjectDTO> GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage)
        {
            var subjects = context.Subjects.Select(x => new SubjectDTO
            {
                Id = x.Id,
                Name = x.Name,
                TeacherIds = context.TeacherSubjects.Where(z => z.SubjectId == x.Id).Select(z => z.TeacherId).ToList(),
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                subjects = subjects.Where(x => x.Name.Contains(keyword));
            }

            var paginationItems = HelperFuction.GetPaging<SubjectDTO>(pageIndex, itemPerPage, subjects.ToList());

            return paginationItems;
        }

        public IQueryable<SubjectDTO> GetSubjectDetail(int id)
        {
            var subject = context.Subjects.Where(x => x.Id == id).Select(x => new SubjectDTO
            {
                Id = x.Id,
                Name = x.Name,
                Teacher = (from ts in context.TeacherSubjects
                          join ui in context.UserInfos on ts.TeacherId equals ui.UserId
                          where ts.SubjectId == id
                          select new
                          {
                              Id= ui.UserId,
                              Name = ui.FullName,
                          }).ToList(),
            });

            return subject;
        }

        public IQueryable<UserDTO> GetTeacherForSubject()
        {
            var teacher = userRepository.GetUSerWithRole(Roles.Teacher, null, null);
            var teacherSubject = context.TeacherSubjects.Select(x => x.TeacherId).ToList();

            teacher = teacher.Where(x => !teacherSubject.Contains(x.Id));
            return teacher;
        }

        public bool RemoveTeacherFromSubject(TeacherSubjectDTO teacherSubject)
        {
            var obj = context.TeacherSubjects.FirstOrDefault(x => x.SubjectId == teacherSubject.SubjectId && x.TeacherId == teacherSubject.TeacherId);
            if (obj != null)
            {
                context.TeacherSubjects.Remove(obj);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        private IQueryable<TeacherSubject> GetTeacherSubjectsBySubject(int? subjectId)
        {
            var teacherSubjects = context.TeacherSubjects.Where(x => x.SubjectId == subjectId);
            return teacherSubjects;
        }
    }
}
