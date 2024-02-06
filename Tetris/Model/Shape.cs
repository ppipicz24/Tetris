using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public enum ShapeType { L, J, I, Z, S, O, T }
    public class Shape
    {
        #region field
        private int[,]? _rotatedPoint;
        #endregion

        #region properties
        public ShapeType Type { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[,] Points { get; private set; }

        #endregion

        #region constructor
        public Shape(ShapeType type, int width, int height, int[,] points)
        {
            Type = type;
            Width = width;
            Height = height;
            Points = points;
        }

        public Shape(string shapeType)
        {
            this.Points = null!;
            if(shapeType == "O")
            {
                this.Type = ShapeType.O;
                this.Width = 2;
                this.Height = 2;
                this.Points = new int[2, 2] { { 1, 1 }, { 1, 1 } };
            }
            if(shapeType == "I")
            {
                this.Type = ShapeType.I;
                this.Width = 4;
                this.Height = 1;
                this.Points = new int[1, 4] { { 1, 1, 1, 1 } };
            }
            if(shapeType == "S")
            {
                this.Type = ShapeType.S;
                this.Width = 3;
                this.Height = 2;
                this.Points = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
            }
            if(shapeType == "Z")
            {
                this.Type = ShapeType.Z;
                this.Width = 3;
                this.Height = 2;
                this.Points = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
            }
            if(shapeType == "L")
            {
                this.Type = ShapeType.L;
                this.Width = 3;
                this.Height = 2;
                this.Points = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
            }
            if(shapeType == "J")
            {
                this.Type = ShapeType.J;
                this.Width = 3;
                this.Height = 2;
                this.Points = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
            }
            if(shapeType == "T")
            {
                this.Type = ShapeType.T;
                this.Width = 3;
                this.Height = 2;
                this.Points = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
            }

        }

        #endregion

        #region public methods
        public void Rotate()
        {

            _rotatedPoint = new int[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = Height - 1; j >= 0; j--)
                {
                    _rotatedPoint[i, Height - 1 - j] = Points[j, i];
                }

            }

            Points = _rotatedPoint;
            int temp = Width;
            Width = Height;
            Height = temp;


        }

        #endregion




    }
}
