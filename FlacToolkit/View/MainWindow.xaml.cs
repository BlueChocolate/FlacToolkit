using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ControlzEx.Theming;
using FlacToolkit.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
#nullable disable

namespace FlacToolkit.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        MainViewModel vm;
        string baseColor;
        public MainWindow()
        {
            InitializeComponent();
            //同步系统主题
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            //设置数据上下文
            vm = new MainViewModel();
            DataContext = vm;

            //获取当前的 BaseColor 
            baseColor = ThemeManager.Current.DetectTheme(this).BaseColorScheme;
        }

        private void ChangeLightDark(object sender, RoutedEventArgs e)
        {
            if (baseColor == "Dark")
            {
                ThemeManager.Current.ChangeThemeBaseColor(this, "Light");
                baseColor = "Light";
            }
            else if (baseColor == "Light")
            {
                ThemeManager.Current.ChangeThemeBaseColor(this, "Dark");
                baseColor = "Dark";
            }
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    vm.Flacs.Add(new Model.Flac(System.IO.Path.GetFileName(filename)));
            }

        }
    }
}
