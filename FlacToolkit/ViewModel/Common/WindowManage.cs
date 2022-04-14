using FlacToolkit.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlacToolkit.ViewModel.Common
{
    internal class WindowManage
    {
    }
    public interface IWindowManage
    {
        void CreateWindow();
    }

    public class DebugWindowManage : IWindowManage
    {
        public void CreateWindow()
        {
            DebugWindow window = new DebugWindow
            {
                //DataContext = new DebugViewModel(),
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }
    }
}
