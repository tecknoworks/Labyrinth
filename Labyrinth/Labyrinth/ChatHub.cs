﻿using Labyrinth;
using Labyrinth.Common;
using Microsoft.AspNet.Identity;
using Labyrinth.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labyrinth
{
    public class ChatHub : Hub
    {
        #region Data Members

        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public void Connect(string hubId, string userName)
        {
            var id = Context.ConnectionId;

            if (ConnectedUsers.Count(x => x.UserName == userName) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName);

            }
            // send to caller
            Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool ok)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }

            return base.OnDisconnected(ok);
        }


        #endregion

        #region private Messages

        private void AddMessageinCache(string userName, string message)
        {
            CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        public void boughtItem(string sellerNick, string itemName, string userNick )
        {

            var toUserId = ConnectedUsers.Where(x => x.UserName == sellerNick);
            string message = userNick + " has bought " + itemName + "(s) from you!";
            var clients = ConnectedUsers;
            Clients.Client(toUserId.First().ConnectionId).boughtItem(message);
            //Clients.Client(toUserId).
            //Clients.All.boughtItem(message);

        }

        #endregion
    }
}