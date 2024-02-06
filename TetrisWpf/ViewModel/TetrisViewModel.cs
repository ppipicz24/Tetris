using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Tetris.Model;
using Tetris.Persistence;

namespace TetrisWpf.ViewModel
{
    public class TetrisViewModel : TetrisViewModelBase
    {
        #region Fields
        private TetrisModel? _model;
        private int _gameSize = 12;
        private bool _isPaused;

        #endregion

        #region Properties
        public DelegateCommand? NewGameCommand { get; private set; }
        public DelegateCommand? LoadGameCommand { get; private set; }
        public DelegateCommand? SaveGameCommand { get; private set; }
        public DelegateCommand? ExitGameCommand { get; private set; }
        public DelegateCommand? PauseGameCommand { get; private set; }
        public ObservableCollection<TetrisField>? Fields { get; private set; }
        public String GameTime { get { return TimeSpan.FromSeconds(_model!.GameTime).ToString("g"); } }
        public int GameSize
        {
            get { return _gameSize; }
            set
            {
                if (_gameSize != value)
                {
                    _gameSize = value;
                }
                OnPropertyChanged();
            }

        }


        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsGameSmall
        {
            get { return GameSize == 4; }
            set
            {
                if (_model!.GameSize == Tetris.Model.GameSize.four) return;
                GameSize = 4;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));

            }
        }
        public bool IsGameMedium
        {
            get { return GameSize == 8; }
            set
            {
                if (_model!.GameSize == Tetris.Model.GameSize.eight) return;
                GameSize = 8;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));

            }
        }

        public bool IsGameLarge
        {
            get { return GameSize == 12; }
            set
            {
                if (_model!.GameSize == Tetris.Model.GameSize.twelve) return;
                GameSize = 12;
                OnPropertyChanged(nameof(IsGameSmall));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameLarge));

            }
        }


        #endregion

        #region events
        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler? ExitGame;
        public event EventHandler? PauseGame;

        #endregion

        #region Constructors
        public TetrisViewModel(TetrisModel model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<TetrisEvent>(Model_GameAdvanced!);
            _model.DrawShape += new EventHandler<TetrisEvent>(Model_DrawShape!);

            _isPaused = false;

            NewGameCommand = new DelegateCommand(OnNewGame);
            LoadGameCommand = new DelegateCommand(OnLoadGame);
            SaveGameCommand = new DelegateCommand(OnSaveGame);
            ExitGameCommand = new DelegateCommand(OnExitGame);
            PauseGameCommand = new DelegateCommand(OnPauseGame);

            Fields = new ObservableCollection<TetrisField>();


            GenerateBoard();

        }
        #endregion

        #region event handlers
        private void Model_GameAdvanced(object sender, TetrisEvent e)
        {
            OnPropertyChanged(nameof(GameTime));
        }



        private void Model_DrawShape(object sender, TetrisEvent e)
        {
            RefreshBoard();
            int x = _model!.CurrentX;
            int y = _model.CurrentY;

            for (int i = 0; i < _model!.CurrentShape.Width; i++)
            {
                for (int j = 0; j < _model.CurrentShape.Height; j++)
                {
                    int fieldIndex = (x + i) + (y + j) * _model.GetBoardSize();

                    // Check if the index is within the valid range
                    if (fieldIndex >= 0 && fieldIndex < Fields!.Count)
                    {
                        if (_model.CurrentShape.Points[j,i] == 1)
                        {
                            Fields[fieldIndex].Color = Brushes.Blue;
                        }
                    }
                }
            }
        }

        private void RefreshBoard()
        {
            for (int i = 0; i < _gameSize; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Fields![j * _model!.GetBoardSize() + i].Color = !_model.GetBlock(i,j) ? Brushes.Blue : Brushes.Black;
                }
            }
        }


        #endregion

        #region event methods
        private void OnNewGame(object? obj)
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadGame(object? obj)
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame(object? obj)
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame(object? obj)
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnPauseGame(object? obj)
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region private methods
        private void GenerateBoard()
        {

            Fields!.Clear();
            for (int i = 0; i < _gameSize; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Fields.Add(new TetrisField
                    {
                        Color = !_model!.GetBlock(i, j) ? Brushes.Blue : Brushes.Black
                    });
                }
            }
            OnPropertyChanged(nameof(Fields));
        }


        #endregion
    }
}
