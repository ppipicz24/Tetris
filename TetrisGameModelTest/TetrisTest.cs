using Tetris.Model;
using Tetris.Persistence;
using Moq;

namespace TetrisGameModelTest
{
    [TestClass]
    public class TetrisTest : IDisposable
    {
        private TetrisModel _model = null!;
        private Board _mockedboard = null!;
        private Mock<ITetrisDataAccess> _mock = null!;


        [TestInitialize]
        public void Initialize()
        {
            int size = 4;
            int currentx = 0;
            int currenty = 0;
            string shape = "I";
            int[,] blocks = new int[4, 16];

            for (int i = 0; i < size-1; i++)
            {
                for (int j = 3; j < 16; j++)
                {
                    blocks[i, j] = 1;
                }

                blocks[3, 15] = 0;

            }

            _mockedboard = new Board(size, currentx, currenty, shape, blocks,0);


            _mock = new Mock<ITetrisDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedboard));

            _model = new TetrisModel(_mock.Object);

            _model.GameAdvanced += new EventHandler<TetrisEvent>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<TetrisEvent>(Model_GameOver);


        }




        [TestMethod]
        public void TetrisAdvanceTimeTest()
        {
            
            _model.NewGame(4);
            Assert.AreEqual(0, _model.GameTime);       
            _model.AdvancedTime();
            Assert.AreEqual(1, _model.GameTime);


        }

        [TestMethod]
        public void TetrisGameModelNewGameSmallTest()
        {
            _model.NewGame(4);

            Assert.AreEqual(GameSize.four, _model.GameSize);
            Assert.AreEqual(4, _model.GetBoardSize());

            int empty = 0;
            for (int i = 0; i < _model.GetBoardSize(); i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (_model.GetBlock(i,j))
                    {
                        empty++;
                    }

                }
            }

            Assert.AreEqual(4 * 16, _model.GetBoardSize() * 16); //cell�k sz�ma
            Assert.AreEqual(4 * 16, empty); //minden cella �res


        }

        [TestMethod]
        public void TetrisGameModelNewGameMediumTest()
        {

            _model.NewGame(8);

            Assert.AreEqual(GameSize.eight, _model.GameSize);
            Assert.AreEqual(8, _model.GetBoardSize());

            int empty = 0;
            for (int i = 0; i < _model.GetBoardSize(); i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (_model.GetBlock(i,j))
                    {
                        empty++;
                    }

                }
            }

            Assert.AreEqual(8 * 16, _model.GetBoardSize() * 16); //cellák száma
            Assert.AreEqual(8 * 16, empty); //minden cella üres

        }

        [TestMethod]
        public void TetrisGameModelNewGameLargeTest()
        {
            _model.NewGame(12);

            Assert.AreEqual(GameSize.twelve, _model.GameSize);
            Assert.AreEqual(12, _model.GetBoardSize());

            int empty = 0;
            for (int i = 0; i < _model.GetBoardSize(); i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (_model.GetBlock(i, j))
                    {
                        empty++;
                    }

                }
            }

            Assert.AreEqual(12 * 16, _model.GetBoardSize() * 16); //cell�k sz�ma
            Assert.AreEqual(12 * 16, empty); //minden cella �res
        }


        [TestMethod]
        public void TetrisGameModelMoveTest()
        {
            Task task = _model.Load(String.Empty);

            //_model.AdvancedTime();
            Assert.AreEqual(0, _model.CurrentX);
            Assert.AreEqual(0, _model.CurrentY);
            Assert.AreEqual(4, _model.CurrentShape.Width);
            Assert.AreEqual(1, _model.CurrentShape.Height);

            //lefele mozgás
            _model.MoveDown();
            Assert.AreEqual(1, _model.CurrentY);

            //forgatas
            _model.CurrentShape.Rotate();
            Assert.AreEqual(1, _model.CurrentShape.Width);
            Assert.AreEqual(4, _model.CurrentShape.Height);

            //jobbra mozgás
            _model.MoveRight(); //nem tud jobbra mozogni, mivel vege a palyanak
            Assert.AreEqual(0, _model.CurrentX);

            //balra mozgás
            _model.MoveLeft();//nem tud jobbra mozogni, mivel vege a palyanak
            Assert.AreEqual(0, _model.CurrentX);
        }

        [TestMethod]
        public void TetrisGameModelLoadTest()
        {

            Task task = _model.Load(String.Empty);


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(_mockedboard.isBlock(i, j), _model.GetBlock(i,j));
                }
            }

            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }


        [TestMethod]
        public void TetrisGameModelGameOverTest()
        {
            //a test which checks if the game is over
            //while not game over than the game is running 
            //the game is over when the current shape is not moveable and the current shape is on the top of the board and the currentshape is less than 1

            Task task = _model.Load(String.Empty);

            Assert.IsFalse(_model.IsGameOver);

            while(!_model.IsGameOver || _model.IsMoveSuccess)
            {
                _model.AdvancedTime();
            }

        }

        private void Model_GameAdvanced(Object? sender, TetrisEvent e)
        {

            Assert.IsTrue(_model.GameTime >= 0);

            Assert.AreEqual(e.GameTime, _model.GameTime);
            //Assert.IsTrue(false);
        }

        private void Model_GameOver(Object? sender, TetrisEvent e)
        {
            Assert.IsTrue(_model.IsGameOver);
            Assert.AreEqual(_model.GameTime, e.GameTime);
            //Assert.IsTrue(false);

        }
        public void Dispose()
        {
            _model.Dispose();
        }




    }
}