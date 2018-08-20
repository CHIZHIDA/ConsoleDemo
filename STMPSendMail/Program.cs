using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace STMPSendMail
{
    class Program
    {
        #region
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailpwd, string mailTo, string mailSubject, string mailContent)
        {
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.qq.com"; //SMTP服务器
            string mailFrom = "chizhida@qq.com"; //登陆用户名
            string userPassword = "ojhwngeafjvibfdf";//登陆密码(授权码)

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("send default");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
                return false;
            }
        }
        #endregion

        static void Main(string[] args)
        {
            string inputpwd = null;
            string inputmail = null;
            string inputsubject = null;
            string inputcontent = null;

            Console.Write("please input mail passworld:");
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey(true);
                if (ck.Key != ConsoleKey.Enter)
                {
                    if (ck.Key != ConsoleKey.Backspace)
                    {
                        inputpwd += ck.KeyChar.ToString();
                        Console.Write("*");
                    }
                    else
                    {
                        inputpwd = inputpwd.Substring(0, inputpwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("please input send to maill address:");
                    inputmail = Console.ReadLine();

                    Console.WriteLine();
                    Console.Write("please input send to maill subject:");
                    inputsubject = Console.ReadLine();

                    Console.WriteLine();
                    Console.Write("please input send to maill content:");
                    inputcontent = Console.ReadLine();

                    bool result = SendEmail(inputpwd, inputmail, inputsubject, inputcontent);
                    if (result)
                    {
                        Console.WriteLine("send success");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
