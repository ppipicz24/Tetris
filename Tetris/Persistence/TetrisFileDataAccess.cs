using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model;

namespace Tetris.Persistence  
{
    public class TetrisFileDataAccess : ITetrisDataAccess
    {
        public async Task SaveAsync(String path, Board table, int currentX, int currentY, Shape currentShape, int time)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(table.Size);
                    await writer.WriteLineAsync(" " + currentX + " " + currentY + " " + currentShape.Type + " " + time);

                   for (int i = 0; i < table.Size; i++)
                   {
                        for (int j = 0; j < 16; j++)
                        {
                            await writer.WriteAsync(table[i, j].ToString() + " ");
                            //await Console.Out.WriteLineAsync(table[i, j].ToString());
                        }
                        await writer.WriteLineAsync();
                    }
                    await Console.Out.WriteLineAsync(writer.ToString());
                }

            }
            catch
            {
                throw new TetrisDataException();
            }
        }

        public async Task<Board> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] cells = line.Split(' ');
                    int tableSize = int.Parse(cells[0]);
                    int currentX = int.Parse(cells[1]);
                    int currentY = int.Parse(cells[2]);
                    string currentShape = cells[3];
                    int time = int.Parse(cells[4]);
                    int[,] blocks = new int[tableSize, 16];

                    for (int i = 0; i < cells.Length; i++)
                    {
                        await Console.Out.WriteLineAsync(cells[i]);
                    }

                    for (int i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        cells = line.Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            blocks[i, j] = int.Parse(cells[j]);
                        }
                    }

                    Board board = new Board(tableSize, currentX, currentY, currentShape, blocks, time);

                    return board;
                    
                }
       
            }
            catch
            {
                throw new TetrisDataException();
            }
        }
    }
}
