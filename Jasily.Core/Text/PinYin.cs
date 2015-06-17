using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace System.Text
{
    /// <summary>
    /// how to use: http://www.evernote.com/l/ALIpFNbcP99E_5AUADs89Ty1dL2K12obbDU/
    /// </summary>
    public class PinYin : ITryGetValue<char, PinYin.Pinyin[]>, ITryGetValue<char, PinYin.Pinyin>
    {
        Lazy<Dictionary<uint, string>> InnerLazyData;
        
        public PinYin(string Uni2Pinyin)
        {
            var factory = new Func<Dictionary<uint, string>>(() => Init(Uni2Pinyin));
            this.InnerLazyData = new Lazy<Dictionary<uint, string>>(factory, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private static Dictionary<uint, string> Init(string Uni2Pinyin)
        {
            var innerData = new Dictionary<UInt32, string>();
            string line = null;
            string[] lines = null;

            using (var reader = new StringReader(Uni2Pinyin))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("#"))
                    {
                        lines = line.Split('\t');
                        innerData.Add(UInt32.Parse(lines[0], NumberStyles.HexNumber), line);
                    }
                }
            }

            return innerData;
        }

        public bool IsChineseChar(char ch)
        {
            return this.InnerLazyData.Value.ContainsKey(ch);
        }

        public Pinyin? this[char ch]
        {
            get { return this.GetValueOrNull<char, Pinyin>(ch); }
        }

        public bool TryGetPinYin(char ch, out Pinyin[] pinyins)
        {
            string r;

            if (this.InnerLazyData.Value.TryGetValue(ch, out r))
            {
                pinyins = this.Selector(r);
                return true;
            }
            else
            {
                pinyins = null;
                return false;
            }
        }

        private Pinyin[] Selector(string source)
        {
            return source.Split('\t').Skip(1).Select(z => new Pinyin(z)).ToArray();
        }

        public bool TryGetFirstPinYin(char ch, out Pinyin pinyin)
        {
            Pinyin[] pinyins = null;
            if (this.TryGetPinYin(ch, out pinyins))
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

            public string PinYin;
            public ToneType Tone;
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
        {
            return this.TryGetPinYin(key, out value);
        }

        bool ITryGetValue<char, Pinyin>.TryGetValue(char key, out Pinyin value)
        {
            return this.TryGetFirstPinYin(key, out value);
        }
    }
}
