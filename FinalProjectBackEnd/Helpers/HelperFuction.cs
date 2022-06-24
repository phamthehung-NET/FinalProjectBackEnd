using FinalProjectBackEnd.Helpers;

namespace FinalProjectBackEnd.Controllers
{
    public class HelperFuction
    {
        public static string handleUserName(string fullName, int? schoolyear)
        {
            string name = fullName.Split(' ').Last().ToLower();
            string userName = name + schoolyear.ToString().Substring(schoolyear.ToString().Length -2);
            return userName;
        }

        public static Pagination<T> GetPaging<T>(int? pageIndex, int? itemPerPage, List<T> items)
        {
            var pagingItems = items.Skip((int)((pageIndex - 1) * itemPerPage)).Take((int)itemPerPage).ToList();

            return new Pagination<T>(items.Count(), pageIndex, itemPerPage, pagingItems);
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

            byte[] imageBytes = Convert.FromBase64String(file);

            if (!Directory.Exists(rootpath.FullName))
            {
                Directory.CreateDirectory(rootpath.FullName);
            }
            try
            {
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
    }
}
