## PinYinManager

### how to use?

1. download resource from ftp: [ftp://ftp.cuhk.hk/pub/chinese/ifcss/software/data/Uni2Pinyin.gz](ftp://ftp.cuhk.hk/pub/chinese/ifcss/software/data/Uni2Pinyin.gz)
1. create manager from resource like:

``` cs
using (var stream = File.OpenRead(@"Uni2Pinyin"))
{
    using (var reader = new StreamReader(stream))
    {
        var manager = new PinYinManager(reader.ReadToEnd());
        var pinyin = manager['赖']; // get pin yin
    }
}
```

##### resource mirror

onedrive: [https://onedrive.live.com/redir?resid=798FF831C53CDF44!729515&authkey=!AE0jRBSvzjEw1d0&ithint=file%2cgz](https://onedrive.live.com/redir?resid=798FF831C53CDF44!729515&authkey=!AE0jRBSvzjEw1d0&ithint=file%2cgz)
