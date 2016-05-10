# readme

## accessor performance

you can found source code in my gist:

[performance comparision for C# accessor](https://gist.github.com/Cologler/456d953eb944c1e269249a6e5a5e1e62)

here is the result:

![](http://i.imgur.com/XR9Srju.png)

It seem like Delegate.CreateDelegate() is best way, but here is the problem:

* only accept specified type (accept typeof(Func<Person, object>) not not accept typeof(Func<object, object>)).
* not exists in PCL.

So Expression.Compile() with cached become my best solution.