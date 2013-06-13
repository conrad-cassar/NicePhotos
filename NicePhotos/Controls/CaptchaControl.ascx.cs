using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using SpeechLib;

namespace NicePhotos.Controls
{
    public enum Complexity { Low, Medium, High };

    public partial class CaptchaControl : System.Web.UI.UserControl
    {
        private string phrase = "";
        private string phrase2 = "";
        private List<SolidBrush> brushes;
        private Random rand = new Random();
        private int xPos;
        private int charAmount;
        private Complexity complexityLevel;
        private int imageWidth;
        private object[] captchaSettings = { 10, Complexity.High };

        public Complexity ComplexityLevel
        {
            get { return complexityLevel; }
            set { complexityLevel = value; }
        }

        public int CharAmount
        {
            get { return charAmount; }
            set { charAmount = value; }
        }

        public string Phrase
        {
            get { return phrase; }
        }

        public string Phrase2
        {
            get { return phrase2; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["captchaSettings"] != null)
            {
                captchaSettings = (object[])HttpContext.Current.Session["captchaSettings"];
            }

            if (!Page.IsPostBack)
            {
                if (CharAmount < 1)
                    CharAmount = 5;

                imageWidth = charAmount * 30;
                captchaContainer.Width = (imageWidth).ToString() + "px";

                captchaSettings[0] = CharAmount;
                captchaSettings[1] = ComplexityLevel;
                HttpContext.Current.Session["captchaSettings"] = captchaSettings;

                Refresh();
            }
            else
            {
                if (HttpContext.Current.Session["captcha"] != null && HttpContext.Current.Session["captcha2"] != null)
                {
                    phrase = (string)HttpContext.Current.Session["captcha"];
                    phrase2 = (string)HttpContext.Current.Session["captcha2"];
                    charAmount = (int)captchaSettings[0];
                    complexityLevel = (Complexity)captchaSettings[1];

                    imageWidth = charAmount * 30;
                }
            }
        }

        protected void imbListen_Click(object sender, ImageClickEventArgs e)
        {
            SpVoice speaker = new SpVoice();

            if (phrase2 != "")
            {
                foreach (char c in phrase2)
                {
                    speaker.Speak(c.ToString());
                }
            }
            else
            {
                speaker.Speak("Unable to read text.");
            }
        }

        private string GenerateImage()
        {
            #region Phrase to Image

            rand = new Random();
            xPos = 0;
            Bitmap bmpOut = new Bitmap(imageWidth, 50);
            Graphics g = Graphics.FromImage(bmpOut);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.Beige, 0, 0, (30 * charAmount), 50);

            foreach (char c in phrase)
            {
                g.DrawString(c.ToString(), new Font("Action Jackson", 30), brushes.ElementAt(rand.Next(9)), xPos, rand.Next(20));
                xPos += 28;
            }

            #endregion

            #region Distortion

            int compValue;
            switch (complexityLevel)
            {
                case Complexity.Low:
                    compValue = 1;
                    break;
                case Complexity.Medium:
                    compValue = 2;
                    break;
                case Complexity.High:
                    compValue = 3;
                    break;
                default:
                    compValue = 2;
                    break;
            }

            for (int i = 0; i <= compValue * 2; i++)
            {
                g.DrawLine(new Pen(Color.BlueViolet), 0, rand.Next(50) + 4, 30 * charAmount, rand.Next(50) + 4);
            }

            #endregion

            #region Image Conversion Process

            MemoryStream ms = new MemoryStream();
            bmpOut.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imgBytes = ms.ToArray();
            bmpOut.Dispose();
            ms.Close();
            return "data:;base64," + Convert.ToBase64String(imgBytes);

            #endregion
        }

        private string GeneratePhrase()
        {
            xPos = 0;
            Bitmap bmpOut = new Bitmap(imageWidth, 50);

            string tmp = "";
            for (int i = 0; i < charAmount; i++)
            {
                tmp += GetRandomLetter();
            }

            return tmp;
        }

        private void SetPhrses()
        {
            phrase = GeneratePhrase();
            HttpContext.Current.Session["captcha"] = phrase;

            phrase2 = GeneratePhrase();
            HttpContext.Current.Session["captcha2"] = phrase2;
        }

        private char GetRandomLetter()
        {
            return (char)('a' + rand.Next(25));
        }

        public void Refresh()
        {
            #region Build brush list
            brushes = new List<SolidBrush>();
            brushes.Add(new SolidBrush(Color.Brown));
            brushes.Add(new SolidBrush(Color.Blue));
            brushes.Add(new SolidBrush(Color.Coral));
            brushes.Add(new SolidBrush(Color.Cyan));
            brushes.Add(new SolidBrush(Color.DarkOrange));
            brushes.Add(new SolidBrush(Color.DarkOliveGreen));
            brushes.Add(new SolidBrush(Color.Red));
            brushes.Add(new SolidBrush(Color.Purple));
            brushes.Add(new SolidBrush(Color.DeepSkyBlue));
            brushes.Add(new SolidBrush(Color.MistyRose));
            #endregion

            SetPhrses();
            imgCaptch.ImageUrl = GenerateImage();
        }

        protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            Refresh();
        }

        internal bool HumanityCheck(string phrase)
        {
            return phrase == Session["captcha"].ToString() || phrase == Session["captcha2"].ToString();
        }
    }
}