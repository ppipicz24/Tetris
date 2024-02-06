using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model;

namespace Tetris.Persistence
{
    public interface ITetrisDataAccess
    {
        Task<Board> LoadAsync(String path);

        Task SaveAsync(String path, Board table, int currentX, int currentY, Shape currentShape, int time);

    }
}
