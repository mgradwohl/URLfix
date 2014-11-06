using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URLFix
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string s in textBoxIn.Lines)
            {
                // trim out any crap
                String str = s;
                str = str.Trim();
                str = str.TrimEnd(',', '\\');
                str = str.ToLower();

                // create a URL
                str = "http://" + str;
                Uri u = new Uri(str);

                // trim subdomains off - first detect if this is a .com style or .co.kr style
                int cdots = 1;
                if (u.Host.EndsWith(".com") ||
                    u.Host.EndsWith(".org") ||
                    u.Host.EndsWith(".net") ||
                    u.Host.EndsWith(".edu"))
                {
                    cdots = 1;
                }
                else
                {
                    cdots = 2;
                }

                // split into the right parts and build a new string with only the .com/.co.kr and the domain
                String[] site = u.Host.Split('.');
                if (cdots == 1 || site.Length == 2)
                {
                    str = site[site.Length - 2] + '.' + site[site.Length - 1];
                }
                else
                {
                    str = site[site.Length - 3] + '.' + site[site.Length - 2] + '.' + site[site.Length - 1];
                }

                // add segments to the url if there are any msn.com/news/sports => /news/sports is the segment
                if (u.Segments.Length > 1)
                {
                    foreach (string t in u.Segments)
                    {
                        str += t;
                    }
                }

                // issue a web request with the IE11 UA string to see if the site exists
                try
                {
                    String wstr = "http://" + str;
                    HttpWebRequest wrGETURL;
                    wrGETURL = (HttpWebRequest) WebRequest.Create(wstr);
                    wrGETURL.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
                    wrGETURL.GetResponse();
                }
                catch (WebException ex)
                {
                    str += "\t" + ex.Status;
                }
                

                //Uri f = new Uri(str);
                textBoxOut.Text += str;
                textBoxOut.Text += "\r\n";
            }
        }
    }
}
