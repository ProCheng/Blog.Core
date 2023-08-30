using Blog.Core.Model.Entity;
using log4net.Util;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Api.Hubs
{
    public class ChatUser
    {
        public string ConID { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 消息类
    /// </summary>
    public class Messages
    {
        public string ID { get; set; }         // 消息 ID，连接ID
        public string SenderName { get; set; }  // 发送者姓名
        public string ReceiverName { get; set; }   // 接受者姓名
        public string Content { get; set; } // 消息内容
        public DateTime Timestamp { get; set; } // 发送时间
        public MessageType Type { get; set; }   // 类型
    }
    public enum MessageType
    {
        指定用户,
        某个组,
        所有人
    }

    public class ChatHub : Hub<IChatClient>
    {
        /// <summary>
        /// 连接池
        /// </summary>
        public static List<ChatUser> _connections = new List<ChatUser>();
        /// <summary>
        /// 消息池
        /// </summary>
        public static List<Messages> _messages = new List<Messages>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _connections.Add(new ChatUser
            {
                 ConID = Context.ConnectionId,
                 Name = Context.User.Identity.Name
            });
            await RoomInit();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connections.Remove(_connections.FirstOrDefault(i=> i.ConID == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }


        #region 组
        /// <summary>
        /// 加入指定组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// 退出指定组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        #endregion


        #region 消息

        /// <summary>
        /// 发送信息到指定人
        /// </summary>
        /// <param name="user">成员名</param>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        public async Task SendMessageToUser(string user, string message)
        {
            
            var msg = new Messages()
            {
                Content = message,
                ID = Context.ConnectionId,
                SenderName = Context.User.Identity.Name,
                ReceiverName = user,
                Timestamp = DateTime.Now,
                Type = MessageType.指定用户
            };
            await Clients.User(user).ReceiveMessage(msg);
            _messages.Add(msg);

        }

        /// <summary>
        /// 发送消息到所有人
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAll(string message)
        {
            var msg = new Messages()
            {
                Content = message,
                ID = Context.ConnectionId,
                SenderName = Context.User.Identity.Name,
                ReceiverName = null,
                Timestamp = DateTime.Now,
                Type = MessageType.所有人
            };
            await Clients.All.ReceiveMessage(msg);
            _messages.Add(msg);

        }

        /// <summary>
        /// 发送消息到指定组
        /// </summary>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToGroup(string group,string message) {

            var msg = new Messages()
            {
                Content = message,
                ID = Context.ConnectionId,
                SenderName = Context.User.Identity.Name,
                ReceiverName = group,
                Timestamp = DateTime.Now,
                Type = MessageType.某个组
            };
            await Clients.Group(group).ReceiveMessage(msg);
            _messages.Add(msg);

        }

        /// <summary>
        /// 进入聊天室，初始化
        /// </summary>
        /// <returns></returns>
        private async Task RoomInit()
        {
            await Clients.All.ReceiveAllUser(_connections);
            await Clients.All.ReceiveMessageLog(_messages);
        }
        #endregion


    }
}
