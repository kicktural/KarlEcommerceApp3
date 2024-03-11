namespace WebUI.Helperservices
{
	public interface IEmailService
	{
		Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
	}
}
