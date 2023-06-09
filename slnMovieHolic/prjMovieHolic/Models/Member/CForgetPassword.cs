﻿using System.Net.Mail;
using System.Net;
using System.Text;
using prjMovieHolic.ViewModels;
using System.Security.Cryptography;

namespace prjMovieHolic.Models.Member
{
    public class CForgetPassword
    {

        private readonly MovieContext _movieContext;
        public CForgetPassword(MovieContext context)
        {
            _movieContext = context;
        }
        public void getNewPasswordEmail(string email)
        {
            try
            {
                //生成一個新的隨機密碼
                string newPassword = GenerateRandomPassword();

                var member = (from p in _movieContext.TMembers
                              where p.FEmail == email
                              select p).ToList().FirstOrDefault();

                // 使用 Google Mail Server 發信
                string GoogleEmail = "iSpanMovieTheater@gmail.com"; //Google 發信帳號
                string GooglePassword = "yrxrcgvlwhtufszp"; //應用程式密碼
                string ReceiveMail = email; //接收信箱


                string mailContent = $"<p>親愛的 {member.FName}：<br><br>我們已於{DateTime.Now}收到提出密碼重設的申請！<br>" +
                    $"您的新密碼為：{newPassword}，請重新登入並重設您的密碼。" +
                    $"如有任何疑問或需要進一步協助，請隨時與我們的客服部門聯繫，他們將樂意為您提供幫助。<br><br>" +
                    $"※本郵件為系統自動發出，請勿直接回覆本信件。<br>" +
                    $"※提醒您小心處理及保管本信件。</p>";



                string SmtpServer = "smtp.gmail.com";
                int SmtpPort = 587;
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleEmail, "MovieHolic");
                mms.Subject = "[密碼重設]-Movie Holic瘋電影會員";
                mms.Body = mailContent;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(ReceiveMail));
                using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleEmail, GooglePassword);//寄信帳密 
                    client.Send(mms); //寄出信件
                }

                if (member != null)
                {
                    member.FPassword = newPassword;
                    _movieContext.SaveChanges();
                }
            }
            catch (Exception ex) { };

        }
        private string GenerateRandomPassword()
        {
            try
            {
                RandomNumberGenerator rng = RandomNumberGenerator.Create();

                // Define the character set
                string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                // Define the length of the random string you want to generate
                int stringLength = 8;

                // Create a byte array to hold the random values
                byte[] randomBytes = new byte[stringLength];

                // Generate random values and store them in the byte array
                rng.GetBytes(randomBytes);

                // Convert the byte array to a string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in randomBytes)
                {
                    sb.Append(characterSet[b % characterSet.Length]);
                }
                string randomString = sb.ToString();
                return randomString;
            }
            catch (Exception ex) { return null; }


        }
    }

}
