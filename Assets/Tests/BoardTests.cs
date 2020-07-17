using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Board
    {
        // A Test behaves as an ordinary method

        [Test]
        public void GetGamePasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            Assert.IsNotNull(game);
        }

        [Test]
        public void GetBoardControllerPasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            GameController gameController =
                game.GetComponentInChildren<GameController>();
            gameController.gameObject.SetActive(false);

            BoardController boardController = game.GetComponentInChildren<BoardController>();
            Assert.IsInstanceOf(typeof(BoardController), boardController);

            gameController.gameObject.SetActive(true);
        }

        [Test]
        public void HorizontalWinCheckPasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            GameController gameController =
                game.GetComponentInChildren<GameController>();
            gameController.gameObject.SetActive(false);
            BoardController boardController = game.GetComponentInChildren<BoardController>();
            for (int columnToStartWith = 0; columnToStartWith < 4; columnToStartWith++)
            {
                for (int rowToCheck = 0; rowToCheck < 6; rowToCheck++)
                {
                    for (int j = columnToStartWith; j < columnToStartWith+4; j++)
                    {
                        boardController.OnGrabberTriggered(rowToCheck, j, "Player1");
                    }

                    for (int j = columnToStartWith; j < columnToStartWith+4; j++)
                    {
                        Assert.IsTrue(boardController.CheckForWin(rowToCheck, j, "Player1"));
                    }
                    boardController.ClearBoard();
                }
            }
            gameController.gameObject.SetActive(true);
        }
/*
        [Test]
        public void UpRightWinCheckPasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            GameController gameController =
                game.GetComponentInChildren<GameController>();
            gameController.gameObject.SetActive(false);
            BoardController boardController = game.GetComponentInChildren<BoardController>();

            // Check one way
            for (int columnToStartWith = 0; columnToStartWith < 4; columnToStartWith++)
            {
                for (int rowToStartWith = 0; rowToStartWith < 3; rowToStartWith++)
                {
                    int[] row = new int[4];
                    int[] col = new int[4];
                    for (int i = rowToStartWith, j = columnToStartWith, k = 0; k < 4; i++, j++, k++)
                    {
                        boardController.OnGrabberTriggered(i, j, "Player1");
                        row[k] = i;
                        col[k] = j;
                    }

                    for (int k = 0; k < 4; k++)
                    {
                        Assert.IsTrue(boardController.CheckForWin(row[k], col[k], "Player1"));
                    }
                    boardController.ClearBoard();
                }
            }
            gameController.gameObject.SetActive(true);
        }
*/
/*
        [Test]
        public void UpLeftWinCheckPasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            GameController gameController =
                game.GetComponentInChildren<GameController>();
            gameController.gameObject.SetActive(false);
            BoardController boardController = game.GetComponentInChildren<BoardController>();

            // Check one way
            for (int columnToStartWith = 0; columnToStartWith < 4; columnToStartWith++)
            {
                for (int rowToStartWith = 0; rowToStartWith < 3; rowToStartWith++)
                {
                    int[] row = new int[4];
                    int[] col = new int[4];
                    for (int i = rowToStartWith, j = columnToStartWith, k = 0; k < 4; i++, j++, k++)
                    {
                        boardController.OnGrabberTriggered(i, j, "Player1");
                        row[k] = i;
                        col[k] = j;
                    }

                    for (int k = 0; k < 4; k++)
                    {
                        Assert.IsTrue(boardController.CheckForWin(row[k], col[k], "Player1"));
                    }
                    boardController.ClearBoard();
                }
            }
            gameController.gameObject.SetActive(true);
        }
*/
        [Test]
        public void VerticalWinCheckPasses()
        {
            GameObject game = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            GameController gameController =
                game.GetComponentInChildren<GameController>();
            gameController.gameObject.SetActive(false);
            BoardController boardController = game.GetComponentInChildren<BoardController>();
            for (int columnToStartWith = 0; columnToStartWith < 7; columnToStartWith++)
            {
                for (int rowToStartWith = 0; rowToStartWith < 3; rowToStartWith++)
                {
                    for (int i = rowToStartWith; i < rowToStartWith+4; i++)
                    {
                        boardController.OnGrabberTriggered(i, columnToStartWith, "Player1");
                    }

                    for (int i = rowToStartWith; i < rowToStartWith+4; i++)
                    {
                        Assert.IsTrue(boardController.CheckForWin(i, columnToStartWith, "Player1"));
                    }
                    boardController.ClearBoard();
                }
            }
            gameController.gameObject.SetActive(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BoardWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
