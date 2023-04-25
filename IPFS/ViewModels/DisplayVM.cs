using CommunityToolkit.Mvvm.ComponentModel;
using IPFS.Models;
using IPFS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace IPFS.ViewModels
{
    public class DisplayVM : ObservableObject
    {
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
