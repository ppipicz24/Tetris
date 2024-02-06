using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tetris.Model;
using Tetris.Persistence;
using TetrisWpf.ViewModel;

namespace TetrisWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region fields
        private TetrisViewModel? _viewModel;
        private TetrisModel? _model;
        private MainWindow? _view;
        private int _gameSize = 12;
        #endregion

        #region constructor
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        #endregion

        #region app event handlers

        void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new TetrisModel(new TetrisFileDataAccess());
            _model.GameOver += new EventHandler<TetrisEvent>(Model_GameOver!);

            _model.NewGame(_gameSize);

            _viewModel = new TetrisViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame!);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame!);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame!);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame!);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame!);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new CancelEventHandler(MainWindow_Closing!);
            _view.KeyDown += new KeyEventHandler(View_KeysDown);
            _view.Show();

        }
        #endregion

        #region view event handlers
        private void View_KeysDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _model!.MoveLeft();
                    break;
                case Key.Right:
                    _model!.MoveRight();
                    break;
                case Key.Down:
                    _model!.MoveDown();
                    break;
                case Key.Up:
                    _model!.RotateShape();
                    break;
                default:
                    return;

            }


        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            bool timer = _model!.EnableTimer();
            if (timer)
            {
                _model!.StopGame();
            }
            if (MessageBox.Show("You are about to exit the game." + Environment.NewLine +
                "Make sure to save if you wish to continue later.", "Exit", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;

                if (timer)
                {
                    _model!.StartGame();
                }
            }


        }

        #endregion

        #region viewmodel event handlers


        private void ViewModel_PauseGame(object sender, EventArgs e)
        {

            if (_model!.EnableTimer())
            {
  
                _model!.StopGame();
                _viewModel!.IsPaused = true;
            }
            else
            {
                _model!.StartGame();
                _viewModel!.IsPaused = false;
            }

        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save Game";
                saveFileDialog.Filter = "Tetris board (*.stl)|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model!.Save(saveFileDialog.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Saving game failed." + Environment.NewLine +
                            "Incorrect filepath or cannot write in directory.", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Saving game failed." + Environment.NewLine +
                            "Incorrect filepath or cannot write in directory.", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
            }
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            bool restartTimer = _model!.EnableTimer();
            _model!.StopGame();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógablak
                openFileDialog.Title = "Tetris tábla betöltése";
                openFileDialog.Filter = "Tetris tábla|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model!.Load(openFileDialog.FileName);
                    _viewModel!.GameSize = _model.GetBoardSize();
                    MainWindow.Width = _model.GetBoardSize() * 30 + 200;
                    _model!.StartGame();

                }

            }
            catch (TetrisDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Tetris", MessageBoxButton.OK, MessageBoxImage.Error);

                _model!.NewGame(_gameSize);
            }
        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {
            _view!.Close();
        }

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            switch(_viewModel!.GameSize)
            {
                case 4:
                    _gameSize = 4;
                    MainWindow.Width = 160;
                    break;
                case 8:
                    _gameSize = 8;
                    MainWindow.Width = 320;
                    break;
                case 12:
                    _gameSize = 12;
                    MainWindow.Width = 480;
                    break;
            }
            _model!.NewGame(_gameSize);
            _model!.StartGame();
        }

        #endregion

        #region model event handlers
        private void Model_GameOver(object sender, TetrisEvent e)
        {
            _model!.StopGame();
               var result =  MessageBox.Show("Nice play!" + Environment.NewLine + "You lasted for: " +
                TimeSpan.FromSeconds(e.GameTime).ToString("g"), "Szeretnél új játékot kezdeni?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (result == MessageBoxResult.Yes)
            {
                _model!.NewGame(_gameSize);
                _model!.StartGame();
            }
            else
            {
                _view!.Close();
            }

        }
        #endregion

    }
}
