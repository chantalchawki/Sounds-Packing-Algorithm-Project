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

        public static void writeAlgorithmInfo(string Name, int AudiosNum, int Seconds, int Folders, long excuetion)
        {
            FileStream fs = new FileStream("HistoryFile.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(Name);
            sw.WriteLine(AudiosNum.ToString());
            sw.WriteLine(Seconds.ToString());
            sw.WriteLine(Folders.ToString());
            sw.WriteLine(excuetion.ToString());
            sw.WriteLine(DateTime.Now);
            sw.Close();
            fs.Close();
        }
        //WORST FIT (LINEAR SEARCH)//
        public static void Worst_Fit_Linear()
        {
            // create new object O(1)
            Stopwatch Timer = new Stopwatch();
            //O(1)
            MainWindow.WorstFitLinearIsRunning = true;
            //
            Timer.Start();
            //O(1)
            List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>(MainWindow.Mylist.Capacity);
            //copy the original lists to a temp O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                Mylist.Add(MainWindow.Mylist[i]);
            }

            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.ListofTime.Count; i++)
            {
                ListofTime.Add(MainWindow.ListofTime[i]);
            }
            //Folder's number,O(1) 
            int Num_Folder = 1;

            //copying from mainwindow,O(1) 
            int Seconds = MainWindow.Seconds_Per_Folder;
            //copying from mainwindow,O(1)
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox,O(1)
            string firstpath = MainWindow.FolderPath;

            //Path entered in textbox + folder with the method name = where "F" should be created,O(1)
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Linear";

            //create a new folder,O(1)??
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders,O(1)
            List<int> Duration = new List<int>();
            //set maximum to move to folder with maximum remaining duration,O(1)
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder,O(1)??
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.OpenOrCreate);
            //O(1)
            StreamWriter sw = new StreamWriter(f);
            //O(1)
            sw.WriteLine("F" + Num_Folder);
            //O(1)
            Duration.Add(ListofTime[0].Item1);
            //O(1)
            string pos = Mylist[ListofTime[0].Item2].Item1;
            //O(1)??
            File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
            //O(1)??
            sw.WriteLine(Mylist[0].Item1 + " " + Mylist[0].Item2);
            //O(1)??
            sw.Close();
            //O(1)??
            f.Close();
            //O(N*M)
            for (int i = 1; i < Num_Audios; i++)
            {
                //O(1)
                max = -1;
                //check if there's a space in the current folders (before creating a new folder)
                //get the folder with maximum remainig duration
                //O(M)
                for (int j = 0; j < Duration.Count; j++)
                {
                    //O(1)
                    if (Seconds - Duration[j] >= ListofTime[i].Item1)
                    {
                        //O(1)
                        if (Seconds - Duration[j] > max)
                        {
                            //O(1)
                            max = Seconds - Duration[j];
                            //O(1)
                            maxnum = j;
                        }
                    }
                }
                //if there's space, move to folder with most remaining space
                //O(1)??
                if (max != -1)
                {
                    //O(1)
                    pos = Mylist[i].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + (maxnum + 1) + @"\" + pos, true);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + (maxnum + 1) + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                    //O(1)??
                    Duration[maxnum] += ListofTime[i].Item1;
                }

                //if there's no space, create a new folder
                //O(1)??
                else
                {
                    //O(1)
                    Duration.Add(ListofTime[i].Item1);
                    //O(1)
                    Num_Folder++;
                    //O(1)??
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine("F" + Num_Folder);
                    //O(1)??
                    sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
            }
            //O(M)
            for (int l = 0; l < Num_Folder; l++)
            {
                //O(1)??
                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                sw = new StreamWriter(f);
                //O(1)??
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                f.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)??
            writeAlgorithmInfo("worst Fit Decreasing PQ", Num_Audios, Seconds, Num_Folder, Timer.ElapsedMilliseconds);
            //O(1)
            MessageBox.Show("Worst Fit (Linear) time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + Num_Folder);
            //O(1)??
            Timer.Reset();
            //O(1)
            MessageBox.Show("Worst Fit (Linear) is done.");
            //O(1)??
            MainWindow.WorstFitLinearIsRunning = false;
        }



        //WORST FIT (PRIORITY QUEUE)
        // O ( N*Log (M) )
        public static void Worst_Fit_Priority_Queue()
        {
            //O(1)??
            Stopwatch Timer = new Stopwatch();
            //O(1)??
            Timer.Start();
            //O(1)
            MainWindow.WorstFitPQIsRunning = true;

            //copy the original lists to a temp ,O(1)
            List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>(MainWindow.Mylist.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                //O(1)
                Mylist.Add(MainWindow.Mylist[i]);
            }
            //O(1)
            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.ListofTime.Count; i++)
            {
                //O(1)
                ListofTime.Add(MainWindow.ListofTime[i]);
            }
            //Folder's number,O(1) 
            int Num_Folder = 1;

            //copying from mainwindow O(1)
            int Seconds = MainWindow.Seconds_Per_Folder;
            //O(1)
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox , O(1)
            string firstpath = MainWindow.FolderPath;

            //Path entered in textbox + folder with the method name = where "F" should be created, O(1)
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_PriorityQueue";


            //create a new folder , O(1)??
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders
            //O(1)??
            PriorityQueue p = new PriorityQueue();
            //O(1)
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder,O(1)??
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            //O(1)??
            StreamWriter sw = new StreamWriter(f);
            //O(1)??
            sw.WriteLine("F" + Num_Folder);

            //O(1) the priority queue is empty
            p.Enqueue(ListofTime[0].Item1, Num_Folder);
            //O(1)
            Duration.Add(ListofTime[0].Item1);
            //O(1)
            string pos = Mylist[ListofTime[0].Item2].Item1;
            //O(1)??
            File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
            //O(1)??
            sw.WriteLine(Mylist[0].Item1 + " " + Mylist[0].Item2);
            //O(1)??
            sw.Close();
            //O(1)??
            f.Close();
            //O(N*log (M) )
            int q;
            for (int i = 1; i < Num_Audios; i++)
            {

                //if there's space, move to folder with most remaining space
                //O(Log (M) )
                if (Seconds - p.Peek() >= ListofTime[i].Item1)
                {
                    //O(1)
                    q = p.Peek() + ListofTime[i].Item1;
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + p.ReturnIndex() + @"\" + pos, true);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + p.ReturnIndex() + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                    //O(1)
                    Duration[p.ReturnIndex() - 1] += ListofTime[i].Item1;
                    //O(log (size) )
                    p.Dequeue();
                    //O(Log (size) )
                    p.Enqueue(q, Num_Folder);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
                //O(Log (M))
                //if there's no space, create a new folder
                else
                {
                    //O(1)
                    Num_Folder++;
                    //O(Log (M) ) 
                    p.Enqueue(ListofTime[i].Item1, Num_Folder);
                    //O(1)
                    Duration.Add(ListofTime[i].Item1);
                    //O(1)??
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine("F" + Num_Folder);
                    //O(1)??
                    sw.WriteLine(Mylist[i].Item1 + " " + Mylist[i].Item2);
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
            }
            //O(M)
            for (int l = 0; l < Num_Folder; l++)
            {
                //O(1)??
                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                sw = new StreamWriter(f);
                //O(1)??
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                f.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)??
            writeAlgorithmInfo("worst Fit Decreasing PQ", Num_Audios, Seconds, Num_Folder, Timer.ElapsedMilliseconds);
            //O(1)
            MessageBox.Show("Worst Fit (PQ) time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + Num_Folder);
            //O(1)
            Timer.Reset();
            //O(1)
            MessageBox.Show("Worst Fit (PQ) is done.");
            //O(1)
            MainWindow.WorstFitPQIsRunning = false;
        }


        //WORST FIT DEACREASING (LINEAR SEARCH)//

        public static void Worst_Fit_Decreasing_Linear()
        {
            //O(1)??
            Stopwatch Timer = new Stopwatch();
            //O(1)??
            Timer.Start();
            //O(1)
            MainWindow.WorstFitLinearDecreasingIsRunning = true;

            //copy the original lists to a temp 
            //O(1)
            List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>(MainWindow.Mylist.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                Mylist.Add(MainWindow.Mylist[i]);
            }
            //O(1)
            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);

            //multithreading merge sort
            //O(N)
            SortAlgorithmThreading sorter = new SortAlgorithmThreading(MainWindow.ListofTime);
            //O(N*Log(N))
            sorter.MergeSort();

            //ref -> returns a sorted copy in ListofTime
            //O(N)
            sorter.getlist(ref ListofTime);
            //O(N)
            ListofTime.Reverse();

            //sort the time in seconds in descending order

            //Folder's number 
            //O(1)
            int Num_Folder = 1;

            //copying from mainwindow 
            //O(1)
            int Seconds = MainWindow.Seconds_Per_Folder;
            //O(1)
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox
            //O(1)
            string firstpath = MainWindow.FolderPath;

            //Path entered in textbox + folder with the method name = where "F" should be created
            //O(1)
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Deacreasing_Linear";

            //create a new folder
            //O(1)??
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
            //list of duration in the created folders
            //O(1)
            List<int> Duration = new List<int>();
            //set maximum to move to folder with maximum remaining duration
            //O(1)
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder
            //O(1)??
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.OpenOrCreate);
            //O(1)??
            StreamWriter sw = new StreamWriter(f);
            //O(1)??
            sw.WriteLine("F" + Num_Folder);
            //O(1)     
            Duration.Add(ListofTime[0].Item1);
            //O(1)
            string pos = Mylist[ListofTime[0].Item2].Item1;
            //O(1)??
            File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
            //O(1)??
            sw.WriteLine(Mylist[ListofTime[0].Item2].Item1 + " " + Mylist[ListofTime[0].Item2].Item2);
            //O(1)??
            sw.Close();
            //O(1)??
            f.Close();
            //O(N*M)
            for (int i = 1; i < Num_Audios; i++)
            {
                //O(1)
                max = -1;
                //check if there's a space in the current folders (before creating a new folder) 
                //get the folder with maximum remainig duration
                //O(M)
                for (int j = 0; j < Duration.Count; j++)
                {
                    //O(1)
                    if (Seconds - Duration[j] >= ListofTime[i].Item1)
                    {
                        //O(1)
                        if (Seconds - Duration[j] > max)
                        {
                            //O(1)
                            max = Seconds - Duration[j];
                            //O(1)
                            maxnum = j;
                        }

                    }
                }
                //if there's space, move to folder with most remaining space
                //O(1)
                if (max != -1)
                {
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + (maxnum + 1) + @"\" + pos, true);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + (maxnum + 1) + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                    //O(1)
                    Duration[maxnum] += ListofTime[i].Item1;
                }

                //if there's no space, create a new folder
                //O(1)
                else
                {
                    //O(1)
                    Duration.Add(ListofTime[i].Item1);
                    //O(1)
                    Num_Folder++;
                    //O(1)??
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine("F" + Num_Folder);
                    //O(1)??
                    sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
            }
            //O(M)
            for (int l = 0; l < Num_Folder; l++)
            {

                //O(1)??
                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                sw = new StreamWriter(f);
                //O(1)??
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                f.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)??
            writeAlgorithmInfo("worst Fit Decreasing PQ", Num_Audios, Seconds, Num_Folder, Timer.ElapsedMilliseconds);
            //O(1)
            MessageBox.Show("Worst Fit Decreasing (Linear) time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + Num_Folder);
            //O(1)??
            Timer.Reset();
            //O(1)
            MessageBox.Show("Worst Fit Decreasing (Linear) is done.");
            //O(1)
            MainWindow.WorstFitLinearDecreasingIsRunning = false;
        }

        //WORST FIT DECREASING (PRIORITY QUEUE)//

        //Folder Names according to the method
        //Folder Path Entered by user
        public static void Worst_Fit_Decreasing_Priority_Queue()
        {
            //O(1)??
            Stopwatch Timer = new Stopwatch();
            //O(1)??
            Timer.Start();
            //O(1)
            MainWindow.WorstFitPQDecreasingIsRunning = true;
            //copy the original lists to a temp 
            //O(1)
            List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>(MainWindow.Mylist.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                //O(1)
                Mylist.Add(MainWindow.Mylist[i]);
            }
            //O(1)
            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);

            //multithreading merge sort
            //O(N)
            SortAlgorithmThreading sorter = new SortAlgorithmThreading(MainWindow.ListofTime);
            //N*Log(N)
            sorter.MergeSort();

            //ref -> returns a sorted copy in ListofTime
            //O(N)
            sorter.getlist(ref ListofTime);

            //sort the time in seconds in descending order
            //O(N)
            ListofTime.Reverse();

            //Folder's number 
            //O(1)
            int Num_Folder = 1;

            //copying from mainwindow 
            //O(1)
            int Seconds = MainWindow.Seconds_Per_Folder;
            //O(1)
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox
            //O(1)
            string firstpath = MainWindow.FolderPath;
            //O(1)
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Decreasing_PriorityQueue";

            //create a new folder
            //O(1)??
            Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);

            //list of duration in the created folders
            //O(1)
            PriorityQueue p = new PriorityQueue();
            //O(1)
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder
            //O(1)??
            FileStream f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            //O(1)??
            StreamWriter sw = new StreamWriter(f);
            //O(1)??
            sw.WriteLine("F" + Num_Folder);
            //O(Log(M))
            p.Enqueue(ListofTime[0].Item1, Num_Folder);
            //O(1)           
            Duration.Add(ListofTime[0].Item1);
            //O(1)
            string pos = Mylist[ListofTime[0].Item2].Item1;
            //O(1)??
            File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
            //O(1)??   
            sw.WriteLine(Mylist[ListofTime[0].Item2].Item1 + " " + Mylist[ListofTime[0].Item2].Item2);
            //O(1)??
            sw.Close();
            //O(1)??
            f.Close();
            //O(N*Log(N))
            for (int i = 1; i < Num_Audios; i++)
            {
                //if there's space, move to folder with most remaining space
                //O(Log(M))
                if (Seconds - p.Peek() >= ListofTime[i].Item1)
                {
                    //O(1)
                    int q = p.Peek() + ListofTime[i].Item1;
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + p.ReturnIndex() + @"\" + pos, true);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + p.ReturnIndex() + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);
                    //O(1)
                    Duration[p.ReturnIndex() - 1] += ListofTime[i].Item1;
                    //O(Log M)
                    p.Enqueue(q, p.ReturnIndex());
                    //O(Log(M))
                    p.Dequeue();
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }

                //if there's no space, create a new folder
                //O(Log(M))
                else
                {
                    //O(1)
                    Num_Folder++;
                    //O(Log(M))
                    p.Enqueue(ListofTime[i].Item1, Num_Folder);
                    //O(1)
                    Duration.Add(ListofTime[i].Item1);
                    //O(1)??
                    Directory.CreateDirectory(finalpath + @"\F" + Num_Folder);
                    //O(1)??
                    f = new FileStream(finalpath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine("F" + Num_Folder);
                    //O(1)??
                    sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);
                    //O(1)
                    pos = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + pos, finalpath + @"\F" + Num_Folder + @"\" + pos, true);
                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
            }
            //O(M)
            for (int l = 0; l < Num_Folder; l++)
            {
                //O(1)??
                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                sw = new StreamWriter(f);
                //O(1)??
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                f.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)??
            writeAlgorithmInfo("worst Fit Decreasing PQ", Num_Audios, Seconds, Num_Folder, Timer.ElapsedMilliseconds);

            //O(1)
            MessageBox.Show("Worst Fit Decreasing (PQ) time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + Num_Folder);
            //O(1)??
            Timer.Reset();
            //O(1)
            MessageBox.Show("Worst Fit Decreasing (PQ) is done.");
            //O(1)
            MainWindow.WorstFitPQDecreasingIsRunning = false;
        }


        //FIRST FIT DECREASING (LINEAR SEARCH)//

        public static void First_Fit_Decreasing()
        {
            //O(1)??
            Stopwatch Timer = new Stopwatch();
            //O(1)
            MainWindow.FirstFitDecreasingIsRunning = true;
            //O(1)??
            Timer.Start();

            //copy the original lists to a temp 
            //O(1)
            List<Tuple<string, string>> Mylist = new List<Tuple<string, string>>(MainWindow.Mylist.Capacity);

            //Total Loop : O(N)*O(1) = O(N)
            //O(N) , N is the number of Audio Files
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                //O(1) [List.Add() takes O(1) unless Count = Capacity]
                Mylist.Add(MainWindow.Mylist[i]);
            }

            //O(1)
            List<Tuple<int, int>> ListofTime = new List<Tuple<int, int>>(MainWindow.ListofTime.Capacity);

            //O(N)
            SortAlgorithmThreading sorter = new SortAlgorithmThreading(MainWindow.ListofTime);

            //multithreading merge sort
            //O(N*Log(N)) , N is the number of Audio Files
            sorter.MergeSort();

            //ref returns a sorted copy in ListofTime
            //O(N)
            sorter.getlist(ref ListofTime);

            //sort the time in seconds in descending order

            //O(N) , N is the number of Audio Files
            ListofTime.Reverse();


            //Folder's number [index]
            //O(1) 
            int Num_Folder = 1;

            //copying from mainwindow 
            //O(1)
            int Seconds = MainWindow.Seconds_Per_Folder;
            //O(1)
            int Num_Audios = MainWindow.Number_Of_Audio_Files;

            //Path enetered in textbox
            //O(1)
            string firstpath = MainWindow.FolderPath;

            //Path entered in textbox + folder with the method name = where "F" should be created
            //O(1)
            string finalPath = MainWindow.FolderPath + @"\First_Fit_Decreasing";

            //create a new folder
            //O(1)??
            Directory.CreateDirectory(finalPath + @"\F" + Num_Folder);


            //list of duration in the created folders
            //O(1)
            List<int> Duration = new List<int>();

            //always move the first audio in the created folder
            //O(1)??
            FileStream f = new FileStream(finalPath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
            //O(1)??
            StreamWriter sw = new StreamWriter(f);
            //O(1)??
            sw.WriteLine("F" + Num_Folder);
            //O(1)
            bool thereIsSpace = false;

            //O(1)
            Duration.Add(ListofTime[0].Item1);
            //O(1)
            string tempname = Mylist[ListofTime[0].Item2].Item1;
            //O(1)??
            File.Copy(firstpath + @"\" + tempname, finalPath + @"\F" + Num_Folder + @"\" + tempname, true);
            //O(1)??
            sw.WriteLine(Mylist[ListofTime[0].Item2].Item1 + " " + Mylist[ListofTime[0].Item2].Item2);
            //O(1)??
            sw.Close();
            //O(1)??
            f.Close();


            //Total loop : O(N*M) N is the number of Audio Files and M is the number of Folders
            //O(N) , N is the number of Audio Files
            for (int i = 1; i < Num_Audios; i++)
            {
                //check if there's a space in the current folders (before creating a new folder)
                //O(M) , M is the number of Folders
                for (int j = 0; j < Duration.Count; j++)
                {
                    //O(1)
                    thereIsSpace = false;
                    //if there is a folder with remaining space, move file to it then break the loop
                    //O(1) for comparison + O(1) for List.Item = O(1)
                    if (Seconds - Duration[j] >= ListofTime[i].Item1)
                    {
                        //O(1) for assignment + O(1) for List.Item
                        tempname = Mylist[ListofTime[i].Item2].Item1;
                        //O(1)??
                        File.Copy(firstpath + '\\' + tempname, finalPath + "\\F" + (j + 1) + "\\" + tempname, true);
                        //O(1)??
                        f = new FileStream(finalPath + "\\F" + (j + 1) + "_METADATA.txt", FileMode.Append);
                        //O(1)??
                        sw = new StreamWriter(f);
                        //O(1)??
                        sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);
                        //O(1)??
                        sw.Close();
                        //O(1)??
                        f.Close();

                        //O(1)
                        Duration[j] += ListofTime[i].Item1;
                        //O(1)
                        thereIsSpace = true;
                        break;
                    }
                }

                //if there's no space, create a new folder

                //O(1) for comparison
                if (thereIsSpace == false)
                {
                    //O(1) for List.Add
                    Duration.Add(ListofTime[i].Item1);

                    //O(1)
                    Num_Folder++;
                    //O(1)??
                    Directory.CreateDirectory(finalPath + @"\F" + Num_Folder);
                    //O(1)??
                    f = new FileStream(finalPath + @"\F" + Num_Folder + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    sw = new StreamWriter(f);
                    //O(1)??
                    sw.WriteLine("F" + Num_Folder);
                    //O(1)??
                    sw.WriteLine(Mylist[ListofTime[i].Item2].Item1 + " " + Mylist[ListofTime[i].Item2].Item2);

                    //O(1) for assignment
                    tempname = Mylist[ListofTime[i].Item2].Item1;
                    //O(1)??
                    File.Copy(firstpath + @"\" + tempname, finalPath + @"\F" + Num_Folder + @"\" + tempname, true);

                    //O(1)??
                    sw.Close();
                    //O(1)??
                    f.Close();
                }
            }

            //O(M) , M is the number of folders
            for (int l = 0; l < Num_Folder; l++)
            {
                //O(1)??
                f = new FileStream(finalPath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                sw = new StreamWriter(f);
                //O(1)??
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                f.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)
            MessageBox.Show("First Fit Decreasing time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + Num_Folder);
            //O(1)??
            Timer.Reset();
            //O(1)
            MessageBox.Show("First Fit Decreasing done.");
            //O(1)
            MainWindow.FirstFitDecreasingIsRunning = false;
        }

        //BEST FIT//
        public static void Best_Fit()
        {
            //Starting a timer for the function , O(1)??
            Stopwatch Timer = new Stopwatch();
            //O(1)??
            Timer.Start();
            //getting how many seconds the user want , O(1)
            int UserInput = MainWindow.Seconds_Per_Folder;
            //seting the final path to move the audio files in it , O(1)
            string finalpath = MainWindow.FolderPath + @"\Best_Fit";
            //creating a list to contain the audio names , O(1)
            List<string> AudioNames = new List<string>(MainWindow.AudioNames.Capacity);
            //O(1)
            string path = MainWindow.FolderPath;
            //O(N)
            for (int i = 0; i < MainWindow.AudioNames.Count; i++)
            {
                //O(1)
                AudioNames.Add(MainWindow.AudioNames[i]);
            }
            //a list contains the audio length and its index in its O(1)
            List<Tuple<int, int>> AudioInfo = new List<Tuple<int, int>>(MainWindow.AudioInfo.Capacity);
            //O(N)
            for (int i = 0; i < MainWindow.AudioInfo.Count; i++)
            {
                AudioInfo.Add(MainWindow.AudioInfo[i]);
            }
            //O(1)??
            if (Directory.Exists(finalpath))
            {
                //O(1)??
                Directory.Delete(finalpath, true);
            }

            //O(1)??
            Directory.CreateDirectory(finalpath);
            //O(1)
            int NumOfFolders = 1;
            //a list contains the remainig time in each folder , O(1)
            List<int> RemainigTime = new List<int>();
            //O(1)
            int minindex = -1;
            //O(1)
            FileStream METADATA;
            //O(1)
            StreamWriter METADATAWRITER;
            //O(1)
            int minumumtime = 1000;
            //O(1)??
            TimeSpan temo = new TimeSpan();
            //O(N*M)
            for (int i = 0; i < AudioInfo.Count; i++)
            {
                //O(M)
                for (int j = 0; j < RemainigTime.Count; j++)
                {
                    //O(1)
                    if (AudioInfo[i].Item1 <= RemainigTime[j] && minumumtime > RemainigTime[j])
                    {
                        //O(1)
                        minindex = j;
                        //O(1)
                        minumumtime = RemainigTime[j];
                    }
                }
                //O(1)
                if (minindex == -1)
                {
                    //O(1)??
                    Directory.CreateDirectory(finalpath + @"\F" + NumOfFolders);//creating the first folder with will contain at least one audio
                    //O(1)??
                    METADATA = new FileStream(finalpath + @"\F" + NumOfFolders + "_METADATA.txt", FileMode.OpenOrCreate);
                    //O(1)??
                    METADATAWRITER = new StreamWriter(METADATA);
                    //O(1)??
                    METADATAWRITER.WriteLine("F" + NumOfFolders);
                    //O(1)??
                    temo = TimeSpan.FromSeconds(AudioInfo[i].Item1);
                    //O(1)??
                    METADATAWRITER.WriteLine(AudioNames[i] + " " + temo);

                    //O(1)??
                    METADATAWRITER.Close();
                    //O(1)??
                    METADATA.Close();
                    //O(1)??
                    File.Copy(path + @"\" + AudioNames[i], finalpath + @"\F" + NumOfFolders + @"\" + AudioNames[i]);
                    //O(1)
                    RemainigTime.Add(UserInput - AudioInfo[i].Item1);
                    //O(1)
                    NumOfFolders++;
                }
                //O(1)
                else
                {
                    //O(1)??
                    METADATA = new FileStream(finalpath + @"\F" + (minindex + 1) + "_METADATA.txt", FileMode.Append);
                    //O(1)??
                    METADATAWRITER = new StreamWriter(METADATA);
                    //O(1)??
                    temo = TimeSpan.FromSeconds(AudioInfo[i].Item1);
                    //O(1)??
                    METADATAWRITER.WriteLine(AudioNames[i] + " " + temo);
                    //O(1)??
                    METADATAWRITER.Close();
                    //O(1)??
                    METADATA.Close();
                    //O(1)??
                    File.Copy(path + @"\" + AudioNames[i], finalpath + @"\F" + (minindex + 1) + "/" + AudioNames[i]);
                    //O(1)
                    RemainigTime[minindex] -= AudioInfo[i].Item1;
                }
                //O(1)
                minindex = -1;
                //O(1)
                minumumtime = 1000;
            }
            //O(M)
            for (int i = 0; i < RemainigTime.Count; i++)
            {
                //O(1)??
                METADATA = new FileStream(finalpath + @"\F" + (i + 1) + "_METADATA.txt", FileMode.Append);
                //O(1)??
                METADATAWRITER = new StreamWriter(METADATA);
                //O(1)??
                temo = TimeSpan.FromSeconds(UserInput - RemainigTime[i]);
                //O(1)??
                METADATAWRITER.WriteLine(temo.ToString());
                //O(1)??
                METADATAWRITER.Close();
                //O(1)??
                METADATA.Close();
            }
            //O(1)??
            Timer.Stop();
            //O(1)??
            writeAlgorithmInfo("Best Fit", AudioInfo.Count, UserInput, NumOfFolders, Timer.ElapsedMilliseconds);
            //O(1)
            MessageBox.Show("Best Fit time: " + Timer.ElapsedMilliseconds.ToString());
            //O(1)
            MessageBox.Show("Number of folders created: " + NumOfFolders);
            //O(1)??
            Timer.Reset();
            //O(1)
            MessageBox.Show("Best Fit is done.");
            //O(1)
            MainWindow.BestFitIsRunning = false;
        }

        //FOLDER FILLING///
        public static void Folder_Filling()
        {
            //O(1)
            MainWindow.FolderFillingIsRunning = true;
            //O(1)
            FileStream fs;
            //O(1)
            StreamWriter sw;

            //O(1)
            int index;
            //O(1)
            int WeightLeft;
            //O(1)
            int pathindex;
            //O(1)
            int numberOfAudios = MainWindow.Number_Of_Audio_Files;
            //O(1)??
            string tempfilename;
            //O(1)
            TimeSpan tempfiletime = new TimeSpan();

            //O(1)
            string metadatapath;
            //O(1)
            Tuple<int, int> tempTuple;
            //O(1)
            string tempfilesourcepath;
            //O(1)
            string tempfiletargetpath;
            //Input text file path O(1)
            string readpath = MainWindow.FolderPath + @"\AudiosInfo.txt";
            //Input music files path O(1)
            string filesourcepath = MainWindow.FolderPath;
            // msh 3arf leh hnrga3lo O(1)
            string targetfolderpath = "";
            //get the desired amount to listen in folder
            //O(1)
            int w = MainWindow.Seconds_Per_Folder;

            // list to copy the data from the main window O(1)
            List<Tuple<int, int>> DurationAndIndexList = new List<Tuple<int, int>>(numberOfAudios + 10);
            // list to copy the data from the main window O(1)
            List<Tuple<string, string>> FilesNames = new List<Tuple<string, string>>(numberOfAudios + 10);
            //O(1)            
            DurationAndIndexList.Add(Tuple.Create(0, 0));
            //O(N)
            for (int i = 0; i < MainWindow.Mylist.Count; i++)
            {
                //O(1)
                FilesNames.Add(MainWindow.Mylist[i]);
                //O(1)
                DurationAndIndexList.Add(MainWindow.ListofTime[i]);
            }
            //O(1)
            MessageBox.Show("Coping data to Folder Filling algorithm done.");

            // index for the folder to rename it with it's index
            //O(1)
            int counter = 0;
            //O(1)??
            Stopwatch stopwatch = new Stopwatch();
            //O(1)??
            stopwatch.Start();
            //O(1)
            long[,] t = new long[DurationAndIndexList.Count + 1, w + 1];
            //O(1)
            List<List<int>> folders = new List<List<int>>();
            //O(1)
            List<int> PathToCopyAtTheEnd;
            //O(1)
            List<int> pathtodelete;
            // Base case of recursion O(W)
            // fill the table for each size we have with the first music file ( we made it to make it 1 based ) with 0  , O(W)
            for (int i = 0; i <= w; i++)
                t[0, i] = 0;
            // fill the table for each sound file we can't put it in 0 space O(N )
            for (int i = 0; i <= DurationAndIndexList.Count; i++)
                t[i, 0] = 0;
            //O(N*N*W)
            //the while loop O(N)
            //the while loop statment O(N*W)
            //the total O(N*(N*W) )
            while (DurationAndIndexList.Count > 1)
            {
                // by entering here that mean we still have more files to copy so we have to make new folder
                // increase the folder indexer to make new folder
                //O(1)
                counter++;

                //build table O(N*W)

                //loop from first file to last file remaning but we are 1 based so we will go for l.count -1
                // since the outer loop O(N)
                // and the inner loop O(W)
                //so the total order for them is O(N*W)
                // The outer for loop O(N)
                // it's statment O(W)
                // the total O(N*(W))
                for (int i = 1; i < DurationAndIndexList.Count; i++)
                {
                    //O(W)
                    for (int j = 1; j <= w; j++)
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
                            t[i, j] = (t[i - 1, j] > t[i - 1, j - DurationAndIndexList[i].Item1] + DurationAndIndexList[i].Item1) ? t[i - 1, j] : t[i - 1, j - DurationAndIndexList[i].Item1] + DurationAndIndexList[i].Item1;
                        }
                    }
                }

                //O(1)
                index = DurationAndIndexList.Count - 1;
                //assign O(1)
                WeightLeft = w;
                // constructor O(1)
                PathToCopyAtTheEnd = new List<int>();
                // constructor O(1)
                pathtodelete = new List<int>();

                //see which audios we should copy O(N)
                while (index != 0 && WeightLeft != 0)
                {
                    // we cannot take this audio 
                    // comparison O(1)
                    if (WeightLeft < DurationAndIndexList[index].Item1)
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
                            // taking this audio in our folder
                            // adding to list O(1)
                            // worst case if the size of this list reached it's limit capacity so it will expand
                            // will take O(N) once . because all the expands will be N+N/2+N/4+N/8 = O(N)

                            pathtodelete.Add(index);
                            // since we took this audio we should subtract it from our folder O(1)
                            WeightLeft -= DurationAndIndexList[index].Item1;
                            // jump to the previous audio O(1)
                            index--;
                        }
                    }
                }
                // O(min (W,N) )
                for (int i = 0; i < pathtodelete.Count; i++)
                {
                    // assign O(1)
                    pathindex = pathtodelete[i];
                    // assign O(1)
                    tempTuple = DurationAndIndexList[pathindex];
                    // assign O(1)
                    DurationAndIndexList[pathindex] = DurationAndIndexList[DurationAndIndexList.Count - 1];
                    // remove at last  O(1) because there will be no shifting
                    DurationAndIndexList.RemoveAt(DurationAndIndexList.Count - 1);
                }
                //O(1)
                folders.Add(new List<int>());
                // O(min (W,N) 
                for (int i = 0; i < PathToCopyAtTheEnd.Count; i++)
                {
                    //O(1)
                    folders[folders.Count - 1].Add(PathToCopyAtTheEnd[i]);
                }
            }
            //O(1)
            MessageBox.Show("algorithm finished at " + stopwatch.ElapsedMilliseconds.ToString());

            //O(1)
            int totalsecondsOfFolder = 0;
            //total complexity will be O(N) because at the worst case we will create folder for each file
            //iterate on each folder we have to create O(M) which at maximum will equal N so we can say O(N)
            for (int i = 0; i < folders.Count; i++)
            {
                // get the new folder path 
                //O(1)
                targetfolderpath = MainWindow.FolderPath + @"\Folder_filling\F" + (i + 1).ToString();
                // create new folder
                //O(1)??
                System.IO.Directory.CreateDirectory(targetfolderpath);
                //O(1)
                metadatapath = MainWindow.FolderPath + @"\Folder_filling\F" + (i + 1).ToString() + "_METADATA.txt";
                // open file stream to write the meta data of each folder
                //O(1)??
                fs = new FileStream(metadatapath, FileMode.OpenOrCreate);
                // open stream writer to write the meta data of each folder
                //O(1)??
                sw = new StreamWriter(fs);
                // write at the begining of the meta data the folder name
                //O(1)??
                sw.WriteLine('F' + (i + 1).ToString());
                //O(1)
                totalsecondsOfFolder = 0;
                //iterate over each file the folder have which will be maximum O(N)
                for (int j = 0; j < folders[i].Count; j++)
                {
                    //O(1)
                    totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Second;
                    //O(1)
                    totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Minute * 60;
                    //O(1)
                    totalsecondsOfFolder += DateTime.Parse(FilesNames[folders[i][j]].Item2).Hour * 3600;
                    // write at the meta data of the folder
                    //O(1)??
                    sw.WriteLine(FilesNames[folders[i][j]].Item1 + ' ' + FilesNames[folders[i][j]].Item2);
                    // get the source path
                    //O(1)
                    tempfilesourcepath = filesourcepath + @"\" + FilesNames[folders[i][j]].Item1;
                    // get the target path
                    //O(1)
                    tempfiletargetpath = targetfolderpath + @"\" + FilesNames[folders[i][j]].Item1;
                    // copy from source to target
                    //O(1)??
                    System.IO.File.Copy(tempfilesourcepath, tempfiletargetpath, true);
                }
                //O(1)??
                tempfiletime = TimeSpan.FromSeconds(totalsecondsOfFolder);
                //O(1)??
                sw.WriteLine(tempfiletime.ToString());
                //O(1)??
                sw.Close();
                //O(1)??
                fs.Close();
            }

            //O(1)??
            stopwatch.Stop();
            //O(1)??
            MessageBox.Show("Folder Filling Time: " + stopwatch.ElapsedMilliseconds.ToString());
            //O(1)??
            MessageBox.Show("Number of folders created: " + folders.Count);
            //O(1)
            MainWindow.FolderFillingIsRunning = false;
            //O(1)??
            writeAlgorithmInfo("Folder Filling", numberOfAudios, w, folders.Count, stopwatch.ElapsedMilliseconds);
            //O(1)??
            stopwatch.Reset();
        }
    }
}

