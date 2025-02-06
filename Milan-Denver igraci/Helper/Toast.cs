using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notification.Wpf;
using System.Threading.Tasks;

namespace Milan_Denver_igraci.Helper
{
    public class Toast
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }

        public Toast()
        {

        }

        public Toast(string title, string message, NotificationType type)
        {
            Title = title;
            Message = message;
            Type = type;
        }
    }
}
