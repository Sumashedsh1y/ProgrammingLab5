using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Lab_5_Semestr_3
{
    class Data
    {
        public string FileName { get; private set; }
        public Match Match { get; private set; }
        public string Text { get; private set; }

        internal void ReadFromFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                Text = sr.ReadToEnd().Replace("\r", "");
                FileName = fileName;
            }
        }
        public void Find(string re)  
        {
            Match = Regex.Match(Text, re);
        }
        public void Next()
        {
            Match = Match?.NextMatch();
        }
        public void GetOfOrStatistics(out int ofc, out int orc)
        {
            ofc = 0; orc = 0;
            foreach (Match m in Regex.Matches(Text, @"\b(of|or)\b"))
            {
                if (m.Value == "of") ofc++;
                else orc++;
            }
        }
        public ISet<string> FindSentencesFirstWords()
        {
            ISet<string> words = new HashSet<string>();
            foreach (Match m in Regex.Matches(Text, @"(([A-Z]|[А-Я])([a-z]|[а-я])+)\b+.*[?]"))
            {
                words.Add(m.Groups[1].Value);
            }
            return words;
        }
        public SortedDictionary<string, int> FirstLetterCounts()
        {
            SortedDictionary<string, int> counts = new SortedDictionary<string, int>();
            Regex r = new Regex(@"(?<=\s)[A-Za-zА-Яа-я]");
            foreach (Match m in r.Matches(Text))
            {
                string b = m.Value.ToUpper();
                if (counts.ContainsKey(b))
                {
                    counts[b]++;
                }
                else
                {
                    counts[b] = 1;
                }
            }
            return counts;
        }
        internal Tuple<string, float> FindMaxSentence()
        {
            int startcount = 0;
            string max = "";
            int sumcounts = 0;
            int sentenses = 0;
            string[] txt = Regex.Split(Text, @"(?<=[\.!\?])\s+");
            foreach (string word in txt)
            {
                int count = word.Split(' ').Length;
                sumcounts += count;
                sentenses += 1;
                if (count > startcount)
                {
                    max = word;
                    startcount = count;
                }
            }
            float srednee = sumcounts / sentenses;
            return Tuple.Create(max, srednee);
        }
    }
}
