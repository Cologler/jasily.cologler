# jasily.cologler

a C# extend library for myself

### how to use

modules|platform|note
:-:|:-:|:-:
Jasily.Core|for any|base for any
Jasily.Core.CSShard|none|**do not use it**
Jasily.Core.Desktop|desktop|link from Jasily.Core.CSShard
Jasily.Core.WP80|windows phone 8.0|link from Jasily.Core.CSShard
Jasily.Core.WP81|windows phone 8.1|link from Jasily.Core.CSShard

## dev

### when use `throw new ArgumentNullException();`

For path safe.

e.g.1

``` cs
public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> list)
    => new ReadOnlyCollection<T>(list);
```

Do not check `null` because of `ReadOnlyCollection<T>()` will check it.