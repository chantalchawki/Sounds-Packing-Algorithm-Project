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
        //WORST FIT DECREASING PRIORITY QUEUE//
        
        //Folder Names according to the method
        //Folder Path Entered by user

        public static void Worst_Fit_Decreasing_Priority_Queue()
        {

            MainWindow.WorstFitPQDecreasingIsRunning = true;
            //sort the time in seconds in descending order
            //MainWindow.ListofTime.Sort();
            List<Tuple<int, int>> tempListOfTime= new List<Tuple<int,int>>() ;
            
            SortAlgorithmThreading sorter = new SortAlgorithmThreading(MainWindow.ListofTime);
            sorter.MergeSortUsingThreadingFirstStep(0, MainWindow.ListofTime.Count - 1);
            sorter.getlist(ref tempListOfTime);
            
            MainWindow.ListofTime.Reverse();
            //number of folders
            int c = 1;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Decreasing_PriorityQueue";

            //create a new folder
            Directory.CreateDirectory( finalpath + @"\F" + c);

            //list of duration in the created folders
            PriorityQueue p = new PriorityQueue();
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath +@"\F" + c + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + c);
            if (Duration.Count == 0)
            {
                p.Enqueue(MainWindow.ListofTime[0].Item1, c);
                Duration.Add(MainWindow.ListofTime[0].Item1);
                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c + @"\" + pos, true);
               
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.x; i++)
            {

                //if there's space, move to folder with most remaining space
                if (MainWindow.num - p.Peek() >= MainWindow.ListofTime[i].Item1)
                {
                    int q = p.Peek() + MainWindow.ListofTime[i].Item1;
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" +pos, finalpath + @"\F" + c + @"\" + pos, true);
                    
                    f = new FileStream(finalpath + @"\F" + p.ReturnIndex() + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    Duration[p.ReturnIndex() - 1] += MainWindow.ListofTime[i].Item1;
                    p.Dequeue();
                    p.Enqueue(q, c);
                    sw.Close();
                    f.Close();
                }

                //if there's no space, create a new folder
                else
                {
                    c++;
                    p.Enqueue(MainWindow.ListofTime[i].Item1, c);
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Directory.CreateDirectory(finalpath + @"\F" + c);
                    f = new FileStream(finalpath + @"\F" + c + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + c);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + (c) + @"\" + pos, true);
                  
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < c; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MainWindow.WorstFitPQDecreasingIsRunning = false;
        }
    
        //WORST FIT (LINEAR)//

        public static void Worst_Fit_Linear()
        {
            
            MainWindow.WorstFitLinearIsRunning = true;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Linear";
            //number of folders
            int c = 1;

            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + c);

            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;
            //set maximum to move to folder with maximum remaining duration
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath + @"\F" + c + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + c);
            if (Duration.Count == 0)
            {
                Duration.Add(MainWindow.ListofTime[0].Item1);
                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c + @"\" + pos, true);
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.x; i++)
            {
                max = 0;
                //check if there's a space in the current folders (before creating a new folder)
                //get the folder with maximum remainig duration
                a = false;
                for (int j = 0; j < Duration.Count; j++)
                {
                    if (MainWindow.num - Duration[j] >= MainWindow.ListofTime[i].Item1)
                    {
                        if (MainWindow.num - Duration[j] > max)
                        {
                            max = MainWindow.num - Duration[j];
                            maxnum = j;
                        }
                        a = true;
                    }
                }
                //if there's space, move to folder with most remaining space
                if (a == true)
                {
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath +@"\F" + (maxnum + 1) + @"\" + pos, true);

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
                    c++;
                    Directory.CreateDirectory(finalpath+ @"\F" + c);
                    f = new FileStream(finalpath +@"\F" + c + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + c);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos , finalpath+ @"\F" + c + @"\" + pos , true);
                   // File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < c; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MessageBox.Show("Worst Fit Linear Is Done");
            MainWindow.WorstFitLinearIsRunning = false ;
        }


        //WORST FIT DEACREASING (LINEAR SEARCH)

        public static void Worst_Fit_Decreasing_Linear()
        {
            MainWindow.WorstFitLinearDecreasingIsRunning = true; 
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_Deacreasing_Linear";
            //number of folders
            int c = 1;
            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + c);
            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;
            //set maximum to move to folder with maximum remaining duration
            int max = 0, maxnum = 0;
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath +@"\F" + c + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + c);
            if (Duration.Count == 0)
            {
                Duration.Add(MainWindow.ListofTime[0].Item1);

                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c +@"\"+ pos, true);
                //File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1));
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.x; i++)
            {
                max = 0;
                //check if there's a space in the current folders (before creating a new folder) 
                //get the folder with maximum remainig duration
                a = false;
                for (int j = 0; j < Duration.Count; j++)
                {
                    if (MainWindow.num - Duration[j] >= MainWindow.ListofTime[i].Item1)
                    {
                        if (MainWindow.num - Duration[j] > max)
                        {
                            max = MainWindow.num - Duration[j];
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
                    c++;
                    Directory.CreateDirectory(finalpath + @"\F" + c);
                    f = new FileStream(finalpath + @"\F" + c + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + c);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);

                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c +@"\"+ pos, true);
                   // File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < c; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MainWindow.WorstFitLinearDecreasingIsRunning = false; 
        }

       // WORST FIT USING PRIORITY QUEUE
        public static void Worst_Fit_Priority_Queue()
            {
            
                MainWindow.WorstFitPQIsRunning = true;
            string finalpath = MainWindow.FolderPath + @"\Worst_Fit_PriorityQueue";
            //number of folders
            int c = 1;

            //create a new folder
            Directory.CreateDirectory(finalpath + @"\F" + c);

            //list of duration in the created folders
            PriorityQueue p = new PriorityQueue();
            List<int> Duration = new List<int>();
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalpath + @"\F" + c + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + c);
            if (Duration.Count == 0)
            {
                p.Enqueue(MainWindow.ListofTime[0].Item1, c);
                Duration.Add(MainWindow.ListofTime[0].Item1);


                string pos = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;

                File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c + @"\" + pos, true);
                //File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1));
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.x; i++)
            {

                //if there's space, move to folder with most remaining space
                if (MainWindow.num - p.Peek() >= MainWindow.ListofTime[i].Item1)
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
                    p.Enqueue(q, c);
                    sw.Close();
                    f.Close();
                }

                //if there's no space, create a new folder
                else
                {
                    c++;
                    p.Enqueue(MainWindow.ListofTime[i].Item1, c);
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    Directory.CreateDirectory(finalpath + @"\F" + c);
                    f = new FileStream(finalpath + @"\F" + c + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + c);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);

                    string pos = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + pos, finalpath + @"\F" + c + @"\" + pos);

                    //File.Move("Audios/" + (), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < c; l++)
            {

                f = new FileStream(finalpath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }


            MessageBox.Show("Worst Fit PQ Is Done");
            MainWindow.WorstFitPQIsRunning = false ;
        }
        //FIRST FIT (DECREASING)
        public static void First_Fit_Decreasing()
        {
            MainWindow.FirstFitDecreasingIsRunning = true;
            
            //Directory.CreateDirectory(MainWindow.FolderPath+@"\First_Fit_Decreasing");

            //sort the time in seconds in descending order
            MainWindow.ListofTime.Sort();
            MainWindow.ListofTime.Reverse();

            //number of folders
            int c = 1;
            string finalPath = MainWindow.FolderPath + @"\First_Fit_Decreasing";
            //create a new folder
            Directory.CreateDirectory(finalPath+@"\F" + c);

            //Directory.CreateDirectory(finalPath+"/F" + c);
            //list of duration in the created folders
            List<int> Duration = new List<int>();
            bool a = false;
            //always move the first audio in the created folder
            FileStream f = new FileStream(finalPath + @"\F" + c + "_METADATA.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine("F" + c);
            if (Duration.Count == 0)
            {
                Duration.Add(MainWindow.ListofTime[0].Item1);
                string tempname = MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1;
                //File.Move("Audios/" + (MainWindow.Mylist[MainWindow.ListofTime[0].Item2].Item1), "Audios/F" + c + "/" + ());
                File.Copy(MainWindow.FolderPath + @"\" + tempname, finalPath + @"\F" + c + @"\" + tempname, true);
                sw.WriteLine(MainWindow.Mylist[0].Item1 + " " + MainWindow.Mylist[0].Item2);
                sw.Close();
                f.Close();
            }
            for (int i = 1; i < MainWindow.x; i++)
            {
                //check if there's a space in the current folders (before creating a new folder)
                for (int j = 0; j < Duration.Count; j++)
                {
                    a = false;
                    //if there is a folder with remaining space, move file to it then break the loop
                    if (MainWindow.num - Duration[j] >= MainWindow.ListofTime[i].Item1)
                    {

                        string tempname = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                        //File.Move("Audios/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1), "Audios/F" + (j + 1) + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                        File.Copy(MainWindow.FolderPath + '\\' + tempname, finalPath + "\\F" + (j+1) + "\\" + tempname, true);

                        f = new FileStream(finalPath+"\\F" + (j + 1) + "_METADATA.txt", FileMode.Append);
                        sw = new StreamWriter(f);
                        sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                        sw.Close();
                        f.Close();
                        Duration[j] += MainWindow.ListofTime[i].Item1;
                        a = true;
                        break;
                    }
                }

                //if there's no space, create a new folder
                if (a == false)
                {
                    Duration.Add(MainWindow.ListofTime[i].Item1);
                    c++;
                    Directory.CreateDirectory(finalPath + @"\F" + c);
                    f = new FileStream(finalPath+@"\F" + c + "_METADATA.txt", FileMode.Append);
                    sw = new StreamWriter(f);
                    sw.WriteLine("F" + c);
                    sw.WriteLine(MainWindow.Mylist[i].Item1 + " " + MainWindow.Mylist[i].Item2);
                    string tempname = MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1;
                    File.Copy(MainWindow.FolderPath + @"\" + tempname, finalPath + @"\F" + (c) + @"\" + tempname, true);

                    //File.Move("Audios/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1), "Audios/F" + c + "/" + (MainWindow.Mylist[MainWindow.ListofTime[i].Item2].Item1));
                    sw.Close();
                    f.Close();
                }
            }
            for (int l = 0; l < c; l++)
            {

                f = new FileStream(finalPath + @"\F" + (l + 1) + "_METADATA.txt", FileMode.Append);
                sw = new StreamWriter(f);
                sw.WriteLine(TimeSpan.FromSeconds(Duration[l]).ToString());
                sw.Close();
                f.Close();
            }

            MainWindow.FirstFitDecreasingIsRunning = false;
        }
        // Folder Filling
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
            
            int w = MainWindow.num;
            
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
            MessageBox.Show("coping data to Folder Filling algorithm done");
            
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
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
            MainWindow.FolderFillingIsRunning = false;
            }
        }
    }

