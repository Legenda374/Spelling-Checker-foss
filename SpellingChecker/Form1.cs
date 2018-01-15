using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SpellingChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
        }

        private string request(string Data)
        {
            WebRequest req = WebRequest.Create("https://speller.yandex.net/services/spellservice/checkText?text=" + Data.Replace(' ', '+'));
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            XDocument response = XDocument.Parse(request(richTextBox1.Text));
            //var a = response.Root.Elements("error").Elements("word");
            //a.XmlText();

            string[] dic = new[] { }; // Слова сюда

            for (int i = 0; i < dic.Length; i++)
            {
                int index = richTextBox1.Text.IndexOf(dic[i]);

                richTextBox1.Select(index, dic[i].Length);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
                richTextBox1.SelectionColor = Color.Red;
            }
        }
    }
}
