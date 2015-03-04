using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Text
{
    public class JasilyPinYin :
        IJasilyTryGetValue<char, JasilyPinYin.Pinyin[]>,
        IJasilyTryGetValue<char, JasilyPinYin.Pinyin>
    {
        private Dictionary<UInt32, string> InnerData;

        public JasilyPinYin(string Uni2Pinyin)
        {
            InnerData = new Dictionary<UInt32, string>();
            string line = null;
            string[] lines = null;

            using (var reader = new StringReader(Uni2Pinyin))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("#"))
                    {
                        lines = line.Split('\t');
                        InnerData.Add(UInt32.Parse(lines[0], NumberStyles.HexNumber), line);
                    }
                }
            }
        }

        public bool IsChineseChar(char ch)
        {
            return InnerData.ContainsKey(ch);
        }

        public Pinyin this[char ch]
        {
            get
            {
                return ((IJasilyTryGetValue<char, JasilyPinYin.Pinyin>)this).GetValueOrDefault(ch);
            }
        }

        public bool TryGetPinYin(char ch, out Pinyin[] pinyins)
        {
            string r = null;
            if (InnerData.TryGetValue(ch, out r))
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

        private Pinyin[] Selector(string source)
        {
            return source.Split('\t').Skip(1).Select(z => new Pinyin(z)).ToArray();
        }

        public bool TryGetFirstPinYin(char ch, out Pinyin pinyin)
        {
            Pinyin[] pinyins = null;
            if (TryGetPinYin(ch, out pinyins))
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
                PinYin = pinyin.Substring(0, pinyin.Length - 1);

                switch (pinyin[pinyin.Length - 1])
                {
                    case '1': Tone = ToneType.Tone1; break;
                    case '2': Tone = ToneType.Tone2; break;
                    case '3': Tone = ToneType.Tone3; break;
                    case '4': Tone = ToneType.Tone4; break;
                    default: Tone = ToneType.Unknown; break;
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

        bool IJasilyTryGetValue<char, JasilyPinYin.Pinyin[]>.TryGetValue(char key, out JasilyPinYin.Pinyin[] value)
        {
            return TryGetPinYin(key, out value);
        }

        bool IJasilyTryGetValue<char, JasilyPinYin.Pinyin>.TryGetValue(char key, out JasilyPinYin.Pinyin value)
        {
            return TryGetFirstPinYin(key, out value);
        }
    }
}
