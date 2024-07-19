using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToeWPF
{
    public partial class MainWindow : Window
    {
        private bool isPlayerOneTurn = true;
        private Stack<Tuple<Button, string>> moveHistory = new Stack<Tuple<Button, string>>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && string.IsNullOrEmpty(clickedButton.Content.ToString()))
            {
                moveHistory.Push(new Tuple<Button, string>(clickedButton, clickedButton.Content.ToString()));
                clickedButton.Content = isPlayerOneTurn ? "X" : "O";
                isPlayerOneTurn = !isPlayerOneTurn;
                CheckForWinner();
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (moveHistory.Count > 0)
            {
                var lastMove = moveHistory.Pop();
                lastMove.Item1.Content = lastMove.Item2;
                isPlayerOneTurn = !isPlayerOneTurn;
            }
        }

        private void CheckForWinner()
        {
            // Define the winning positions
            int[][] winningPositions = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 },
                new int[] { 0, 3, 6 },
                new int[] { 1, 4, 7 },
                new int[] { 2, 5, 8 },
                new int[] { 0, 4, 8 },
                new int[] { 2, 4, 6 }
            };

            string[] board = new string[9];
            int index = 0;
            foreach (Button button in GameGrid.Children)
            {
                board[index] = button.Content.ToString();
                index++;
            }

            foreach (int[] position in winningPositions)
            {
                if (!string.IsNullOrEmpty(board[position[0]]) &&
                    board[position[0]] == board[position[1]] &&
                    board[position[1]] == board[position[2]])
                {
                    MessageBox.Show($"Player {board[position[0]]} wins!", "Congratulations");
                    ResetGame();
                    return;
                }
            }

            // Check for draw
            bool isDraw = true;
            foreach (string cell in board)
            {
                if (string.IsNullOrEmpty(cell))
                {
                    isDraw = false;
                    break;
                }
            }

            if (isDraw)
            {
                MessageBox.Show("The game is a draw!", "Draw");
                ResetGame();
            }
        }

        private void ResetGame()
        {
            foreach (Button button in GameGrid.Children)
            {
                button.Content = "";
            }
            moveHistory.Clear();
            isPlayerOneTurn = true;
        }
    }
}
