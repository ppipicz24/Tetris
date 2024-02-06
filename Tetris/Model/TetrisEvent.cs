using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public class TetrisEvent : EventArgs
    {
        private int _gametime;
        private bool _isMovable = false;
        //idő lekérdezése
        public int GameTime { get { return _gametime; } }
        public bool IsMovable { get { return _isMovable; } }

        public TetrisEvent(int gametime)
        {
            _gametime = gametime;
        }

        public TetrisEvent(bool isMovable)
        {
            _isMovable = isMovable;
        }
    }

    public class TetrisGenerateEvent : EventArgs
    {

    }


}
