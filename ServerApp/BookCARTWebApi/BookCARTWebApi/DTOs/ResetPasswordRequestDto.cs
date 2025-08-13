namespace BookCARTWebApi.DTOs
{
    public class ResetPasswordRequestDto
    {
        public int UserId { get; set; }
        public Guid Token { get; set; }
        public string NewPassword { get; set; }
    }
}
