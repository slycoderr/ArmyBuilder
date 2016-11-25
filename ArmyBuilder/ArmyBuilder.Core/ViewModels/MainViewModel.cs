using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ArmyBuilder.Core.Models;
using GalaSoft.MvvmLight.Command;
using Slycoder.Portable.MVVM;

namespace ArmyBuilder.Core.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ParserEngineViewModel ParserEngineViewModel { get; }
        public ListViewModel ListViewModel { get; }

        public ObservableCollection<Army> Armies { get; } = new ObservableCollection<Army>();


        public static SynchronizationContext UiContext;

        public MainViewModel()
        {
            ParserEngineViewModel = new ParserEngineViewModel(this);
            ListViewModel = new ListViewModel(this);
        }

        public void LoadArmyData(params Stream[] dataStreams)
        {
            var dsArmy = new XmlSerializer(typeof(Army));

            if (dataStreams == null || dataStreams.Length == 0)
            {
                throw new ArgumentException("Army data cannot be null or empty.");
            }

            foreach (var s in dataStreams)
            {
                using (var reader = XmlReader.Create(s))
                {
                    var army = (Army) dsArmy.Deserialize(reader);

                    Armies.Add(army);
                }

                s.Dispose();
            }
        }

        public void LoadArmyData(params string[] dataStreams)
        {
            var dsArmy = new XmlSerializer(typeof(Army));

            if (dataStreams == null || dataStreams.Length == 0)
            {
                throw new ArgumentException("Army data cannot be null or empty.");
            }

            foreach (var s in dataStreams)
            {
                using (var reader = XmlReader.Create(new StringReader(s)))
                {
                    var army = (Army)dsArmy.Deserialize(reader);

                    Armies.Add(army);
                }
            }
        }


    }
}