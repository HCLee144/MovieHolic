using System.Net.Mail;
using System.Net;
using System.Text;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Models
{
    public class CForgetPassword
    {

        MovieContext _movieContext=new MovieContext();
        public void getNewPasswordEmail(string email)
        {

            //生成一個新的隨機密碼
            string newPassword = GenerateRandomPassword();

            // 使用 Google Mail Server 發信
            string GoogleEmail = "iSpanMovieTheater@gmail.com"; //Google 發信帳號
            string GooglePassword = "yrxrcgvlwhtufszp"; //應用程式密碼
            string ReceiveMail = email; //接收信箱

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleEmail);
            mms.Subject = "[密碼重設]-Movie Holic瘋電影會員";
            mms.Body = "您好，" + "您的新密碼為 :" + newPassword + "，請重新登入並重設您的密碼。";
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            mms.To.Add(new MailAddress(ReceiveMail));
            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleEmail, GooglePassword);//寄信帳密 
                client.Send(mms); //寄出信件
            }


            var member = (from p in _movieContext.TMembers
                          where p.FEmail == email
                          select p).ToList().FirstOrDefault();

            if (member != null)
            {
                member.FPassword = newPassword;
                _movieContext.SaveChanges();
                string resetPassword = "您的密碼已重設。請檢查您的電子郵件。";

            }


        }
        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }
    }

}
