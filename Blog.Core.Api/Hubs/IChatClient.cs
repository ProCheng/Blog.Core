using System.Threading.Tasks;

namespace Blog.Core.Api.Hubs
{
    public interface IChatClient
    {
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        Task ReceiveMessage(object message);

        /// <summary>
        /// 接收消息历史
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task ReceiveMessageLog(object obj);


        /// <summary>
        /// 接收所有用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task ReceiveAllUser(object obj);

        
    }
}
