using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Sounds_Packing_Algorithm_Project
{
    class ALGORITHM
    {
        //WORST FIT DECREASING PRIORITY QUEUE//
        
        //Folder Names according to the method
        //Folder Path Entered by user

        public static void Worst_Fit_Decreasing_Priority_Queue()
        {
           

            //sort the time in seconds in descending order
            MainWindow.ListofTime.Sort();
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
        }
    
        //WORST FIT (LINEAR)//

        public static void Worst_Fit_Linear()
        {
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
        }


        //WORST FIT DEACREASING (LINEAR SEARCH)

        public static void Worst_Fit_Decreasing_Linear()
        {
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

        }

       // WORST FIT USING PRIORITY QUEUE
        public static void Worst_Fit_Priority_Queue()
            {
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
        }
        //FIRST FIT (DECREASING)
        public static void First_Fit_Decreasing()
        {
            
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
        }

    }
}
