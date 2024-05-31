using MVC.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace MVC.PL.Helper
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("abdelrahmanhussain160@gmail.com", "cuyiocvzwiqdbyap");
			Client.Send("abdelrahmanhussain160@gmail.com",email.Recipents,email.Subject,email.Body);
		}
	}
}
