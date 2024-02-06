using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Persistence;
using System.Drawing;
using System.ComponentModel;

namespace Tetris.Model
{
    public enum GameSize { four, eight, twelve }
    public class TetrisModel : IDisposable
    {

        #region Fields
        private int _gametime;
        private GameSize _gamesize;
        private Board _board;
        private ITetrisDataAccess _dataAccess;
        private Shapes _shape;
        private Shape _currentShape = null!;
        private int _currentX, _currentY;
        private bool _isMoveSuccess;
        private bool _isGameOver;
        private System.Timers.Timer timer;
        #endregion

        #region Properties
        public int GameTime { get { return _gametime; } }
        private Board Board { get { return _board; } set { _board = value; } }
        public GameSize GameSize { get { return _gamesize; } private set { _gamesize = value; } }
        public bool IsGameOver { get { return _isGameOver; } private set { _isGameOver = value; } }
        public Shape CurrentShape { get { return _currentShape; } private set { _currentShape = value; } }
        public int CurrentX { get { return _currentX; } private set { _currentX = value; }}
        public int CurrentY { get { return _currentY; } private set { _currentY = value; }}
        public bool IsMoveSuccess { get { return _isMoveSuccess; } private set { _isMoveSuccess = value; } }

        public int GetBoardSize()
        {
            return _board.Size;
        }
        public bool GetBlock(int x, int y)
        {
            return _board.isBlock(x, y);
        }
        #endregion

        #region Events
        public event EventHandler<TetrisEvent> GameOver = null!;
        public event EventHandler<TetrisEvent> GameAdvanced = null!;
        public event EventHandler<TetrisEvent> Move = null!;
        public event EventHandler<TetrisEvent> DrawShape = null!;

        #endregion

        #region Constructor
        public TetrisModel(ITetrisDataAccess dataAccess, ISynchronizeInvoke? nev=null)
        {
            _dataAccess = dataAccess;
            _board = new Board();
            _gamesize = GameSize.four;
            _gamesize = GameSize.eight;
            _gamesize = GameSize.twelve;
            _shape = new Shapes();
            _gametime = 0;
            _isGameOver = false;
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) => AdvancedTime();
            timer.SynchronizingObject = nev;
            timer.Start();
        }
        #endregion

        #region event methods
        public void AdvancedTime()
        {
            if (!_isGameOver)
            {
                _gametime++;
                MoveSuccessable();
                OnGameAdvanced();
            }

        }

        private void Gameover()
        {
            _isGameOver = true;
            if (_currentY < 1)
            {
                OnGameOver();
            }
        }


        private void MoveSuccessable()
        {
            _isMoveSuccess = isMove(moveY: 1); // ez az eventek miatt kell, ugyanis ha ez true akkor hivodik meg a move, rotate, drawshape, filledrows event
            if(!_isMoveSuccess)
            {
                refreshBoard();
                SetShape();
            }
            OnMove();
            OnDrawShape();
            _board.FilledRows();

        }

        #endregion 

        #region Public methods
        public void StartGame()
        {
            timer.Start();
        }
        public void StopGame()
        {
            timer.Stop();
        }
        public bool EnableTimer()
        {
            return timer.Enabled;
        }
        public void NewGame(int size)
        {
            _gametime = 0;
            _isGameOver = false;
            _board = new Board(size);
            if (size == 4)
            {
                _gamesize = GameSize.four;
            }
            if (size == 8)
            {
                _gamesize = GameSize.eight;
            }
            if (size == 12)
            {
                _gamesize = GameSize.twelve;
            }
            SetShape();
            OnDrawShape();

        }

        public void MoveLeft()
        {
            isMove(-1);
            OnDrawShape();
        }

        public void MoveRight()
        {
            isMove(1);
            OnDrawShape();
        }

        public void MoveDown()
        {
            isMove(moveY: 1);
            OnDrawShape();
        }

        public void RotateShape()
        {
            _currentShape.Rotate();
            OnDrawShape();
        }


        public async Task Save(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _board, _currentX, _currentY, _currentShape, _gametime);

        }

        public async Task Load(String path)
        {

            if(_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }

            _board = await _dataAccess.LoadAsync(path);   
            _isGameOver = false;
            _gametime = _board.Time;

            switch(GetBoardSize())
            {
                case 4:
                    _gamesize = GameSize.four;
                    break;

                case 8:
                    _gamesize = GameSize.eight;

                    break;
                case 12:
                    _gamesize = GameSize.twelve;
                    break;
            }


            _currentX = _board.CurrentX;
            _currentY = _board.CurrentY;
            _currentShape = _board.CurrentShape;


        }
       
        public void Dispose()
        {
            timer.Dispose();
        }

        #endregion

        #region Private methods

        private bool isMove(int moveX = 0, int moveY = 0) //ez vizsgálja, hogy a leeső alakzat mozgatható-e, ha igen akkor mozgatja is
        {
            var newX = _currentX + moveX;
            var newY = _currentY + moveY;

            if (newX < 0 || newX + _currentShape.Width > _board.Size || newY + _currentShape.Height > 16)
            {
                return false;
            }

            try
            {
                for (int i = 0; i < _currentShape.Width; i++)
                {
                    for (int j = 0; j < _currentShape.Height; j++)
                    {

                        if (newY + j > 0 && !_board.isBlock(newX + i, newY + j) && _currentShape.Points[j, i] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            _currentX = newX;
            _currentY = newY;


            return true;
        }

        private void refreshBoard() //lefrissiti a boardot, ha már nem mozgatható az alakzat, ha már nem mozgathato akkor gameover
        {
            try
            {
                _board.RefreshBoard(_currentShape, _currentX, _currentY);

            }
            catch
            {
                Gameover();
            }
        }
        private void SetShape()
        {
            _currentShape = getRandomShapeCenter();
        }
        private Shape getRandomShapeCenter()
        {
            var shape = _shape.GetRandomShape();
            _currentX = 0;
            _currentY = -shape.Height;
            _currentShape = shape;
            
            return shape;
        }
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new TetrisEvent(_gametime));
        }
        private void OnGameOver()
        {
            GameOver?.Invoke(this, new TetrisEvent(_gametime));

        }

        private void OnMove()
        {
            Move?.Invoke(this, new TetrisEvent(_isMoveSuccess));
        }

        private void OnDrawShape()
        {
            DrawShape?.Invoke(this, new TetrisEvent(_isMoveSuccess));
        }

        



        #endregion

    }
}
