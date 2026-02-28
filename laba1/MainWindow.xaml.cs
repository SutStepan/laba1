using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laba1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] buttons;
        private int currentPlayer; // 1 - X, 2 - O
        private bool gameOver;
        private int scoreX;
        private int scoreO;
        private char playerXSymbol;
        private char playerOSymbol;
        private Button[] winningButtons; // для подсветки

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[3, 3] {
                { btn00, btn01, btn02 },
                { btn10, btn11, btn12 },
                { btn20, btn21, btn22 }
            };

            playerXSymbol = 'X';
            playerOSymbol = 'O';

            scoreX = 0;
            scoreO = 0;

            UpdateScoreDisplay();
            NewGame();
        }

        private void NewGame()
        {
            foreach (Button btn in buttons)
            {
                btn.Content = "";
                btn.IsEnabled = true;
                btn.Background = Brushes.White; // сброс подсветки
                btn.Foreground = Brushes.Black;
            }

            currentPlayer = 1;
            gameOver = false;
            winningButtons = null;
        }

        private void UpdateScoreDisplay()
        {
            ScoreXText.Text = scoreX.ToString();
            ScoreOText.Text = scoreO.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameOver) return;

            Button clickedButton = sender as Button;

            if (clickedButton.Content != null && clickedButton.Content.ToString() != "")
                return;

            // Ставим символ с цветом
            char symbol = (currentPlayer == 1) ? playerXSymbol : playerOSymbol;
            clickedButton.Content = symbol.ToString();
            clickedButton.Foreground = (currentPlayer == 1) ? Brushes.Red : Brushes.Blue;

            if (CheckWin(out winningButtons))
            {
                // Подсвечиваем победную линию
                HighlightWinningLine(winningButtons);

                if (currentPlayer == 1)
                    scoreX++;
                else
                    scoreO++;

                UpdateScoreDisplay();

                string winner = (currentPlayer == 1) ? $"Игрок X ({playerXSymbol})" : $"Игрок O ({playerOSymbol})";
                MessageBox.Show($"Победил {winner}!", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
                gameOver = true;
                DisableAllButtons();
                return;
            }

            if (CheckDraw())
            {
                MessageBox.Show("Ничья!", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
                gameOver = true;
                return;
            }

            // Меняем игрока
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }

        private bool CheckWin(out Button[] winLine)
        {
            winLine = null;

            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                if (IsLineEqual(buttons[i, 0], buttons[i, 1], buttons[i, 2]))
                {
                    winLine = new Button[] { buttons[i, 0], buttons[i, 1], buttons[i, 2] };
                    return true;
                }
            }

            // Проверка колонок
            for (int j = 0; j < 3; j++)
            {
                if (IsLineEqual(buttons[0, j], buttons[1, j], buttons[2, j]))
                {
                    winLine = new Button[] { buttons[0, j], buttons[1, j], buttons[2, j] };
                    return true;
                }
            }

            // Диагонали
            if (IsLineEqual(buttons[0, 0], buttons[1, 1], buttons[2, 2]))
            {
                winLine = new Button[] { buttons[0, 0], buttons[1, 1], buttons[2, 2] };
                return true;
            }

            if (IsLineEqual(buttons[0, 2], buttons[1, 1], buttons[2, 0]))
            {
                winLine = new Button[] { buttons[0, 2], buttons[1, 1], buttons[2, 0] };
                return true;
            }

            return false;
        }

        private bool IsLineEqual(Button b1, Button b2, Button b3)
        {
            string c1 = b1.Content?.ToString();
            string c2 = b2.Content?.ToString();
            string c3 = b3.Content?.ToString();

            return !string.IsNullOrEmpty(c1) && c1 == c2 && c2 == c3;
        }

        private void HighlightWinningLine(Button[] line)
        {
            if (line == null) return;

            foreach (Button btn in line)
            {
                btn.Background = Brushes.LightGreen;
            }
        }

        private bool CheckDraw()
        {
            foreach (Button btn in buttons)
            {
                if (string.IsNullOrEmpty(btn.Content?.ToString()))
                    return false;
            }
            return true;
        }

        private void DisableAllButtons()
        {
            foreach (Button btn in buttons)
                btn.IsEnabled = false;
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void ResetScore_Click(object sender, RoutedEventArgs e)
        {
            scoreX = 0;
            scoreO = 0;
            UpdateScoreDisplay();
            NewGame();
        }

        private void ApplyCustomization_Click(object sender, RoutedEventArgs e)
        {
            string newX = CustomXSymbol.Text.Trim();
            string newO = CustomOSymbol.Text.Trim();

            if (!string.IsNullOrEmpty(newX))
                playerXSymbol = newX[0];

            if (!string.IsNullOrEmpty(newO))
                playerOSymbol = newO[0];

            NewGame();
            MessageBox.Show($"Новые символы: X = {playerXSymbol}, O = {playerOSymbol}", "Настройки применены");
        }
    }
}