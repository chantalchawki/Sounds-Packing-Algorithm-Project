using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;
using Sounds_Packing_Algorithm_Project.Classes;

namespace Sounds_Packing_Algorithm_Project
{
    class ALGORITHM
    {

        //WORST FIT (LINEAR SEARCH)//

        public static void Worst_Fit_Linear()
        {
            Timer.Start();

            MainWindow.WorstFitLinearIsRunning = true;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Linear";
            //number of folders
            int Num_Folder = 1;

            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;
            //set maximum to move to folder with maximum remaining duration
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + Num_Folder);
            if (Duration.Count == 0)
            {
                Duration.Add(MainWindow.ListofTime[0].Item1);
                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.Number_Of_Audio_Files; i++)
            {
                max = 0;
                //check if there's a space in the current folders (before creating a new folder)
                //get the folder with maximum remainig duration
                a = false;
                for (int j = 0; j < Duration.Count; j++)
                {
                    if (MainWindow.Seconds_Per_Folder - Duration[j] >= MainWindow.ListofTime[i].Item1)
                    {
                        if (MainWindow.Seconds_Per_Folder - Duration[j] > max)
                        {
                            max = MainWindow.Seconds_Per_Folder - Duration[j];
                            maxnum = j;
                        }
                        a = true;
                    }
                }
                //if there's space, move to folder with most remaining space
                if (a == true)
                {
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + (maxnum + 1) + @"\" + pos, true);

                    // File.Move("Audios/" + (), "Audios/F" + (maxnum + 1) + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    f = new FileStream(finalpath + @"\F" + (maxnum + 1) + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    sw.Close();
                    f.Close();
                    Duration[maxnum] += MainWindow.ListofTime[i].Item1;
                }

                //if there's no space, create a new folder
                if (a == false)
                {
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Num_Folder++;
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + Num_Folder);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    // File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < Num_Folder; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }
            Timer.Stop();
            MessageBox.Show("Worst Fit (Linear) time: " + Timer.ElapsedMilliseconds.ToString());
            Timer.Reset();
            MessageBox.Show("Worst Fit (Linear) is done.");
            MainWindow.WorstFitLinearIsRunning = false;
        }



        // WORST FIT (PRIORITY QUEUE)//
        public static void Worst_Fit_Priority_Queue()
        {
            Timer.Start();
            MainWindow.WorstFitPQIsRunning = true;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_PriorityQueue";
            //number of folders
            int Num_Folder = 1;

            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders
            PriorityQueue p = new PriorityQueue();
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + Num_Folder);
            if (Duration.Count == 0)
            {
                p.Enqueue(MainWindow.ListofTime[0].Item1, Num_Folder);
                Duration.Add(MainWindow.ListofTime[0].Item1);


                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;

                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                //File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1));
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.Number_Of_Audio_Files; i++)
            {

                //if there's space, move to folder with most remaining space
                if (MainWindow.Seconds_Per_Folder - p.Peek() >= MainWindow.ListofTime[i].Item1)
                {
                    int q = p.Peek() + MainWindow.ListofTime[i].Item1;

                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + p.ReturnIndex() + @"\" + pos, true);

                    // File.Move("Audios/" + (), "Audios/F" + + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    f = new FileStream(finalpath + @"\F" + p.ReturnIndex() + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    Duration[p.ReturnIndex() - 1] += MainWindow.ListofTime[i].Item1;
                    p.Dequeue();
                    p.Enqueue(q, Num_Folder);
                    sw.Close();
                    f.Close();
                }

                //if there's no space, create a new folder
                else
                {
                    Num_Folder++;
                    p.Enqueue(MainWindow.ListofTime[i].Item1, Num_Folder);
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + Num_Folder);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);

                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos);


                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < Num_Folder; l++)
            {
                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            Timer.Stop();
            MessageBox.Show("Worst_Fit (PQ) time: " + Timer.ElapsedMilliseconds.ToString());
            MessageBox.Show("Worst Fit (PQ) is Done.");
            MainWindow.WorstFitPQIsRunning = false;
        }


        //WORST FIT DEACREASING (LINEAR SEARCH)//

        public static void Worst_Fit_Decreasing_Linear()
        {
            MainWindow.WorstFitLinearDecreasingIsRunning = true;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Deacreasing_Linear";
            //number of folders
            int Num_Folder = 1;
            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;
            //set maximum to move to folder with maximum remaining duration
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + Num_Folder);
            if (Duration.Count == 0)
            {
                Duration.Add(MainWindow.ListofTime[0].Item1);

                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                //File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1));
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.Number_Of_Audio_Files; i++)
            {
                max = 0;
                //check if there's a space in the current folders (before creating a new folder) 
                //get the folder with maximum remainig duration
                a = false;
                for (int j = 0; j < Duration.Count; j++)
                {
                    if (MainWindow.Seconds_Per_Folder - Duration[j] >= MainWindow.ListofTime[i].Item1)
                    {
                        if (MainWindow.Seconds_Per_Folder - Duration[j] > max)
                        {
                            max = MainWindow.Seconds_Per_Folder - Duration[j];
                            maxnum = j;
                        }
                        a = true;
                    }
                }
                //if there's space, move to folder with most remaining space
                if (a == true)
                {
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;

                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + (maxnum + 1) + @"\" + pos, true);
                    //File.Move("Audios/" + (), "Audios/F" +  + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    f = new FileStream(finalpath + @"\F" + (maxnum + 1) + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    sw.Close();
                    f.Close();
                    Duration[maxnum] += MainWindow.ListofTime[i].Item1;
                }

                //if there's no space, create a new folder
                if (a == false)
                {
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Num_Folder++;
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + Num_Folder);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);

                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    // File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < Num_Folder; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MainWindow.WorstFitLinearDecreasingIsRunning = false;
        }

        //WORST FIT DECREASING (PRIORITY QUEUE)//

        //Folder Names according to the method
        //Folder Path Entered by user
        public static Stopwatch Timer = new Stopwatch();
        public static void Worst_Fit_Decreasing_Priority_Queue()
        {
        
            MainWindow.WorstFitPQDecreasingIsRunning = true;
            //sort the time in seconds in descending order
            //MainWindow.ListofTime.Sort();
            List<Tuple<int, int>> tempListOfTime= new List<Tuple<int,int>>() ;
            
            SortAlgorithmThreading sorter = new SortAlgorithmThreading(MainWindow.ListofTime);
            sorter.MergeSort();
            sorter.getlist(ref tempListOfTime);
            
            MainWindow.ListofTime.Reverse();
            //number of folders
            int Num_Folder = 1;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Decreasing_PriorityQueue";

            //create a new folder
            Directory.CreateDirectory( finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders
            PriorityQueue p = new PriorityQueue();
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath +@"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + Num_Folder);
            if (Duration.Count == 0)
            {
                p.Enqueue(MainWindow.ListofTime[0].Item1, Num_Folder);
                Duration.Add(MainWindow.ListofTime[0].Item1);
                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
               
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.Number_Of_Audio_Files; i++)
            {

                //if there's space, move to folder with most remaining space
                if (MainWindow.Seconds_Per_Folder- p.Peek() >= MainWindow.ListofTime[i].Item1)
                {
                    int q = p.Peek() + MainWindow.ListofTime[i].Item1;
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" +pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    
                    f = new FileStream(finalpath + @"\F" + p.ReturnIndex() + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    Duration[p.ReturnIndex() - 1] += MainWindow.ListofTime[i].Item1;
                    p.Dequeue();
                    p.Enqueue(q, Num_Folder);
                    sw.Close();
                    f.Close();
                }

                //if there's no space, create a new folder
                else
                {
                    Num_Folder++;
                    p.Enqueue(MainWindow.ListofTime[i].Item1, Num_Folder);
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + Num_Folder);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                  
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < Num_Folder; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MainWindow.WorstFitPQDecreasingIsRunning = false;
        }
    

        //FIRST FIT DECREASING (LINEAR SEARCH)//

        public static void First_Fit_Decreasing()
        {
            MainWindow.FirstFitDecreasingIsRunning = true;
            Timer.Start();

            //copy the original lists to a temp 
            List<Tuple<string , string >> Mylist = new List<Tuple<string , string >>(MainWindow.Mylist.Capacity);
            for ( int i = 0; i <MainWindow.Mylist.Count; i++)
            {
                Mylist.Add(MainWindow.Mylist[i]);
            }
            

            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);

            //multithreading merge sort
            SortAlgorithmThreading sorter = new SortAlgorithmThreading( MainWindow.ListofTime );
            sorter.MergeSort();

            //ref -> returns a sorted copy in ListofTime
            sorter.getlist( ref ListofTime);

            //sort the time in seconds in descending order
            ListofTime.Reverse();

            //Folder's number 
            int Num_Folder = 1;

            //copying from mainwindow 
            int Seconds = MainWindow.Seconds_Per_Folder;
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox
            string firstpath = MainWindow.FolderPath; 

            //Path entered in textbox + folder with the method name = where "F" should be created
            string finalPath = MainWindow.FolderPath + @"\First_Fit_Decreasing";

            //create a new folder
            Directory.CreateDirectory(finalPath+@"\F" + Num_Folder);

            
            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;

            //always move the first audio in the created folder
            FileStream f = new FileStream(finalPath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + Num_Folder);

            if (Duration.Count == 0)
            {
                Duration.Add(ListofTime[0].Item1);
                string tempname = Mylist[ListofTime[0].Item2].Item1;
               
                File.Copy(firstpath + @"\" + tempname, finalPath + @"\F" + Num_Folder + @"\" + tempname, true);
                sw.WriteLine(Mylist[0].Item1 + " " + Mylist[0].Item2);
                sw.Close();
                f.Close();
            }

            
            for (int i = 1; i < Num_Audios ; i++)
            {
                //check if there's a space in the current folders (before creating a new folder)
                for (int j = 0; j < Duration.Count; j++)
                {
                    a = false;
                    //if there is a folder with remaining space, move file to it then break the loop
                    if (Seconds - Duration[j] >= ListofTime[i].Item1)
                    {

                        string tempname = Mylist[ListofTime[i].Item2].Item1;
                       
                        File.Copy(firstpath + '\\' + tempname, finalPath + "\\F" + (j+1) + "\\" + tempname, true);

                        f = new FileStream(finalPath+"\\F" + (j + 1) + "_METADATA.txt", FileMode.Append);
                        sw = new StreamWriter(f);
                        sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                        sw.Close();
                        f.Close();
                        Duration[j] += ListofTime[i].Item1;
                        a = true;
                        break;
                    }
                }

                //if there's no space, create a new folder
                if (a == false)
                {
                    Duration.Add(ListofTime[i].Item1);
                    Num_Folder++;
                    Directory.CreateDirectory(finalPath + @"\F" + Num_Folder);
                    f = new FileStream(finalPath+@"\F" + Num_Folder+ "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + Num_Folder);
                    sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                    string tempname = Mylist[ListofTime[i].Item2].Item1;
                    File.Copy(firstpath + @"\" + tempname, finalPath + @"\F" + Num_Folder + @"\" + tempname, true);

                    
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < Num_Folder; l++)
            {

                f = new FileStream(finalPath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }
            Timer.Stop();
            MessageBox.Show("First Fit Decreasing time: " + Timer.ElapsedMilliseconds.ToString());
            MessageBox.Show("First Fit Decreasing done.");

            MainWindow.FirstFitDecreasingIsRunning = false;
        }

        //BEST FIT//
        public static void Best_Fit()
        {
            Timer.Start();
            int UserInput = MainWindow.Seconds_Per_Folder; //getting how many seconds the user want , O(1)
            string finalpath = MainWindow.FolderPath + @"\Best_Fit"; //seting the final path to move the audio files in it , O(1)
            List<string> AudioNames = new List<string>();
            string path = MainWindow.FolderPath;
            for (int i = 0; i < MainWindow.AudioNames.Count; i++)
            {
                AudioNames.Add(MainWindow.AudioNames[i]);
            }
            List<Tuple<int, int>> AudioInfo = new List<Tuple<int, int>>();
            for(int i = 0; i < MainWindow.AudioInfo.Count; i++)
            {
                AudioInfo.Add(MainWindow.AudioInfo[i]);
            }
            if (Directory.Exists(finalpath))
            {

                Directory.Delete(finalpath,true);
            }
            Directory.CreateDirectory(finalpath);
            int NumOfFolders = 1;
            List<int> RemainigTime = new List<int>(); //contains remainig time in each folder
            int minindex = -1;
            FileStream METADATA;
            StreamWriter METADATAWRITER;
            int minumumtime = 1000;
            for (int i = 0; i < AudioInfo.Count; i++)
            {

                for (int j = 0; j < RemainigTime.Count; j++)
                {
                    if (AudioInfo[i].Item1 <= RemainigTime[j] && minumumtime > RemainigTime[j])
                    {
                        minindex = j;
                        minumumtime = RemainigTime[j];
                    }
                }

                if (minindex == -1)
                {

                    Directory.CreateDirectory(finalpath + @"\F" + NumOfFolders);//creating the first folder with will contain at least one audio
                    METADATA = new FileStream(finalpath + @"\F" + NumOfFolders + "_METADATA.txt", FileMode.OpenOrCreate);
                    METADATAWRITER = new StreamWriter(METADATA);
                    METADATAWRITER.WriteLine("F" + NumOfFolders);
                    TimeSpan temo = new TimeSpan();
                    temo = TimeSpan.FromSeconds(AudioInfo[i].Item1);
                    METADATAWRITER.WriteLine(AudioNames[i] + " " + temo);

                    METADATAWRITER.Close();
                    METADATA.Close();
                    File.Copy(path+ @"\" + AudioNames[i], finalpath + @"\F" + NumOfFolders + @"\" + AudioNames[i]);
                    RemainigTime.Add(UserInput - AudioInfo[i].Item1);
                    NumOfFolders++;
                }
                else
                {
                    METADATA = new FileStream(finalpath + @"\F" + (minindex + 1) + "_METADATA.txt", FileMode.Append);
                    METADATAWRITER = new StreamWriter(METADATA);
                    TimeSpan temo = new TimeSpan();
                    temo = TimeSpan.FromSeconds(AudioInfo[i].Item1);
                    METADATAWRITER.WriteLine(AudioNames[i] + " " + temo);
                    METADATAWRITER.Close();
                    METADATA.Close();
                    File.Copy(path+ @"\" + AudioNames[i], finalpath + @"\F" + (minindex + 1) + "/" + AudioNames[i]);
                    RemainigTime[minindex] -= AudioInfo[i].Item1;
                }
                minindex = -1;
                minumumtime = 1000;
            }
            for (int i = 0; i < RemainigTime.Count; i++)
            {
                METADATA = new FileStream(finalpath + @"\F" + (i + 1) + "_METADATA.txt", FileMode.Append);
                METADATAWRITER = new StreamWriter(METADATA);
                TimeSpan temo = new TimeSpan();
                temo = TimeSpan.FromSeconds(UserInput - RemainigTime[i]);
                METADATAWRITER.WriteLine(temo.ToString());
                METADATAWRITER.Close();
                METADATA.Close();
            }
            Timer.Stop();
            MessageBox.Show("Best Fit time: " + Timer.ElapsedMilliseconds.ToString());
            MessageBox.Show("Best Fit is done.");
            MainWindow.BestFitIsRunning = false;
        }

        //FOLDER FILLING///
        public static void Folder_Filling()
        {
            MainWindow.FolderFillingIsRunning = true;
            FileStream fs;

            StreamWriter sw;


            int index;

            int WeightLeft;

            int pathindex;

            string tempfilename;

            TimeSpan tempfiletime = new TimeSpan();


            string metadatapath;

            Tuple<int, int> tempTuple;

            string tempfilesourcepath;

            string tempfiletargetpath;
            //Input text file path O(1)
            string readpath = MainWindow.FolderPath+@"\AudiosInfo.txt";
            //Input music files path O(1)
            string filesourcepath = MainWindow.FolderPath  ;
            // msh 3arf leh hnrga3lo 
            string targetfolderpath = "";
            //get the desired amount to listen in folder
            
            int w = MainWindow.Seconds_Per_Folder;
            
            // copy the data from the main window
            List<Tuple<int, int>> DurationAndIndexList = new List<Tuple<int, int>>();
            List<Tuple <string , string > > FilesNames = new List<Tuple <string , string > >();
            //FilesNames.Add(Tuple.Create("",""));
            DurationAndIndexList.Add(Tuple.Create(0,0));
            //O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {

                FilesNames.Add(MainWindow.Mylist[i]);
                
                DurationAndIndexList.Add(MainWindow.ListofTime[i]);
            }
            MessageBox.Show("Coping data to Folder Filling algorithm done.");
            
            // index for the folder to rename it with it's index
            int counter = 0;
            // msh 3arf bta3 eh bs hashof
            int test = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
                long[,] t = new long[DurationAndIndexList.Count + 1, w + 1];
            List < List < int > > folders  = new List<List<int>>();
           
            while (DurationAndIndexList.Count > 1)
            {
                // by entering here that mean we still have more files to copy so we have to make new folder
                // increase the folder indexer to make new folder
                counter++;
                
                // Base case of recursion O(W)
                // fill the table for each size we have with the first music file ( we made it to make it 1 based ) with 0  , O(W)
                for (int i = 0; i <= w; i++)
                    t[0, i] = 0;
                // fill the table for each sound file we can't put it in 0 space O(N )
                for (int i = 0; i <= DurationAndIndexList.Count; i++)
                    t[i,0] = 0;
                //build table O(N*W)
                
                //loop from first file to last file remaning but we are 1 based so we will go for l.count -1
                // since the outer loop O(N)
                // and the inner loop O(W)
                //so the total order for them is O(N*W)
                for (int i = 1; i < DurationAndIndexList.Count; i++)
                {
                    for (int j= 1 ; j <= w ; j++)
                    {
                        //comparison O(1)
                        if (DurationAndIndexList[i].Item1 > j)
                        {
                            // assign  O(1)
                            t[i, j] = t[i - 1, j];
                        }
                        else
                        {
                            // assign  O(1) and comparison O(1) 
                            t[i, j] = (t[i - 1, j] > t[i - 1, j - DurationAndIndexList[i].Item1] + DurationAndIndexList[i].Item1) ? t[i - 1, j] : t[i - 1, j - DurationAndIndexList[i].Item1] +DurationAndIndexList[i].Item1;
                        }
                    }
                }
                

                //Get path / get the used files O(N)
                index = DurationAndIndexList.Count - 1;
                WeightLeft = w;
                List <int > PathToCopyAtTheEnd = new List<int>();
                List < int > pathtodelete = new List<int>() ;
                //see which audios we should copy O(N)
                while (index!=0 && WeightLeft!=0)
                {
                    // we cannot take this audio 
                    // comparison O(1)
                    if (WeightLeft<DurationAndIndexList[index].Item1)
                    {
                        // jump to the next audio O(1)
                        index--;
                    }
                    else
                    {
                        // comparison to if we skipped this audio better than taking it
                        // comparison and accesing O(1)
                        if (t[index - 1, WeightLeft] > t[index - 1, WeightLeft - DurationAndIndexList[index].Item1] + DurationAndIndexList[index].Item1)
                        {
                            // jump to the previous audio O(1)
                            index--;
                        }
                        else
                        {
                            // taking this audio in our folder
                            // adding to list O(1)
                            // worst case if the size of this list reached it's limit capacity so it will expand
                            // will take O(N) once . because all the expands will be N+N/2+N/4+N/8 = O(N)

                            PathToCopyAtTheEnd.Add(DurationAndIndexList[index].Item2);
                            pathtodelete.Add(index);
                            // since we took this audio we should subtract it from our folder 
                            WeightLeft -= DurationAndIndexList[index].Item1;
                            // jump to the previous audio
                            index--;
                        }
                    }
                }
                for (int i = 0; i < pathtodelete.Count; i++)
                {
                    pathindex = pathtodelete[i];
                    tempTuple  = DurationAndIndexList[pathindex];
                    DurationAndIndexList[pathindex] = DurationAndIndexList[DurationAndIndexList.Count - 1];
                    DurationAndIndexList.RemoveAt(DurationAndIndexList.Count - 1);
                }
                folders.Add( new List<int>() );
                for (int i = 0; i < PathToCopyAtTheEnd.Count; i++ )
                {

                    folders[folders.Count-1].Add(PathToCopyAtTheEnd[i]);
                }
            }
            MessageBox.Show("algorithm finished at " + stopwatch.ElapsedMilliseconds.ToString());
                int totalsecondsOfFolder = 0;
                //total complexity will be O(N) because at the worst case we will create folder for each file
                //iterate on each folder we have to create O(M) which at maximum will equal N so we can say O(N)
                for (int i = 0; i < folders.Count; i++)
                {
                    // get the new folder path
                    targetfolderpath = MainWindow.FolderPath + @"\Folder_filling\F" + (i + 1).ToString();
                    // create new folder
                    System.IO.Directory.CreateDirectory(targetfolderpath);
                    metadatapath = MainWindow.FolderPath + @"\Folder_filling\F" + (i+1).ToString() + "_METADATA.txt";
                    // open file stream to write the meta data of each folder
                    fs = new FileStream(metadatapath, FileMode.OpenOrCreate);
                    // open stream writer to write the meta data of each folder
                    sw = new StreamWriter(fs);
                    // write at the begining of the meta data the folder name
                    sw.WriteLine('F' + (i + 1).ToString());
                    totalsecondsOfFolder = 0;
                    //iterato on each file the folder have which will be maximum O(N)
                    for (int j = 0; j < folders[i].Count; j++)
                    {
                        totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Second;
                        totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Minute * 60;
                        totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Hour * 3600;
                        // write at the meta data of the folder
                        sw.WriteLine(FilesNames[folders[i][j]].Item1 + ' ' + FilesNames[folders[i][j]].Item2);
                        // get the source path
                        tempfilesourcepath = filesourcepath + @"\" + FilesNames[folders[i][j]].Item1;
                        // get the target path
                        tempfiletargetpath = targetfolderpath + @"\" + FilesNames[folders[i][j]].Item1;
                        // copy from source to target
                        System.IO.File.Copy(tempfilesourcepath, tempfiletargetpath, true);
                    }
                    tempfiletime = TimeSpan.FromSeconds(totalsecondsOfFolder);
                    sw.WriteLine(tempfiletime.ToString());
                    sw.Close();
                    fs.Close();
                }
                

            stopwatch.Stop();
            MessageBox.Show("Folder Filling Time: "+stopwatch.ElapsedMilliseconds.ToString());
            MainWindow.FolderFillingIsRunning = false;
            }
        }
    }

