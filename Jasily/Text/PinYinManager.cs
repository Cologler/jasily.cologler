using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Jasily.Text
{
    /// <summary>
    /// how to use: http://www.evernote.com/l/ALIpFNbcP99E_5AUADs89Ty1dL2K12obbDU/
    /// </summary>
    public class PinYinManager : ITryGetValue<char, PinYinManager.Pinyin[]>, ITryGetValue<char, PinYinManager.Pinyin>
    {
        private readonly Lazy<Dictionary<uint, string>> innerLazyData;

        public PinYinManager(string uni2Pinyin)
        {
            var factory = new Func<Dictionary<uint, string>>(() => Init(uni2Pinyin));
            this.innerLazyData = new Lazy<Dictionary<uint, string>>(factory, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private static Dictionary<uint, string> Init(string uni2Pinyin)
        {
            var innerData = new Dictionary<uint, string>();

            using (var reader = new StringReader(uni2Pinyin))
            {
                foreach (var line in reader.EnumerateLines())
                {
                    if (!line.StartsWith("#"))
                    {
                        var lines = line.Split('\t');
                        innerData.Add(uint.Parse(lines[0], NumberStyles.HexNumber), line);
                    }
                }
            }

            return innerData;
        }

        public bool IsChineseChar(char ch)
        {
            return this.innerLazyData.Value.ContainsKey(ch);
        }

        public Pinyin? this[char ch] => this.GetValueOrNull<char, Pinyin>(ch);

        public bool TryGetPinYin(char ch, out Pinyin[] pinyins)
        {
            string r;

            if (this.innerLazyData.Value.TryGetValue(ch, out r))
            {
                pinyins = Selector(r);
                return true;
            }
            else
            {
                pinyins = null;
                return false;
            }
        }

        private static Pinyin[] Selector(string source)
        {
            return source.Split('\t').Skip(1).Select(z => new Pinyin(z)).ToArray();
        }

        public bool TryGetFirstPinYin(char ch, out Pinyin pinyin)
        {
            Pinyin[] pinyins = null;
            if (this.TryGetPinYin(ch, out pinyins) && pinyins.Length > 0)
            {
                pinyin = pinyins[0];
                return true;
            }
            else
            {
                pinyin = default(Pinyin);
                return false;
            }
        }

        public struct Pinyin
        {
            internal Pinyin(string pinyin)
            {
                Debug.Assert(!string.IsNullOrEmpty(pinyin));

                this.PinYin = pinyin.Substring(0, pinyin.Length - 1);

                switch (pinyin[pinyin.Length - 1])
                {
                    case '1':
                        this.Tone = ToneType.Tone1; break;
                    case '2':
                        this.Tone = ToneType.Tone2; break;
                    case '3':
                        this.Tone = ToneType.Tone3; break;
                    case '4':
                        this.Tone = ToneType.Tone4; break;
                    default:
                        this.Tone = ToneType.Unknown; break;
                }
            }

            public string PinYin { get; }

            public ToneType Tone { get; }
        }

        public enum ToneType : byte
        {
            Unknown,
            Tone1,
            Tone2,
            Tone3,
            Tone4
        }

        bool ITryGetValue<char, Pinyin[]>.TryGetValue(char key, out Pinyin[] value)
            => this.TryGetPinYin(key, out value);

        bool ITryGetValue<char, Pinyin>.TryGetValue(char key, out Pinyin value)
            => this.TryGetFirstPinYin(key, out value);
    }
}
