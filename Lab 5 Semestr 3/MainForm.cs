using Lab_5_Semestr_3.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Lab_5_Semestr_3
{
    public partial class MainForm : Form
    {
        Data data = new Data();
        public MainForm()
        {
            InitializeComponent();
            listBox1.Items.Add(@"\b(([A-Z])[a-z]+)\?\s");
            listBox1.Items.Add(@"\b\d{4}\b");
            listBox1.Items.Add(@"\b(о)([а-я]){5,}");
            listBox1.Items.Add(@"\b(о)(\S)+");
            listBox1.Items.Add(@"([А-Я]).*([.!?])+");
            listBox1.Items.Add(@"\s+\w+\s+");
            listBox1.Items.Add(@"\b(of|or)\b");
            this.listBox1.Click += (s, e) =>
            {
                textBox1.Text = listBox1.Text;
                data.Find(textBox1.Text);
                ShowMatch();
            };
        }
        private void Load(object sender, EventArgs e)
        {
            data.ReadFromFile(Settings.Default.DefaultFileName);
            Console.WriteLine($"File - {data.FileName} - is open");
            richTextBox1.Text = data.Text;
        }
        private void Save(object sender, FormClosingEventArgs e)
        {
            Settings.Default.DefaultFileName = data.FileName;
            Settings.Default.Save();
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                data.ReadFromFile(dlg.FileName);
                richTextBox1.Text = data.Text;
            }
        }
        private void Find(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                richTextBox2.Text = "Enter the text";
            }
            else
            {
                data.Find(textBox1.Text);
                ShowMatch();
            }
        }
        private void ShowMatch()
        {
            Match m = data.Match;
            if (m != null && m.Success)
            {
                richTextBox1.SelectionBackColor = Color.White;
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Value.Length;
                richTextBox1.ScrollToCaret();
                richTextBox1.SelectionBackColor = Color.Yellow;
                richTextBox2.Text = $"Найдено[{m.Index}]: ##{m.Value}##\n";
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    richTextBox2.Text += String.Format("Groups[{0}]={1}\n", i, m.Groups[i]);
                }
            }

        }

        private void DownAnyKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Find(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void NextMatch(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                richTextBox2.Text = "Enter the text";
            }
            else
            {
                data.Next();
                ShowMatch();
            }

        }

        private void OfOrClick(object sender, EventArgs e)
        {

                int ofc, orc;
            data.GetOfOrStatistics(out ofc, out orc);
            richTextBox2.Text = $" of: {ofc}, or: {orc}";

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

        private void FirstWords(object sender, EventArgs e)
        {
            ISet<String> words = data.FindSentencesFirstWords();
            richTextBox2.Text = String.Join(", ", words);
        }

        private void Statistic(object sender, EventArgs e)
        {
            new StatisticsForm(data.FirstLetterCounts()).Show();
        }

        private void MaxSentense(object sender, EventArgs e)
        {
            string[] text;
            string word = data.FindMaxSentence().Item1;
            float sred = data.FindMaxSentence().Item2;
            text = word.Split(' ');
            richTextBox2.Text = "Max length sentense:" + "\n" + word + "\n" + text.Length.ToString() + " words" + "\n" + "Srednee kolichestvo: " + sred.ToString();

        }
    }
}
