# How To Use

`class JasilyEditableViewModel<T>`

### 需求

编辑一个类的属性，要么都提交，要么都不提交

### e.g.

已经有了一个类：

``` cs
A a = new A() {  Name = '5', Level =7, Other = 8 } // 要被修改的类

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

