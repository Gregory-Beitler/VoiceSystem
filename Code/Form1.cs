using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Web;

namespace Voice_System
{
       
    public partial class Form1 : Form
    {
        //builds variables for weather
        string Temp;
        string con;
        string hum;
        string wind;
        string town;
        string tfCond;
        string tfHigh;
        string tfLow;

        //Uses the second code
        Form2 f2 = new Form2();

        //Builds the stuff we need for voice recognition
        SpeechSynthesizer Snyth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine spEngine = new SpeechRecognitionEngine();

        public Form1()
        {
            InitializeComponent();
        }
  
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                spEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { 
            
            }
            button1.Enabled = false;
           
            
        }

        //The function to open weather
        void Weather()
        {
            //Gets the link
            string query = String.Format("http://weather.yahooapis.com/forecastrss?w=2459115");

            //Makes an xml variable
            XmlDocument weather = new XmlDocument();

            //Load the query
            weather.Load(query);

            //Manager the weather file
            XmlNamespaceManager manager = new XmlNamespaceManager(weather.NameTable);

            //Goes into yweather
            manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            //Skips rss and channel
            XmlNode chl = weather.SelectSingleNode("rss").SelectSingleNode("channel");

            //Goes to rss/channel/item/yweather:forecast
            XmlNodeList nodes = weather.SelectNodes("/rss/channel/item/yweather:forecast", manager);

            //Temp, condensation and humidity are stores
            Temp = chl.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
            con = chl.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
            hum = chl.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;


            //Gets your town/city and it will have a high/low and condition (raining, snowing etc.)
            town = chl.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;
            tfCond = chl.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["text"].Value;
            tfHigh = chl.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value;
            tfLow = chl.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value;
        }

        //The logic when the computer recognizes voice (See Readme.txt)
        private void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text) { 
                case "hello":
                                        
                    textBox1.Text = "";
                    textBox1.Text = "Hey, how are you doing?";
                    Snyth.Speak("Hey, how are you doing?");
                    break;

                case "hi":
                    textBox1.Text = "";
                    textBox1.Text = "Hey, how are you doing?";
                    Snyth.Speak("Hey, how are you doing?");
                    break;

                case "bye":

                    textBox1.Text = "";
                    textBox1.Text = "EXITING";
                    Snyth.Speak("farewell");
                    Environment.Exit(0);
                    break;

                case "good bye":

                    textBox1.Text = "";
                    textBox1.Text = "EXITING";
                    Snyth.Speak("farewell");
                    Environment.Exit(0);
                    break;
               
                case "shutdown":

                    textBox1.Text = "";
                    textBox1.Text = "EXITING";
                    Snyth.Speak("farewell");
                    Environment.Exit(0);
                    break;

                case "how is the weather":
                    textBox1.Text = "";
                    Weather();
                    textBox1.Text = "The weather in " + town + " is " + con + " at " + Temp + " degrees. Also a humidity of " + hum;
                    Snyth.Speak("The weather in " + town + " is " + con + " at " + Temp + " degrees. Also a humidity of " + hum);
                    break;

                case "what will the weather be like tomorrow":
                    Weather();
                    textBox1.Text = "Tomorrow it wil be " + tfCond + " with a high of " + tfHigh + " and a low of " + tfLow;
                    Snyth.Speak("Tomorrow it wil be " + tfCond + " with a high of " + tfHigh + " and a low of " + tfLow);                    
                    break;
                case "open w n y c":
                    textBox1.Text = "Opening wnyc";
                    Snyth.Speak("Opening WNYC");
                    System.Diagnostics.Process.Start("https://www.wnyc.org/radio/#streams/wnyc-fm939");
                    break;
                case "about":
                    spEngine.RecognizeAsyncStop();
                    button1.Enabled = true;
                    button2.Enabled = false;
                    AboutBox1 ab = new AboutBox1();
                    ab.Show();
                    Thread.Sleep(1000);
                    break;
                case "open a program":
                    Form2 f2 = new Form2();
                    f2.Show();
                    break;

              
            }
        }

        //Stops the voice system
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            spEngine.RecognizeAsyncStop();
        }

        //When Form1 lauches (the window)
        private void Form1_Load(object sender, EventArgs e)
        {
            Choices cmd = new Choices();
            cmd.Add(new string[] { "hello", "hi", "bye", "exit", "shutdown", "how is the weather", "what will the weather be like tomorrow", "open w n y c", "about" , "open a program"});
            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(cmd);
            Grammar g = new Grammar(gbuilder);
            spEngine.LoadGrammarAsync(g);
            spEngine.SetInputToDefaultAudioDevice();
            spEngine.SpeechRecognized += recEngine_SpeechRecognized;
            
        }



    }
}
