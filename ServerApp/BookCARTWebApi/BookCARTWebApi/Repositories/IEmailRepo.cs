namespace BookCARTWebApi.Repositories
{
    public interface IEmailRepo
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
