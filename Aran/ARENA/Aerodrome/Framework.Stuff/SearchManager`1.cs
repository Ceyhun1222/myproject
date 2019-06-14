// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.SearchManager`1
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Framework.Stuff
{
  public class SearchManager<T> where T : class
  {
    private readonly List<SearchField<T>> _searchFields = new List<SearchField<T>>();
    private IEnumerable<T> _baseList;

    public SearchManager(IEnumerable<T> baseList)
    {
      this._baseList = baseList;
      this.MinQueryLength = 3;
    }

    public IEnumerable<T> BaseList
    {
      get
      {
        return this._baseList;
      }
      set
      {
        this._baseList = value;
      }
    }

    public string SearchPattern { get; set; }

    public int MinQueryLength { get; set; }

    public void AddSearchField(Func<T, string> getProperty, bool searchZeroIndex)
    {
      this._searchFields.Add(new SearchField<T>(getProperty, searchZeroIndex));
    }

    public IEnumerable<T> Values
    {
      get
      {
        return this.DoSearch();
      }
    }

    protected IEnumerable<T> DoSearch()
    {
      if (string.IsNullOrEmpty(this.SearchPattern))
        return this._baseList;
      string[] queries = this.SearchPattern.ToLower().Split(new string[1]
      {
        " "
      }, StringSplitOptions.RemoveEmptyEntries);
      return (!(this._baseList is INotifyCollectionChanged) ? (IEnumerable<T>) this._baseList.ToArray<T>() : this._baseList).Where<T>((Func<T, bool>) (e => this.QueriesMatch(queries, e)));
    }

    private bool QueriesMatch(string[] queries, T entity)
    {
      List<bool> boolList = new List<bool>();
      List<KeyValuePair<string, bool>> targetFields = new List<KeyValuePair<string, bool>>();
      this._searchFields.ForEach((Action<SearchField<T>>) (e => this.SplitSearchFields(e, entity, targetFields)));
      bool flag1 = false;
      foreach (string query in queries)
      {
        if (query.Length >= this.MinQueryLength || queries.Length > 1)
        {
          bool flag2 = false;
          foreach (KeyValuePair<string, bool> keyValuePair in targetFields)
          {
            if (keyValuePair.Value)
            {
              if (keyValuePair.Key.ToLower().IndexOf(query) == 0)
              {
                flag2 = true;
                break;
              }
            }
            else if (keyValuePair.Key.ToLower().IndexOf(query) >= 0)
            {
              flag2 = true;
              break;
            }
          }
          if (flag2)
          {
            flag1 = true;
          }
          else
          {
            flag1 = false;
            break;
          }
        }
      }
      return flag1;
    }

    private void SplitSearchFields(SearchField<T> searchField, T entity, List<KeyValuePair<string, bool>> list)
    {
      string str1 = searchField.GetProperty(entity);
      if (str1 == null)
        return;
      string str2 = str1;
      string[] separator = new string[1]{ " " };
      int num = 1;
      foreach (string key in str2.Split(separator, (StringSplitOptions) num))
      {
        KeyValuePair<string, bool> keyValuePair = new KeyValuePair<string, bool>(key, searchField.SearchZeroIndex);
        list.Add(keyValuePair);
      }
    }
  }
}
