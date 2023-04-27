using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IPFS.ViewModels
{
    public class DetailVM : ObservableObject
    {
        private Animation? _animationInfo;
        public Animation? AnimationInfo
        {
            get => _animationInfo;
            set => SetProperty(ref _animationInfo, value);
        }

        public ObservableCollection<FileData> FileInfo { get; } = new ObservableCollection<FileData>();

        private RelayCommand? _returnCommand;
        public RelayCommand ReturnCommand => _returnCommand ??= new RelayCommand(() =>
        {
            WeakReferenceMessenger.Default.Send("DisplayPage.xaml");
        });

        private RelayCommand? _openInBrowser;
        public RelayCommand OpenInBrowser => _openInBrowser ??= new RelayCommand(() =>
        {
            MessageBox.Show("测试");
        });


        public DetailVM()
        {
            //请求初始数据
            var result = WeakReferenceMessenger.Default.Send(new RequestMessage<Animation>());
            AnimationInfo = result.Response;
            UpdateData(AnimationInfo);
            //注册数据接收
            WeakReferenceMessenger.Default.Register<Animation>(this, LoadInfo);
        }

        private void LoadInfo(object recipient, Animation message)
        {
            if (message != null) AnimationInfo = message;
            UpdateData(AnimationInfo);
        }

        private void UpdateData(Animation? AnimationInfo)
        {
            if (AnimationInfo == null) return;
            FileInfo.Clear();
            foreach (var item in AnimationInfo!.VideosData!.Values)
                FileInfo.Add(item);
        }
    }
}
