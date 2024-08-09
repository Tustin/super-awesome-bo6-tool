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
        public string? CommandBuffer { get; set; } = "xstartlobby;";
        public string? ConsoleIpAddress { get; set; } = "192.168.0.112";
        public string? SelectedDvar { get; set; }

        public string? SelectedDvarValue { get; set; }

        public string? GameMode { get; set; }

        public List<string> GameModes { get; set; } = new();

        public List<string> Maps { get; set; } = new();

        public string? SelectedMap { get; set; }

        public MainWindowViewModel()
        {
            GameModes.Add("war");
            GameModes.Add("koth");
            GameModes.Add("arena");
            GameModes.Add("dom");
            GameModes.Add("hvt");

            Maps.Add("mp_t10_island");
            Maps.Add("mp_t10_penthouse");
            Maps.Add("mp_t10_radar");
            Maps.Add("mp_t10_traingraveyard");
            Maps.Add("mp_t10_stripmall");
            Maps.Add("mp_t10_sm_babylon");
            Maps.Add("mp_t10_sm_capital");
            Maps.Add("mp_t10_sm_flat");
            Maps.Add("mp_t10_sm_vorkuta_mine");

        }
    }
}
