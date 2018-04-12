using System;
using Microsoft.Win32;
        
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Sounds_Packing_Algorithm_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void readfile()
        {
            ListofTime = new List<Tuple<int, int>>();
            Mylist = new List<Tuple<string, string>>();
            //WORST FIT(priority queue)//
            //number entered by user for specific duration in each folder
            num = int.Parse(NumberOfSec.Text);

            //file containing info about audios
            FileStream fs = new FileStream("AudiosInfo.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            //list containing name of audio file , duration (string)

            //list containing duartion of audio file in SECONDS , index for each audio

            string[] records;
            string[] fields;


            string name;
            int time;
            string strTime;

            //x is the number of audios in text file
            x = int.Parse(sr.ReadLine());
            int index = 0;

            while (sr.Peek() != -1)
            {
                //each record is an audio file writen in a new line
                records = sr.ReadLine().Split('\n');

                for (int i = 0; i < 1; i++)
                {
                    //each record has 2 fields (name and time)
                    fields = records[i].Split(' ');

                    name = fields[0];

                    //convert the total time to seconds
                    time = DateTime.Parse(fields[1]).Second + (DateTime.Parse(fields[1]).Minute) * 60 + (DateTime.Parse(fields[1]).Hour) * 3600;

                    //total time as a string
                    strTime = fields[1];

                    //pair of strings (audio name and time)
                    Tuple<string, string> MyTup = new Tuple<string, string>(name, strTime);

                    //pair of integers (audio time in seconds , index for each audio)
                    Tuple<int, int> MyTup2 = new Tuple<int, int>(time, index);

                    Mylist.Add(MyTup);
                    ListofTime.Add(MyTup2);

                    index++;

                }
            }
            sr.Close();
            fs.Close();

        }
        private void Open_File_Explorer_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            //  ofd.ShowDialog();

            if (ofd.ShowDialog() == true)
            {
                Path_TextBox.Text = ofd.FileName;
            }

        }

        public static List <Tuple<int, int>> ListofTime = new List<Tuple<int, int>>();
            public static List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>();
            public static int num ;
        public static int x;
        //WORST FIT LINEAR
        private void WorstFit1_Click(object sender, RoutedEventArgs e)
        {
            readfile();
            ALGORITHM.Worst_Fit_Linear();
            MessageBox.Show("Audio files moved.");
            NumberOfSec.Text = "";
        }

        // WORST FIT USING PRIORITY QUEUE
        private void WorstFit2_Click(object sender, RoutedEventArgs e)
        {
            readfile();
            ALGORITHM.Worst_Fit_Priority_Queue();
            MessageBox.Show("Audio files moved.");
            NumberOfSec.Text = "";
        }
        //WORST FIT DECREASING (LINEAR)
        private void WorstFitD1_Click(object sender, RoutedEventArgs e)
        {
            readfile();
            ALGORITHM.Worst_Fit_Decreasing_Linear();
            MessageBox.Show("Audio files moved.");
            NumberOfSec.Text = "";
        }
        //WORST FIT DECREASING (PRIORITY QUEUE)
        private void WorstFitD2_Click(object sender, RoutedEventArgs e)
        {
            readfile();
            ALGORITHM.Worst_Fit_Decreasing_Priority_Queue();
            MessageBox.Show("Audio files moved.");
            NumberOfSec.Text = "";
        }

        private void FirstFit_Click(object sender, RoutedEventArgs e)
        {
            readfile();
            ALGORITHM.First_Fit_Decreasing();
            MessageBox.Show("Audio files moved.");
            NumberOfSec.Text = "";
        }
    }
}
