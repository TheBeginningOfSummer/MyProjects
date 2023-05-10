using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IPFS.ViewModels
{
    public class DisplayVM : ObservableObject, IRecipient<RequestMessage<Animation>>
    {
        #region 绑定的属性
        private Animation? _selectedItem;
        public Animation? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<Animation> Albums { get; } = new ObservableCollection<Animation>();
        public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }
        #endregion

        #region 绑定的命令
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
                NavigationService.Navigation("DetailPage.xaml", "DetailVM", SelectedItem);
            }
            catch (Exception)
            {

            }
        });

        private RelayCommand<Animation>? _DeleteCommand;
        public RelayCommand<Animation> DeleteCommand => _DeleteCommand ??= new RelayCommand<Animation>((message) =>
        {
            if(message != null)
            {
                MessageBox.Show(message.Name);
            }
        });

        #endregion

        #region 组件
        private readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;
        #endregion

        public DisplayVM()
        {
            _csl.SQLite.InitializeTableAsync<Animation>();
            PageMessengerInitialize();
        }

        public void Receive(RequestMessage<Animation> message)
        {
            if (SelectedItem != null) message.Reply(SelectedItem);
        }

        private async Task PageUpdateAsync(SQLiteService sqlite)
        {
            Albums.Clear();
            List<Animation>? AlbumsSource = await sqlite.SQLConnection.Table<Animation>().ToListAsync();
            if (AlbumsSource != null)
                foreach (var animation in AlbumsSource)
                {
                    //读出的json数据解析
                    animation.GetVideosData();
                    Albums.Add(animation);
                }
        }

        private async void MessageUpdateAsync(object recipient, Animation message)
        {
            await PageUpdateAsync(_csl.SQLite);
        }

        private async void PageMessengerInitialize()
        {
            await PageUpdateAsync(_csl.SQLite);
            //初始化详情页面时，监听数据请求
            WeakReferenceMessenger.Default.Register(this, "InitializeDetailVM");
            //上传数据更改时，刷新界面
            WeakReferenceMessenger.Default.Register<Animation, string>(this, "DisplayVM", MessageUpdateAsync);
        }

    }
}
