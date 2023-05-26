using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace IPFS.Models;

public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    where TKey : IComparable
{
    public ObservableDictionary() : base() { }
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    private int _index;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public new KeyCollection Keys { get { return base.Keys; } }
    public new ValueCollection Values { get { return base.Values; } }
    public new int Count { get { return base.Count; } }
    public new TValue? this[TKey key]
    {
        get { return this.GetValue(key); }
        set { this.SetValue(key, value); }
    }
    public TValue? this[int index]
    {
        get => GetIndexValue(index);
        set => SetIndexValue(index, value);
    }

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, FindPair(key), _index));
        OnPropertyChanged(nameof(Keys));
        OnPropertyChanged(nameof(Values));
        OnPropertyChanged(nameof(Count));
    }

    public new void Clear()
    {
        base.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        OnPropertyChanged(nameof(Keys));
        OnPropertyChanged(nameof(Values));
        OnPropertyChanged(nameof(Count));
    }

    public new bool Remove(TKey key)
    {
        var pair = FindPair(key);
        if (base.Remove(key))
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, pair, _index));
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Count));
            return true;
        }
        return false;
    }

    #region 方法
    private TValue? GetIndexValue(int index)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (i == index)
            {
                var pair = this.ElementAt(i);
                return pair.Value;
            }
        }
        return default(TValue);
    }

    private void SetIndexValue(int index, TValue? value)
    {
        try
        {
            if (value == null) return;
            var pair = this.ElementAtOrDefault(index);
            SetValue(pair.Key, pair.Value);
        }
        catch (System.Exception)
        {

        }
    }

    private TValue? GetValue(TKey key)
    {
        if (base.ContainsKey(key))
            return base[key];
        else
            return default;
    }

    private void SetValue(TKey key, TValue? value)
    {
        if (value == null) return;
        if (base.ContainsKey(key))
        {
            var pair = this.FindPair(key);
            int index = _index;
            base[key] = value;
            var newPair = this.FindPair(key);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newPair, pair, index));
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged("Item[]");
        }
        else
            Add(key, value);
    }

    private KeyValuePair<TKey, TValue> FindPair(TKey key)
    {
        _index = 0;
        foreach (var item in this)
        {
            if (item.Key.Equals(key)) return item;
            _index++;
        }
        return default;
    }

    private int IndexOf(TKey key)
    {
        int index = 0;
        foreach (var item in this)
        {
            if (item.Key.Equals(key)) return index;
            index++;
        }
        return -1;
    }
    #endregion
}
