using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SpellingChecker
{
    public partial class Form1 : Form
    {

        XDocument response;
        string[] error;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = "Пришло теплое лето. На лисной опушки распускаюца колоколчики, незабутки, шыповник. Белые ромашки пратягивают к сонцу свои нежные лепески. Вылитают из уютных гнёзд птинцы. У зверей взраслеет смена. Мидвежата старше всех. Они радились еще холодной зимой в берлоги. Теперь они послушно следуют за строгай матерью. Рыжые лесята весело играют у нары. А кто мелькает в сасновых ветках? Да это лофкие бельчята совершают свои первые высотные прышки. В сумерках выходят на охоту колючии ежата. Не обижайте лесных малышей. Приходите в лес верными друзями.";
        }

        private string request(string Data)
        {
            try
            {
                WebRequest req = WebRequest.Create("https://speller.yandex.net/services/spellservice/checkText?text=" + Data.Replace(' ', '+'));
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string Out = sr.ReadToEnd();
                sr.Close();
                return Out;
            }
            catch
            {
                MessageBox.Show("Error request");
                return "<Error>";
            }
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            try
            {
                response = XDocument.Parse(request(richTextBox1.Text));
            }
            catch
            {
                return;
            }
            
            error = response.Element("SpellResult").Elements("error").Elements("word").Select(s => s.Value).ToArray();
            comboBox1.Items.AddRange(error);



            for (int i = 0; i < error.Length; i++)
            {
                int index = richTextBox1.Text.IndexOf(error[i]);

                richTextBox1.Select(index, error[i].Length);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
                richTextBox1.SelectionColor = Color.Red;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var values = response.XPathSelectElements("//word[text()='" + comboBox1.Text + "']")
     .Select(el => new
     {
         Values = el.XPathSelectElements("following-sibling::s")
                     .Select(s => s.Value).ToArray()
     })
     .ToArray();
            listBox1.Items.AddRange(values[0].Values);
        }
    }
}
