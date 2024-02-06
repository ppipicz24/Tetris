using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using Tetris.Model;
using Tetris.Persistence;
using static System.Formats.Asn1.AsnWriter;
using Timer = System.Windows.Forms.Timer;

namespace TetrisForms
{
    public partial class Forms : Form
    {

        #region fields
        private Bitmap? _board;
        private Bitmap? _boardWithShapes;
        private Graphics? _graphics;
        private Graphics? _graphicsWithShapes;
        private TetrisModel _model;
        private int _fieldSize = 60;
        private ITetrisDataAccess _dataAccess = null!;
        private int _gameSize = 12;

        #endregion

        #region constructor
        public Forms()
        {
            InitializeComponent();

            _dataAccess = new TetrisFileDataAccess();

            _model = new TetrisModel(_dataAccess, this);
            _model.GameAdvanced += new EventHandler<TetrisEvent>(Game_GameAdvanced!);
            _model.GameOver += new EventHandler<TetrisEvent>(Game_Gameover!);
            _model.Move += new EventHandler<TetrisEvent>(Game_Move!);
            _model.DrawShape += new EventHandler<TetrisEvent>(Game_DrawShape!);

            _model.NewGame(_gameSize);

            Save.Enabled = false;
            Load.Enabled = false;
            Pause.Enabled = true;

            generateBoard();
            SetupMenus();

        }

        #endregion

        #region game event handler
        private void Game_GameAdvanced(object sender, TetrisEvent e)
        {
            TimeStrip.Text = TimeSpan.FromSeconds(e.GameTime).ToString("g");
        }
        private void Game_Gameover(object sender, TetrisEvent e)
        {
            _model.StopGame();
            Size.Enabled = true;
            Save.Enabled = true;
            Load.Enabled = true;
            DialogResult dialogResult = MessageBox.Show("Játék vége!" + Environment.NewLine +
                                 "Összesen " +
                                 TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál." + "Szeretnél új játékot kezdeni?",
                                 "Tetris",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Asterisk);
            
            
            
   
            if (dialogResult == DialogResult.Yes)
            {
                _model.NewGame(_gameSize);
                _model.StartGame();
                Pause.Text = "Pause";
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }

        }

        private void Game_Move(object sender, TetrisEvent e) //ha mozog az alakzat akkor frissíti a táblát
        {
            if (!e.IsMovable)
            {
                _board = new Bitmap(_boardWithShapes!); 
            }
            FreshDrawBoard(); //minden lépés után ujra rajzolja a boardot
        }

        private void Game_DrawShape(object sender, TetrisEvent e) // ez rajzolja ki a mozgó alakzatokat
        {
            if (_board == null)
            {
                return;
            }
            _boardWithShapes = new Bitmap(_board!);
            _graphicsWithShapes = Graphics.FromImage(_boardWithShapes);

            
            for (int i = 0; i < _model.CurrentShape.Width; i++)
            {
                for (int j = 0; j < _model.CurrentShape.Height; j++)
                {
                    if (_model.CurrentShape.Points[j, i] == 1)
                    {
                        _graphicsWithShapes.FillRectangle(Brushes.DarkCyan, (_model.CurrentX + i) * _fieldSize, (_model.CurrentY + j) * _fieldSize, _fieldSize, _fieldSize);

                    }

                }

            }
            panel.Image = _boardWithShapes;
        }


        #endregion

        #region private methods
        private void generateBoard() //ez rajzolja ki a táblát
        {
            //panel.Controls.Clear();


            //panel.Width = _gameSize * _fieldSize;

            panel.Width = _model.GetBoardSize() * _fieldSize;
            panel.Height = 16 * _fieldSize;

            _board = new Bitmap(panel.Width, panel.Height);
            _graphics = Graphics.FromImage(_board);

            _graphics.FillRectangle(Brushes.Black, 0, 0, _board.Width, _board.Height); 

            panel.Image = _board;
                    
        }

        private void SetupMenus()
        {
            SmallSize.Checked = (_model.GameSize == GameSize.four);
            MediumSize.Checked = (_model.GameSize == GameSize.eight);
            LargeSize.Checked = (_model.GameSize == GameSize.twelve);
        }
        private void FreshDrawBoard() //a már leérkezett, megfagyptt alakzatokat rajzolja ki
        {

            for (int i = 0; i < _model.GetBoardSize(); i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    _graphics = Graphics.FromImage(_board!);

                    _graphics.FillRectangle(_model.GetBlock(i, j) ? Brushes.Black : Brushes.DarkCyan, i * _fieldSize, j * _fieldSize, _fieldSize, _fieldSize);

                }
            }

            panel.Image = _board;
        }
        #endregion

        #region key
        private void Forms_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Left:
                    _model.MoveLeft();
                    break;
                case Keys.Right:
                    _model.MoveRight();
                    break;
                case Keys.Down:
                    _model.MoveDown();
                    break;
                case Keys.Up:
                    _model.RotateShape();
                    break;
                default:
                    return;
            }

        }
        #endregion

        #region menu
        private void NewGame_Click(object sender, EventArgs e)
        {
            Save.Enabled = false;
            Load.Enabled = false;
            _model.NewGame(_gameSize);

            generateBoard();
            SetupMenus();

            _model.StartGame();
            Pause.Enabled = true;
            Pause.Text = "Pause";
            Size.Enabled = false;

        }
        private void SmallSize_Click(object sender, EventArgs e)
        {
            _gameSize = 4;
        }
        private void MediumSize_Click(object sender, EventArgs e)
        {
            _gameSize = 8;
        }
        private void LargeSize_Click(object sender, EventArgs e)
        {
            _gameSize = 12;
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Boolean restartTime = _model.EnableTimer();
            _model.StopGame();
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Tetris",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                if (restartTime)
                {
                    _model.StartGame();
                }
            } 
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            DateTime starttime = DateTime.Now;
            DateTime stoptime = DateTime.Now;
            if (Pause.Text == "Pause")
            {
                Pause.Text = "Start";
                _model.StopGame();
                Save.Enabled = true;
                Load.Enabled = true;
                Size.Enabled = true;
                stoptime = DateTime.Now;
            }
            else
            {
                Pause.Text = "Pause";
                Save.Enabled = false;
                Load.Enabled = false;
                Size.Enabled = false;
                starttime += (DateTime.Now - stoptime);
                
 
                _model.StartGame();
                TimeStrip.Visible = true;
                _model.EnableTimer();

            }

        }
        private async void Save_Click(object sender, EventArgs e)
        {
            bool restartTimer = _model.EnableTimer();
       
            _model.StopGame();
            

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.Save(saveFileDialog.FileName);
                }  
                catch (TetrisDataException) 
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            if (restartTimer)
            {
                _model.StartGame();
            } 

        }
        private async void Load_Click(object sender, EventArgs e)
        {
            bool restartTimer = _model.EnableTimer();
            _model.StopGame();

            if(openFileDialog.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    await _model.Load(openFileDialog.FileName);
                }
                catch(TetrisDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame(_gameSize);
                }
                generateBoard();
                SetupMenus();
                FreshDrawBoard();
                Save.Enabled = true;
                Pause.Enabled = true;
                Pause.Text = "Pause";
                _model.StartGame(); //elindul az ido
            }
        }
        #endregion

    }
}