using Sounds_Packing_Algorithm_Project.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Sounds_Packing_Algorithm_Project
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            updatetable();
        }

        public void updatetable()
        {
            
            
            //thelist();
            table.ItemsSource = thelist();
        }

        private List<HistoryClass> thelist()
        {
            List<HistoryClass> ret = new List<HistoryClass>();
            FileStream fs = new FileStream("HistoryFile.txt", FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string[] args = new string[6];
            while (sr.Peek() != -1)
            {
                for (int i = 0 ; i < 6 ; i++)
                {
                    args[i] = sr.ReadLine();
                }
                HistoryClass temp = new HistoryClass()
                {
                    Algorithm_Name = args[0],
                    NumberOfAudios = args[1],
                    SecondsPerFolder = args[2],
                    NumberOfFolders = args[3],
                    ExecutionTime = args[4],
                    Date = args[5]
                };
                ret.Add(temp);
            }
            ret.Reverse();
            return ret;
            
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("HistoryFile.txt", FileMode.Truncate);
            fs.Close();
            table.ItemsSource = new List<HistoryClass>();
          
        }

        
    }
}
