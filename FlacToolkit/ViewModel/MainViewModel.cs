#nullable disable
using FlacToolkit.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FlacToolkit.Model;
using System.Collections.ObjectModel;

namespace FlacToolkit.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region 命令
        public RelayCommand TestCommand { get => testCommand; set => testCommand = value; }
        public RelayCommand CancelCommand { get => cancelCommand; set => cancelCommand = value; }
        public RelayCommand SetWorkspaceCommand
        {
            get
            {
                if (setWorkspaceCommand == null)
                {
                    setWorkspaceCommand = new RelayCommand(new Action<object>(o =>
                    {
                        flacs.Add(new Flac(DateTime.Now.ToString()));
                    }));
                }
                return setWorkspaceCommand;
            }
        }

        public RelayCommand OpenDebugWindowCommand {
            get
            {
                if (openDebugWindowCommand==null)
                {
                    openDebugWindowCommand=new RelayCommand(new Action<object>(o =>
                    {
                        debugWindowManage.CreateWindow();
                    }));
                }
                return openDebugWindowCommand;
            }
        }
        #endregion

        /// <summary>
        /// 工作区路径列表
        /// </summary>
        public ObservableCollection<string> WorkspacePaths
        {
            get => workspacePaths; set { workspacePaths = value; RaisePropertyChanged("WorkspacePaths"); }
        }

        /// <summary>
        /// Flac音乐文件列表
        /// </summary>
        public ObservableCollection<Flac> Flacs
        {
            get => flacs; set { flacs = value; RaisePropertyChanged("Flacs"); }
        }

        public Flac CurrentFlac
        {
            get => currentFlac; set { currentFlac = value; RaisePropertyChanged("CurrentFlac"); }
        }

        private readonly DebugWindowManage debugWindowManage;
        private RelayCommand testCommand;
        private RelayCommand cancelCommand;
        private RelayCommand setWorkspaceCommand;
        private RelayCommand openDebugWindowCommand;
        private ObservableCollection<Flac> flacs;
        private ObservableCollection<string> workspacePaths;
        private Flac currentFlac;

        public MainViewModel()
        {
            debugWindowManage=new DebugWindowManage();
            flacs = new ObservableCollection<Flac>();
            workspacePaths = new ObservableCollection<string>();
            flacs.Add(new Flac("jb"));
            flacs.Add(new Flac("abc"));


        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        private string shit;

        public string Shit { get => shit; set => SetProperty(ref shit, value); }
    }
}