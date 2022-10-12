namespace TheBlogProject.ViewModels

#nullable disable
{
    public class MailSettings
    {
        //So we can configure and use an smtp server
        //from Google for example
        public string Mail { get; set; }
        public string DisplaName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
