namespace FinalProjectBackEnd.Helpers
{
    public class Roles
    {
        public static string Student = "Student";

        public static int StudentRoleId = 3;

        public static string Teacher = "Teacher";

        public static int TeacherRoleId = 2;

    }

    public class Account
    {
        public static string DefaultPassword = "Abc@12345";
    }

    public class StudentStatus
    {
        public static int Learning = 0;

        public static int Graduated = 1;

        public static int DroppedOut = 2;
    }

    public class StudentRole
    {
        public static int Normal = 0;

        public static int Monitor = 1;

        public static int Secretary = 2;
    }

    public class ImageDirectories
    {
        public static string Student = "Images/StudentAvatars";

        public static string Teacher = "Images/TeacherAvatars";

        public static string Post = "Images/Posts";
    }

    public class NotificationLinks
    {
        public static string PostDetail = "/Post/GetPostDetail?id=";

        public static string CommentDetail = "/Comment/GetCommentDetail?id=";
    }
}
