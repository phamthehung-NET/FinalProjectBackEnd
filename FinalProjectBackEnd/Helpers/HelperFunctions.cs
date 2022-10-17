using FinalProjectBackEnd.Models.CommonModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FinalProjectBackEnd.Helpers
{
    public class HelperFunctions
    {
        public static string handleUserName(string fullName, DateTime? DoB, int? schoolyear)
        {
            string name = fullName.Split(' ').Last().ToLower();
            string userName = name + DoB.Value.Day + DoB.Value.Month + DoB.Value.Year + "_" + schoolyear.ToString().Substring(schoolyear.ToString().Length -2);
            return userName;
        }

        public static Pagination<T> GetPaging<T>(int? pageIndex, int? itemPerPage, List<T> items)
        {
            var count = items.Count();
            if (pageIndex > count)
            {
                pageIndex = count;
            }
            var pagingItems = items.Skip((int)((pageIndex - 1) * itemPerPage)).Take((int)itemPerPage).ToList();

            return new Pagination<T>(count, pageIndex, itemPerPage, pagingItems);
        }

        public static string UploadFile(IFormFile file, string directory)
        {
            FileInfo rootpath = new FileInfo(@"wwwroot\" + directory);

            if (!Directory.Exists(rootpath.FullName))
            {
                Directory.CreateDirectory(rootpath.FullName);
            }
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtendsion = Path.GetExtension(file.FileName);
                    string newFileName = fileName + DateTime.Now.ToString("ddMMyyyyHms") + fileExtendsion;
                    var fileSavePath = Path.Combine(rootpath.FullName, newFileName);

                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return directory + newFileName;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string UploadBase64File(string file, string fileName, string directory)
        {
            FileInfo rootpath = new FileInfo(@"wwwroot\" + directory);

            if (!Directory.Exists(rootpath.FullName))
            {
                Directory.CreateDirectory(rootpath.FullName);
            }
            try
            {
                byte[] imageBytes = Convert.FromBase64String(file);
                if (file != null && file.Length > 0)
                {
                    var fileNameWE = Path.GetFileNameWithoutExtension(fileName);
                    var fileExtention = Path.GetExtension(fileName);
                    string newFileName = fileNameWE + DateTime.Now.ToString("ddMMyyyyHms") + fileExtention;
                    var fileSavePath = Path.Combine(rootpath.FullName, newFileName);

                    using (var imageFile = new FileStream(fileSavePath, FileMode.Create))
                    {
                        imageFile.Write(imageBytes, 0, imageBytes.Length);
                        imageFile.Flush();
                    }
                    return "/" + directory + "/" + newFileName;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void SendMail(MailModel mail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(MailConfiguration.SystemEmail));
            mail.ArrivalEmails.ForEach(mail =>
            {
                email.To.Add(MailboxAddress.Parse(mail));
            });
            email.Subject = mail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(MailConfiguration.StmpConfig, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(MailConfiguration.UserName, MailConfiguration.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
