using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MusicRenamer {

    public partial class MainWindow : Window {
        private Random rnd = new Random();
        private string path;
        private int DCount;
        private int maxInt;
        private List<int> ints;

        public MainWindow() {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            path = System.IO.Path.GetDirectoryName(args[0]);

        }

        private int GetRndFromTo() {
            return rnd.Next(0, 10);
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e) {
            FileInfo[] files = GetFiles();

            foreach (FileInfo f in files) {
                if (CheckFileName(f.Name)) {
                    string fname = RemoveDigits(f.Name);
                    fname = NormalizeFName(fname);
                    File.Move(f.FullName, f.DirectoryName + System.IO.Path.DirectorySeparatorChar + fname);
                }
            }

            MessageBox.Show("DONE", "GUNDONE", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            if (!Int32.TryParse(DigitsTxt.Text, out DCount)) return;
            if (DCount < 1) return;

            maxInt = (int)(Math.Pow(10, DCount) - 1);

            FileInfo[] files = GetFiles();
            ints = new List<int>();

            foreach (FileInfo f in files) {
                string fname;
                if (CheckFileName(f.Name)) {
                    fname = ReAddDigits(f.Name);
                } else {
                    fname = AddDigits(f.Name);
                }
                fname = NormalizeFName(fname);
                File.Move(f.FullName, f.DirectoryName + System.IO.Path.DirectorySeparatorChar + fname);
            }

            MessageBox.Show("DONE", "GUNDONE", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private FileInfo[] GetFiles() {
            DirectoryInfo dir = new DirectoryInfo(path);
            return dir.GetFiles("*.txt");
        }

        private bool CheckFileName(string fname) {
            return Regex.IsMatch(fname, @"^[0-9]+_");
        }

        private string NormalizeFName(string fname) {
            return fname.ToLower().Replace(' ', '_');
        }

        private string AddDigits(string fname) {
            return GetRandomNum().ToString().PadLeft(DCount, '0') + "_" + fname;
        }

        private string RemoveDigits(string fname) {
            return fname.Substring(fname.IndexOf('_') + 1);
        }

        private string ReAddDigits(string fname) {
            return AddDigits(RemoveDigits(fname));
        }

        private int GetRandomNum() {
            int res = 0;
            while (true) {
                res = rnd.Next(maxInt);
                if (!ints.Contains(res)) break;
            }
            ints.Add(res);
            return res;
        }
    }
}
