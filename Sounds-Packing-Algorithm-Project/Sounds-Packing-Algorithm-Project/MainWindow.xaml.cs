using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using win = System.Windows.Forms; //included to use folders


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
        public static string FilePath;
        public static string FolderPath;
        bool validatePaths()
        {

            if (File_Path_TextBox.Text.Length != 0 && Folder_Path_Textbox.Text.Length != 0 && NumberOfSec.Text.Length != 0)
            {
                FilePath = File_Path_TextBox.Text.ToString();
                FolderPath = Folder_Path_Textbox.Text.ToString();
                return true;
            }
            return false;
        }
        private void readfile()
        {
            ListofTime = new List<Tuple<int, int>>();
            Mylist = new List<Tuple<string, string>>();
            //number entered by user for specific duration in each folder
            num = int.Parse(NumberOfSec.Text);

            //file containing info about audios
            FileStream fs = new FileStream(File_Path_TextBox.Text, FileMode.Open);
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

                //////////CHANGED//////
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

            if (ofd.ShowDialog() == true)
            {
                string filepath = ofd.FileName;
                File_Path_TextBox.Text = filepath;
            }


        }

        private void Open_Folder(object sender, RoutedEventArgs e)
        {
            win.FolderBrowserDialog FD = new win.FolderBrowserDialog();
            FD.ShowNewFolderButton = false;
            FD.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            win.DialogResult res = FD.ShowDialog();

            if (res == win.DialogResult.OK)
            {
                string folderpath = FD.SelectedPath;
                Folder_Path_Textbox.Text = folderpath;

            }

        }



        DispatcherTimer dt = new DispatcherTimer();

        public void StartTimer()
        {
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += ticker;
            dt.Start();

        }

        public void StopTimer()
        {
            dt.Stop();
        }

        int tim = 0;

        private void ticker(object sender, EventArgs e)
        {
            tim++;
            TimerLabel.Content = tim.ToString();
        }

        public static List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>();
        public static List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>();
        public static int num;
        public static int x;
        bool WorstFitLinearIsRunning = false;
        bool WorstFitPQIsRunning = false;
        bool WorstFitLinearDecreasingIsRunning = false;
        bool WorstFitPQDecreasingIsRunning = false;
        bool FolderFillingIsRunning = false;
        bool FirstFitDecreasingIsRunning = false;
        bool BestFitIsRunning = false;
        //WORST FIT (LINEAR)
        void WorstFitLinearByThreadingSecondStep()
        {
            WorstFitLinearIsRunning = true;
            ALGORITHM.Worst_Fit_Linear();
            MessageBox.Show("files copied using Worst fit");
            WorstFitLinearIsRunning = false;
        }
        void WorstFitLinearByThreadingFirstStep()
        {

            Thread temp = new Thread(WorstFitLinearByThreadingSecondStep);
            temp.Start();

        }


        private void WorstFit1_Click(object sender, RoutedEventArgs e)
        {
            if (WorstFitLinearIsRunning == true)
            {
                MessageBox.Show("you are already using this method");
                return;
            }
            if (validatePaths() == true)
            {
                readfile();
                WorstFitLinearByThreadingFirstStep();


            }
            else
                MessageBox.Show("Please Enter Paths ");
        }
        void WorstFitPQByThreadingSecondStep()
        {
            WorstFitPQIsRunning = true;
            ALGORITHM.Worst_Fit_Priority_Queue();
            MessageBox.Show("files copied using Worst fit PQ");
            WorstFitPQIsRunning = false;
        }
        void WorstFitPQByThreadingFirstStep()
        {

            Thread temp = new Thread(WorstFitPQByThreadingSecondStep);
            temp.Start();

        }
        // WORST FIT (PRIORITY QUEUE)
        private void WorstFit2_Click(object sender, RoutedEventArgs e)
        {
            if (WorstFitPQIsRunning == true)
            {
                MessageBox.Show("This method is already used");
                return;
            }
            if (validatePaths() == true)
            {
                readfile();
                WorstFitPQByThreadingFirstStep();
                //NumberOfSec.Text = "";
            }
            else
                MessageBox.Show("Please Select Paths ");

        }

        //WORST FIT DECREASING (LINEAR)
        private void WorstFitD1_Click(object sender, RoutedEventArgs e)
        {
            if (validatePaths() == true)
            {
                readfile();
                ALGORITHM.Worst_Fit_Decreasing_Linear();
                System.Windows.Forms.MessageBox.Show("Audio files moved.");
                //NumberOfSec.Text = "";

            }
            else
                MessageBox.Show("Please Select Paths ");
        }

        //WORST FIT DECREASING (PRIORITY QUEUE)
        private void WorstFitD2_Click(object sender, RoutedEventArgs e)
        {
            if (validatePaths() == true)
            {
                readfile();
                ALGORITHM.Worst_Fit_Decreasing_Priority_Queue();
                MessageBox.Show("Audio files moved.");
                //NumberOfSec.Text = "";
            }
            else
                MessageBox.Show("Please Select Paths ");
        }

        //FIRST FIT DECREASING
        private void FirstFit_Click(object sender, RoutedEventArgs e)
        {
            if (validatePaths() == true)
            {

                readfile();

                ALGORITHM.First_Fit_Decreasing();

                MessageBox.Show("Audio files moved.");
                //  NumberOfSec.Text = "";

            }
            else MessageBox.Show("Pleas Select Paths ");
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void BestFit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
