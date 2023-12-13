using CommunityToolkit.Mvvm.Messaging;

namespace IPFS.Services
{
    public class NavigationService
    {
        public NavigationService() { }

        public static void Navigation(string page)
        {
            //发送消息到主窗口，进行切换页面
            WeakReferenceMessenger.Default.Send(page, "ChangePage");
        }

        public static void Navigation<T>(string page, string token, T? message) where T : class
        {
            //发送消息到主窗口，进行切换页面
            WeakReferenceMessenger.Default.Send(page, "ChangePage");
            //发送数据到详情页，进行展示
            if (message != null)
                WeakReferenceMessenger.Default.Send(message, token);
        }
    }
}
