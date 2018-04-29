﻿using System.Text;
using System.Net;
using System.Net.Mail;
using PlaySpace.Abstract;
using System.Linq;

namespace PlaySpace.Models
{
    public class EmailSettings
    {
        public string MailToAddress = "vip.ra.19992@gmail.com";
        public string MailFromAddress = "vip.ra.1999@gmail.com";
        public bool UseSsl = true;
        public string Username = "vip.ra.1999@gmail.com";
        public string Password = "ruslanruslan132";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                StringBuilder body = new StringBuilder()
                    .AppendLine("Здравствуйте, " + shippingInfo.Name + ". Ваш заказ:")
                    .AppendLine("---");

                foreach (var line in cart.Lines)
                {
                    var subtotal = (line.Game.Price / 100 * (100 - line.Game.Discount)) * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c})",
                        line.Quantity, line.Game.Name, subtotal)
                        .AppendLine();
                }

                body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                    .AppendLine()
                    .AppendLine("---");
                
                foreach (var line in cart.Lines)
                {
                    int i = 0;
                    while (i < line.Quantity)
                    {
                        using (UserContext db = new UserContext())
                        {
                            Game dbEntry = db.Games.Include(nameof(Game.Keys)).FirstOrDefault(g => g.GameId == line.Game.GameId);
                            body.AppendFormat("Ваш ключ для игры {0}:{1}.", line.Game.Name, dbEntry.ActiveKey);
                            dbEntry.ActiveKey = db.Keys.FirstOrDefault(p => p.GameId == line.Game.GameId).Item;
                            Key delkey = db.Keys.FirstOrDefault(p => p.Item == dbEntry.ActiveKey);
                            db.Keys.Remove(delkey);
                            db.SaveChanges();
                        }
                        body.AppendLine();
                        i++;
                    }
                }
                body.AppendLine("Спасибо за покупку! :)");
                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// От кого
                                       shippingInfo.Email,		// Кому
                                       "Заказ доставлен!",		// Тема
                                       body.ToString());            // Тело письма
                smtpClient.Send(mailMessage);
            }
        }
    }
}