using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace bo6_cbuf_tool.ViewModels
{
    class MainWindowViewModel
    {
        public string? SelectedDvar { get; set; }

        public string? SelectedDvarValue { get; set; }

        public string? GameMode { get; set; }

        public List<string> GameModes { get; set; } = new();

        public MainWindowViewModel()
        {
            GameModes.Add("war");
            GameModes.Add("koth");
            GameModes.Add("arena");
            GameModes.Add("dom");
            GameModes.Add("hvt");
        }
    }
}
