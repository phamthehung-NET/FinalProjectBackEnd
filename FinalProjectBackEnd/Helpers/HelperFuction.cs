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
    }
}
