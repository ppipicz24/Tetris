using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TetrisWpf.ViewModel
{
    public class TetrisField : TetrisViewModelBase
    {
        private SolidColorBrush? _color;

        public SolidColorBrush Color
        {
            get { return _color!; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
