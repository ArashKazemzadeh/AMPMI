namespace AQS_Aplication.Dtos.IdentityServiceDto
{
    public class LoginResultDto
    {
        public LoginOutPutMessegeEnum Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Role { get; set; }
        public long UserId { get; set; }

    }
    public enum LoginOutPutMessegeEnum
    {
        UserNotFound,
        Invalid,
        LockedOut,
        LoginSuccessful
    }
}
