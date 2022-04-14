#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlacToolkit.Model
{
    internal static class FlacHelper
    {

    }

    internal class Flac : INotifyPropertyChanged
    {
        private string fileName;
        private string info;
        private ObservableCollection<string> vorbisComments;

        public Flac(string fileName)
        {
            vorbisComments = new ObservableCollection<string>();
            FileName = fileName;
            vorbisComments.Add("comment[0]=\"ARTIST = me\"; ");
            vorbisComments.Add("comment[1]=\"TITLE=the sound of Vorbis\"; ");
        }

        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(); } }
        public string Info { get => info; set => info = value; }
        public ObservableCollection<string> VorbisComments { get => vorbisComments; set => vorbisComments = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
