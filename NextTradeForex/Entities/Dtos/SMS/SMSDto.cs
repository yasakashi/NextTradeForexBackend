namespace Entities.Dtos
{
    public class SMSDto
    {
        public string? sourcenumber { get; set; }
        public string? distinationnumber { get; set; }
        public string? smsbody { get; set; }
    }

    public class SMSRegisterDto
    {
        public string mobile { get; set; }
    }


}