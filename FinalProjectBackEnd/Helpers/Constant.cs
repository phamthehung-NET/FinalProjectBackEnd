namespace FinalProjectBackEnd.Helpers
{
    public class Roles
    {
        public static string Student = "Student";

        public static int StudentRoleId = 3;

        public static string Teacher = "Teacher";

        public static int TeacherRoleId = 2;

        public static string Admin = "Admin";

        public static int AdminRoleId = 1;
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

        public static string Admin = "Images/AdminAvatars";

        public static string Post = "Images/Posts";
    }

    public class NotificationLinks
    {
        public static string PostDetail = "/Post/GetPostDetail?id=";

        public static string CommentDetail = "/Comment/GetCommentDetail?id=";
    }

    public class NotificationTypes
    {
        public static int AddPost = 0;

        public static int LikePost = 1;

        public static int CommentPost = 2;

        public static int LikeComment = 3;

        public static int ReplyComment = 4;
    }

    public class PostVisibility
    {
        public static bool Private = false;
        
        public static bool Public = true;
    }

    public class LikeStatus
    {
        public static int Like = 0;

        public static int Love = 1;

        public static int Haha = 2;

        public static int Wow = 3;

        public static int Sad = 4;

        public static int Angry = 5;
    }

    public class Priority
    {
        public static int Bad = 0;

        public static int VeryBad = 1;
    }
}
