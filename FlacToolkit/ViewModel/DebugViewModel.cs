using FlacToolkit.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace FlacToolkit.ViewModel
{
    internal class DebugViewModel : ViewModelBase
    {
        private RelayCommand startCmdProcessCommand;
        private RelayCommand stopCmdProcessCommand;
        private RelayCommand sendCmdProcessCommand;
        private string cmdInputText;
        private string cmdOutputText;
        private string cmdErrorText;
        private bool isProcessRunning;
        Process cmdProcess = null;
        StreamWriter stdin = null;

        public string CmdInputText { get => cmdInputText; set => SetProperty(ref cmdInputText, value); }
        public string CmdOutputText { get => cmdOutputText; set { cmdOutputText = value; RaisePropertyChanged("CmdOutputText"); } }
        public string CmdErrorText { get => cmdErrorText; set { cmdErrorText = value; OnPropertyChanged(); } }


        public bool IsProcessRunning
        {
            get => isProcessRunning; set { isProcessRunning = value; RaisePropertyChanged("IsProcessRunning"); }
        }

        public RelayCommand StartCmdProcessCommand
        {
            get
            {
                if (startCmdProcessCommand == null)
                {
                    startCmdProcessCommand = new RelayCommand(new Action<object>(o =>
                    {
                        if (cmdProcess == null)
                        {
                            ProcessStartInfo pStartInfo = new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                //Batch File Arguments = "/C START /b /WAIT testbatch1.bat",
                                //Test: Arguments = "START /WAIT /K ipconfig /all",
                                Arguments = "START /WAIT",
                                WorkingDirectory = Environment.SystemDirectory,
                                // WorkingDirectory = Application.StartupPath,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                RedirectStandardInput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                            };

                            cmdProcess = new Process
                            {
                                StartInfo = pStartInfo,
                                EnableRaisingEvents = true,
                                // Test without and with this
                                // When SynchronizingObject is set, no need to BeginInvoke()
                                //SynchronizingObject = this
                            };

                            cmdProcess.Start();
                            cmdProcess.BeginErrorReadLine();
                            cmdProcess.BeginOutputReadLine();
                            stdin = cmdProcess.StandardInput;
                            stdin.AutoFlush = true;

                            cmdProcess.OutputDataReceived += (s, evt) =>
                            {
                                if (evt.Data != null)
                                {
                                    System.Windows.Application.Current.Dispatcher.Invoke((() =>
                                    {
                                        CmdOutputText += evt.Data + "\n";
                                    }));
                                }
                            };

                            cmdProcess.ErrorDataReceived += (s, evt) =>
                            {
                                if (evt.Data != null)
                                {
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        CmdOutputText += evt.Data + "\n";
                                    });
                                }
                            };

                            cmdProcess.Exited += (s, evt) =>
                            {
                                stdin?.Dispose();
                                cmdProcess?.Dispose();
                            };
                        }
                    }));
                }
                return startCmdProcessCommand;
            }
        }

        public RelayCommand StopCmdProcessCommand
        {
            get
            {
                if (stopCmdProcessCommand == null)
                {
                    stopCmdProcessCommand = new RelayCommand(new Action<object>(o =>
                    {
                        if (stdin.BaseStream.CanWrite)
                        {
                            stdin.WriteLine("exit");
                        }
                        cmdProcess?.Close();
                        isProcessRunning = false;
                    }));
                }
                return stopCmdProcessCommand;
            }
        }
        public RelayCommand SendCmdProcessCommand
        {
            get
            {
                if (sendCmdProcessCommand == null)
                {
                    sendCmdProcessCommand = new RelayCommand(new Action<object>(o =>
                    {
                        if (stdin == null)
                        {
                            CmdErrorText += "进程未开始\n";
                            return;
                        }

                        if (stdin.BaseStream.CanWrite)
                        {
                            //stdin.Write(rtbStdIn.Text + Environment.NewLine);
                            stdin.WriteLine(CmdInputText);
                            // To write to a Console app, just 
                            // stdin.WriteLine(rtbStdIn.Text); 
                        }
                        CmdInputText = "";
                    }));
                }
                return sendCmdProcessCommand;
            }
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

        public DebugViewModel()
        {
            isProcessRunning = false;
        }
    }
}