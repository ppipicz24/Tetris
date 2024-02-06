using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model;

namespace Tetris.Persistence
{
    public class Board
    {
        #region fields
        private int _size;
        private int[,] _block;
        private int _currentX, _currentY;
        private Shape? _currentShape;
        private int _time;

        #endregion

        #region properties
        public int Size{ get { return _size; } private set { _size = value; } }
        public int this[int x, int y] { get { return getValue(x, y); } }
        public int CurrentX { get { return _currentX; } private set { _currentX = value; } }
        public int CurrentY { get { return _currentY; } private  set { _currentY = value; } }
        public Shape CurrentShape { get { return _currentShape!; } private set { _currentShape = value; } }
        public int Time { get { return _time; } private set { _time = value; } }
        #endregion

        #region Constructors


        public Board() : this(12) { }

        public Board(int size) { 
            if(size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "The table size less than zero");
            }
            ClearBoard();
            _size = size;
            _block = new int[size, 16];
            _currentX = 0;
            _currentY = 0;
        }

        public Board(int size, int currentx, int currenty, string currentshape, int[,] blocks, int time)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "The table size less than zero");
            }
            ClearBoard();

            _size = size;
            _block = new int[size, 16];
            _currentX = currentx;
            _currentY = currenty;
            _currentShape = new Shape(currentshape);
            _block = blocks;
            _time = time;



        }
        #endregion

        #region private methods
        private void setBlock(int x, int y, int b)
        {
            _block[x, y] = b;
        }

        private void ClearBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    setBlock(i, j, 0);

                }
            }
        }

        #endregion

        #region public methods
        public int getValue(int x, int y)
        {
            return _block[x,y];
        }

        public bool isBlock(int x, int y)
        {
            return _block[x, y] == 0; //akkor true ha nulla van ott
        }

        public void FilledRows()
        {
            for (int i = 0; i < 16; i++)
            {
                int j;
                for (j = Size - 1; j >= 0; j--)
                {
                    if (isBlock(j, i))
                        break;
                }

                if (j == -1)
                {
                    for (j = 0; j < Size; j++)
                    {
                        for (int k = i; k > 0; k--)
                        {

                            if (isBlock(j, k - 1))
                            {
                                setBlock(j, k, 0);
                            }
                            if (!isBlock(j, k - 1))
                            {
                                setBlock(j, k, 1);
                            }
                        }

                        setBlock(j, 0, 0);

                    }
                }
            }
        }

        public void RefreshBoard(Shape currentShape, int currentx, int currenty)
        {
            if(currentShape == null)
            {
                return;
            }
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Points[j, i] == 1)
                    {
                        setBlock(currentx + i, currenty + j, 1);
                    }
                }
            }
        }



        #endregion

    }
}
