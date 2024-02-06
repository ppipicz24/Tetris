using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public class Shapes
    {
        private Shape[] shapesArray;
        public Shapes()
        {
            shapesArray = new Shape[] {

                new Shape(ShapeType.L, 3, 2, new int[,] { { 0, 0, 1 }, { 1, 1, 1 } }),
                new Shape(ShapeType.J, 3, 2, new int[,] { { 1, 1, 1 }, { 0, 0, 1 } }),
                new Shape(ShapeType.O, 2, 2, new int[,] { { 1, 1 }, { 1, 1 } }),
                new Shape(ShapeType.I, 4, 1, new int[,] { { 1, 1, 1, 1} }),
                new Shape(ShapeType.T, 3, 2, new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }),
                new Shape(ShapeType.Z, 3, 2, new int[,] { { 1, 1, 0}, { 0, 1, 1 } }),
                new Shape(ShapeType.S, 3, 2, new int[,] { { 0, 1, 1 }, { 1, 1, 0 } })
            };
        }

        public Shape GetRandomShape()
        {
            Random rand = new Random();
            var shape = shapesArray[rand.Next(shapesArray.Length)];
            return shape;
        }


    }
}
