using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace IPFS.ViewModels
{
    public class DisplayVM : ObservableObject, IRecipient<RequestMessage<Animation>>
    {
        private Animation? _selectedItem;
        public Animation? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private RelayCommand? _refreshCommand;
        public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(() =>
        {
            //if (SelectedItem != null)
            //    MessageBox.Show(SelectedItem.Name);
        });

        private RelayCommand? _itemPaddingCommand;
        public RelayCommand ItemPaddingCommand => _itemPaddingCommand ??= new RelayCommand(() =>
        {
            try
            {
                //发送消息到主窗口，进行切换页面
                WeakReferenceMessenger.Default.Send("DetailPage.xaml");
                //发送数据到详情页，进行展示
                if (SelectedItem != null)
                    WeakReferenceMessenger.Default.Send(SelectedItem);
            }
            catch (System.Exception)
            {


            }
        });

        #region 集合
        public List<Animation> AnimationsSource;
        public ObservableCollection<Animation> Animations { get; } = new ObservableCollection<Animation>();
        public ICollectionView AnimationsView { get { return CollectionViewSource.GetDefaultView(Animations); } }
        #endregion

        private readonly SQLiteService _sqlite = new();
        private readonly HttpClientAPI _ipfsApi = new();

        public DisplayVM()
        {
            _sqlite.InitializeTable<Animation>();
            AnimationsSource = _sqlite.SQLConnection.Table<Animation>().ToListAsync().Result;
            foreach (var animation in AnimationsSource)
            {
                //读出的json数据解析
                animation.GetVideosData();
                Animations.Add(animation);
            }
            //初始化详情页面时，监听数据请求
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(RequestMessage<Animation> message)
        {
            if (SelectedItem != null) message.Reply(SelectedItem);
        }
    }
}
