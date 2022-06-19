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
    }
}
