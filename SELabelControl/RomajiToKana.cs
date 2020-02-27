using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SELabelControl
{
    public class RomajiToKana
    {
        static readonly List<ConversionData> table
            = new List<ConversionData>();

        public RomajiToKana()
        {
            SetDictionary(ConversionType.HalfKatakana);
        }

        public RomajiToKana(ConversionType type)
        {
            SetDictionary(type);
        }
        
        // Main Method
        public string ConvertToKana(string currentString)
        {
            if (string.IsNullOrEmpty(currentString)) return string.Empty;

            // Get the alphabets from the last 3 charactors to be converted.
            // The charactors to be converted is [A-Z] and [-ｰ](ﾏｲﾅｽ,長音)
            string targetString = GetTargetCharactors(currentString);

            if (targetString.Length == 0) return currentString;

            string nonTargetPart = currentString.Substring(0, currentString.Length - targetString.Length);
            string converted = GetKana(targetString);

            return nonTargetPart + converted;
        }

        private string GetTargetCharactors(string originalString)
        {
            // take max 3 chraractors from the end of the original string.
            string str = "";
            int originalLength = originalString.Length;
            if (originalLength > 3) { str = originalString.Substring(originalLength - 3); }
            else                    { str = originalString; }

            // target charactor is alphabet or '-'.
            string resultString = "";
            var chars = str.Reverse().ToArray();
            for(int i=0; i < chars.Length; i++)
            {
                if (!Regex.IsMatch(chars[i].ToString(CultureInfo.CurrentCulture), @"[A-Z-]", RegexOptions.IgnoreCase)) break;
                resultString = chars[i] + resultString;
            }

            return resultString;
        }

        private string GetKana(string romaji)
        {
            // 小文字変換,(長音)→(ﾏｲﾅｽ)
            string str = romaji.ToLower(CultureInfo.CurrentCulture).Replace("ｰ", "-");

            var candidates = table.Where(x => x.From.StartsWith(str,true,CultureInfo.CurrentCulture));
            if (candidates == null) return romaji;

            if (candidates.Count() == 1 && candidates.First().From == str)
            {
                return candidates.First().To;
            }
            else
            {
                if (Regex.IsMatch(str, "n[^n]"))
                {
                    var n = table.Where(x => x.From == "n").Select(x => x.To).First();
                    return n.ToString(CultureInfo.CurrentCulture) + str.Substring(1);
                }
                else
                {
                    return romaji;
                }
            }
        }

        private void SetDictionary(ConversionType conversionType)
        {
            int dataRow;
            
            switch (conversionType)
            {
                case ConversionType.HalfKatakana:
                    dataRow = 1;
                    break;
                case ConversionType.FullKatakana:
                    dataRow = 2;
                    break;
                case ConversionType.Hiragana:
                    dataRow = 3;
                    break;
                default:
                    dataRow = 1;
                    break;
            }

            ReadTableFile(dataRow);
        }

        private void ReadTableFile(int dataRow)
        {
            table.Clear();
            
            using (var reader = new StreamReader(@"RomaToKanaTable.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] items = line.Split(',');

                    var conv = new ConversionData();
                    conv.From = items[0];
                    conv.To = items[dataRow];
                    table.Add(conv);
                }
            }
        }

        struct ConversionData
        {
            public string From { get; set; }
            public string To { get; set; }
        }
    }
    
    public enum ConversionType
    {
        HalfKatakana,
        FullKatakana,
        Hiragana
    }
}
