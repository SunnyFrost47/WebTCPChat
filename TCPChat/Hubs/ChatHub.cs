using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using TCPChat.Models;

namespace TCPChat.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();
        static List<ChatMessage> Messages = new List<ChatMessage>();
        static List<ChatUser> Users = new List<ChatUser>();

        public void Send(string name, string message, string userId)
        {
            if (Users.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                int id;
                if (Messages.Count == 0) id = db.ChatLog.Count() + 1;
                else id = Messages.Last().Id + 1;
                ChatMessage NewMessage = new ChatMessage { Id = id, Nick = name, Data = message, Date = DateTime.Now, UserId = userId };
                db.ChatLog.Add(NewMessage);
                db.SaveChanges();
                Messages.Add(NewMessage);
                Clients.All.addMessage(Messages.Last());
            }
        }

        public void Connect(string userName)
        {
            //var id = Context.ConnectionId;
            if (!Users.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                Users.Add(new ChatUser { ConnectionId = Context.ConnectionId, NickName = userName });
                Clients.Caller.onConnected(Context.ConnectionId, userName, Users, Messages);
                Clients.AllExcept(Context.ConnectionId).onNewUserConnected(Context.ConnectionId, userName);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.NickName);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}