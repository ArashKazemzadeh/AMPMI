using AQS_Common.Enums;

namespace AQS_Aplication.Dtos
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; set; }
        public LoginOutPutMessegeEnum Message { get; set; }
        public long UserId { get; set; }
        public string? Role { get; set; }
    }

}
