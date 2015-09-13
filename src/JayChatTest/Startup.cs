using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Net.Http.Server;
using websocket_sharp_test.ChatHub;
using WebSocketSharp.Server;

namespace JayChatTest
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                await context.Response.SendFileAsync("index.html");
            });

           
            
            try
            {
                var wssv = new WebSocketServer(80);
                wssv.AddWebSocketService<Chat>("/Chat");
                wssv.Start();
                
            }
            catch (Exception ex)
            {
                // format needs to be host:port:username:password
                string[] emailInfo = Environment.GetEnvironmentVariable("defaultEmailServerValues").Split(':');

                if (emailInfo.Length > 0)
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("info@landofzorth.com");
                    message.To.Add(new MailAddress("jairo.assis@landofzorth.com"));

                    message.Subject = "Shit blew up!";
                    message.Body = $"Error Message: {ex.Message}, InnerException: {ex.InnerException}, Stacktrace: {ex.StackTrace}";

                    SmtpClient client = new SmtpClient();
                    client.Host = emailInfo[0];
                    client.Port = int.Parse(emailInfo[1]); 
                    client.Credentials = new System.Net.NetworkCredential(emailInfo[2], emailInfo[3]);
                    client.EnableSsl = true;
                    client.Send(message);
                }
                
                throw;
            }

        }
    }
}
