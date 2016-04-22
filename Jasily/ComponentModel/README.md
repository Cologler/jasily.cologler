# How To Use

## NotifyPropertyChangedObject

### 原型

``` cs
class NotifyPropertyChangedObject : object
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    public void RegisterForEndRefresh<T>(Expression<Func<T, object>> propertySelector);
    public void EndRefresh();
    
    protected bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null);
    protected bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames);
    
    public virtual void RefreshProperties();
    
    public void ClearPropertyChangedInvocationList();
}
```

#### e.g. RegisterForEndRefresh() & EndRefresh()

``` cs
// not UI thread call, call:
this.RegisterForEndRefresh(z => z.Name);

// then back in UI thread, call:
this.EndRefresh(); // 可以通知 UI 更新 Name
```

#### e.g. RefreshProperties()

``` cs
class X : NotifyPropertyChangedObject
{
    [NotifyPropertyChanged] // can order by args.
    public string Name { get; set; }
}

var x = new X();
x.RefreshProperties(); // notify UI with all properties which contains [NotifyPropertyChanged] was updated.
```

## PropertySelector

### 原型

``` cs
public static class PropertySelector
{
    public static PropertySelector<TProperty> SelectProperty<T, TProperty>(this T obj,
        Expression<Func<T, TProperty>> selectExpression)
        => PropertySelector<T>.Start(selectExpression);

    public static PropertySelector<TProperty> SelectProperty<T, TProperty>(
        Expression<Func<T, TProperty>> selectExpression)
        => PropertySelector<T>.Start(selectExpression);
}

class PropertySelector<T> : object
{
    public static PropertySelector<TProperty> Start<TProperty>(Expression<Func<T, TProperty>> selectExpression);
    public PropertySelector<TProperty> Select<TProperty>(Expression<Func<T, TProperty>> selectExpression);
    public PropertySelector<TProperty> SelectMany<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> selectExpression)
}
```

### 需求

C# 6.0 添加了语法 `nameof(this.Name)`，但是对于深层的属性选择还是很困难。
比如 `nameof(this.UserConfig.Mapper)` 返回 `"Mapper"`。
而我需要的是 `"UserConfig.Mapper"`。

### e.g.

``` cs
// "UserConfig.Mapper"
this.SelectProperty(z => z.UserConfig.Mapper);

// "Length"
PropertySelector<int[]>.Start(z => z.Length);

// "Item1.Length"
PropertySelector<Tuple<string[], string>>.Start(z => z.Item1).SelectMany(z => z).Select(z => z.Length);
```

问题是，Item1.Length 指的是 string[].Length 还是 string.Length，我不能确定。

但是不能否定，在 NoSQL 数据库的查询上挺有用的。