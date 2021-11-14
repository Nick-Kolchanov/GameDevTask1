using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevTask1
{
    class GameBackCode
    {
        #region Variables
        private int[,] gameBoard;
        private int gameBoardSize;
        private const int DEFAULT_BOARD_SIZE = 3;
        public bool isPlayerTurn = true;

        #endregion

        #region Initializations
        public GameBackCode() : this(DEFAULT_BOARD_SIZE)
        {

        }

        public GameBackCode(int gameFieldSize_)
        {
            Init(gameFieldSize_);
        }

        private void Init(int gameFieldSize_)
        {
            gameBoardSize = gameFieldSize_;
            gameBoard = GetNewPlayField();
        }

        private int[,] GetNewPlayField()
        {
            var newPlayField = new int[gameBoardSize, gameBoardSize];
            for (int i = 0; i < gameBoardSize; i++)
            {
                for (int j = 0; j < gameBoardSize; j++)
                {
                    newPlayField[i, j] = 0;
                }
            }
            return newPlayField;
        }

        public void SetPlayerTurn(bool playerturns)
        {
            isPlayerTurn = playerturns;
        }

        #endregion

        public bool MakeTurn(int x, int y)
        {
            if (gameBoard[x, y] != 0)
                return false;

            if (isPlayerTurn)
            {
                gameBoard[x, y] = 1;
            }
            else
            {
                gameBoard[x, y] = -1;
            }

            isPlayerTurn = !isPlayerTurn;
            return true;
        }

        public bool IsEndGame()
        {
            return !IsFreeCellsLeft() || IsWinCondition();
        }

        public int IsPlayerWon()
        {
            if (IsWinCondition())
            {
                if (isPlayerTurn) return 1;

                return 2;
            }
            else if (!IsFreeCellsLeft()) return 0;
            return -1;
        }

        private bool IsFreeCellsLeft()
        {
            for (int i = 0; i < gameBoardSize; i++)
            {
                for (int j = 0; j < gameBoardSize; j++)
                {
                    if (gameBoard[i, j] == 0) return true;
                }
            }

            return false;
        }

        #region Checking for win
        private bool IsWinCondition()
        {
            if (!isCleanBoard())
            {
                return CheckRowsForWin() ||
                       CheckColumnsForWin() ||
                       CheckDiagForWin();
            }
            else
            {
                return false;
            }
        }

        private bool CheckRowsForWin()
        {
            bool isWin = true;

            for (int i = 0; i < gameBoardSize; i++)
            {
                isWin = true;

                for (int j = 1; j < gameBoardSize; j++)
                {
                    if (gameBoard[i, j] != gameBoard[i, j - 1] ||
                        gameBoard[i, j] == 0)
                        isWin = false;
                }

                if (isWin) return true;
            }

            return false;
        }

        private bool CheckColumnsForWin()
        {
            bool isWin = true;

            for (int i = 0; i < gameBoardSize; i++)
            {
                isWin = true;

                for (int j = 1; j < gameBoardSize; j++)
                {
                    if (gameBoard[j, i] != gameBoard[j - 1, i] ||
                        gameBoard[j, i] == 0)
                        isWin = false;
                }

                if (isWin) return true;
            }

            return isWin;
        }

        private bool CheckDiagForWin()
        {
            bool isWin = true;

            for (int i = 1; i < gameBoardSize; i++)
            {
                if (gameBoard[i, i] != gameBoard[i - 1, i - 1] ||
                    gameBoard[i, i] == 0)
                    isWin = false;
            }

            if (isWin) return true;

            isWin = true;

            for (int i = 1; i < gameBoardSize; i++)
            {
                if (gameBoard[gameBoardSize - i - 1, i] != gameBoard[gameBoardSize - i, i - 1] ||
                    gameBoard[gameBoardSize - i - 1, i] == 0)
                    isWin = false;
            }

            return isWin;
        }

        private bool isCleanBoard()
        {
            for (int i = 0; i < gameBoardSize; i++)
            {
                for (int j = 0; j < gameBoardSize; j++)
                {
                    if (gameBoard[i, j] != 0) return false;
                }
            }

            return true;
        }

        #endregion


        private void ChangeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
        }

        // Код далее - базовый бот-противник. На данный момент он не встроен в игру. Патчи на подходе.

        private void MakeAITurn()
        {
            (int x, int y) AIChoicedCell = ChooseCellAI();

            gameBoard[AIChoicedCell.x, AIChoicedCell.y] = -1;
        }
       
        #region Choosing AI cell
        private (int, int) ChooseCellAI() //TODO: Улучшенный (не рандомный) AI
        {
            var cell = FindUsedCell();

            if (cell != (-1, -1))
                cell = FindFreeCellAround(cell);

            if (cell == (-1, -1))
                cell = FindFreeCell();

            return cell;
        }

        private (int, int) FindUsedCell()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var cell = (-1, -1);

            for (int i = 0; i < gameBoardSize; i++)
            {
                for (int j = 0; j < gameBoardSize; j++)
                {
                    if (gameBoard[i, j] == -1)
                    {
                        cell = (i, j);
                        if (r.Next(0, 2) == 0)
                        {
                            return cell;
                        }
                    }
                }
            }

            return cell;
        }

        public (int, int) FindFreeCellAround((int, int) cell)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (isAvailablePoint((cell.Item1 + i, cell.Item2 + j)))
                        return (cell.Item1 + i, cell.Item2 + j);
                }
            }

            return (-1, -1);
        }

        private (int, int) FindFreeCell()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var cell = (-1, -1);

            for (int i = 0; i < gameBoardSize; i++)
            {
                for (int j = 0; j < gameBoardSize; j++)
                {
                    if (gameBoard[i, j] == 0)
                    {
                        cell = (i, j);

                        if (r.Next(0, 3) == 0)
                        {
                            return cell;
                        }
                    }
                }
            }

            return cell;
        }

        #endregion

        private bool isAvailablePoint((int, int) point)
        {
            if (point.Item1 < gameBoardSize && point.Item1 >= 0 &&
                point.Item2 < gameBoardSize && point.Item2 >= 0)
            {
                if (gameBoard[point.Item1, point.Item2] == 0)
                    return true;
            }

            return false;
        }
    }
}
