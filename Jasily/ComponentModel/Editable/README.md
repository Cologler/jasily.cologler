# How To Use

## JasilyEditableViewModel

### type

``` cs
class JasilyEditableViewModel<T> :  JasilyViewModel<T>
{
    public virtual void WriteToObject(T obj);
    public virtual void ReadFromObject(T obj);
}
```

### design for

edit a object fields (or properties), all commit, or not commit.

### e.g. 1

``` cs
A a = new A() {  Name = '5', Level = 7, Other = 8 }; // 要被修改的类

// define a editor and name it B.
class B : JasilyEditableViewModel<A>
{
    [EditableField]
    public string Name { get; set; } // 要和 A 同名
    
    [EditableField]
    public int Level;
}

// use B
var b = new B();
b.ReadFromObject(a); // read data from a
b.Name = '';
b.Level = 100;
// after user click 'OK' button
b.WriteToObject(a); // write data to a
```

### e.g. 2

auto call child EditableViewModel:

``` cs
class C : JasilyEditableViewModel<A>
{
    [EditableField(IsSubEditableViewModel = true)]
    public B Name { get; }
}

var c = new C();
// ok, then it can auto call B.ReadFromObject() and b.WriteToObject()
c.ReadFromObject(a);
c.WriteToObject(a);
```

### e.g. 3

but `TextBox` only support input `string`, how can I working with `A.Level` (a `int` type)?

``` cs
class D : JasilyEditableViewModel<A>
{
    [EditableField(Converter = typeof(Int32ToStringConverter))]
    public string Level { get; set; }
}
// then it can using Converter for get or set. :)
```

### e.g. 4

if I use `Property<string>` for it, how can I code?

``` cs
class D : JasilyEditableViewModel<A>
{
    [EditableField(Converter = typeof(Int32ToStringConverter))]
    public Property<string> Level { get; set; }
}
// it will auto wrap and unwrap Property<>, but you should sure Level was not null.
```