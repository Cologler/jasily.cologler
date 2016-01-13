# How To Use

## NotifyPropertyChangedObject

### 原型

```
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
// 非 UI 线程
this.RegisterForEndRefresh(z => z.Name);

// 然后回到 UI 线程
this.EndRefresh(); // 可以通知 UI 更新 Name
```

#### e.g. RefreshProperties()

``` cs
class X : NotifyPropertyChangedObject
{
    [NotifyPropertyChanged] // 可以通过参数进行排序
    public string Name { get; set; }
}

var x = new X();
x.RefreshProperties(); // 通知 UI 所有带 [NotifyPropertyChanged] 的属性都更新了。
```

## JasilyEditableViewModel

### 原型

``` cs
class JasilyEditableViewModel<T> :  JasilyViewModel<T>
{
    public virtual void WriteToObject(T obj);
    public virtual void ReadFromObject(T obj);
}
```

### 需求

编辑一个类的属性，要么都提交，要么都不提交

### e.g.

已经有了一个类：

``` cs
A a = new A() {  Name = '5', Level =7, Other = 8 }; // 要被修改的类

// 定义 A 的编辑器
class B : JasilyEditableViewModel<A>
{
    [EditableField]
    string Name { get; set; } // 要和 A 同名
    
    [EditableField]
    int Level;
}

// 使用
var b = new B();
b.ReadFromObject(a); // 从 a 读取数据
b.Name = '';
b.Level = 100;
b.WriteToObject(b); // 将更改后的数据写入 a
```

