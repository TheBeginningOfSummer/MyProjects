using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Models;
using IPFS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;

namespace IPFS.ViewModels
{
    public class DisplayVM : ObservableObject
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
            if (SelectedItem != null)
                MessageBox.Show(SelectedItem.Name);
        });

        public List<Animation> AnimationsSource; 
        public ObservableCollection<Animation> Animations { get; } = new ObservableCollection<Animation>();
        public ICollectionView AnimationsView { get { return CollectionViewSource.GetDefaultView(Animations); } }
        
        private readonly SQLiteService _sqlite = new();
        private readonly HttpClientAPI _ipfsApi = new();

        public DisplayVM()
        {
            _sqlite.InitializeTable<Animation>();
            AnimationsSource = _sqlite.SQLConnection.Table<Animation>().ToListAsync().Result;
            foreach (var animation in AnimationsSource)
            {
                animation.GetVideosData();
                Animations.Add(animation);
            }
        }
    }
}
