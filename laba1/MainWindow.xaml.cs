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
        private char currentPlayer;
        private bool gameOver;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Создаем массив кнопок для удобства
            buttons = new Button[3, 3] {
                { btn00, btn01, btn02 },
                { btn10, btn11, btn12 },
                { btn20, btn21, btn22 }
            };

            NewGame();
        }

        private void NewGame()
        {
            // Очищаем все кнопки
            foreach (Button btn in buttons)
            {
                btn.Content = "";
                btn.IsEnabled = true;
            }

            currentPlayer = 'X';
            gameOver = false;
            StatusText.Text = "Ход игрока: X";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameOver) return;

            Button clickedButton = sender as Button;

            // Если клетка уже занята - ничего не делаем
            if (clickedButton.Content != null && clickedButton.Content.ToString() != "")
                return;

            // Ставим символ
            clickedButton.Content = currentPlayer.ToString();

            // Проверяем победу
            if (CheckWin())
            {
                StatusText.Text = $"Игрок {currentPlayer} победил!";
                gameOver = true;
                DisableAllButtons();
                return;
            }

            // Проверяем ничью
            if (CheckDraw())
            {
                StatusText.Text = "Ничья!";
                gameOver = true;
                return;
            }

            // Меняем игрока
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
            StatusText.Text = $"Ход игрока: {currentPlayer}";
        }

        private bool CheckWin()
        {
            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Content?.ToString() != "" &&
                    buttons[i, 0].Content?.ToString() == buttons[i, 1].Content?.ToString() &&
                    buttons[i, 1].Content?.ToString() == buttons[i, 2].Content?.ToString())
                {
                    return true;
                }
            }

            // Проверка колонок
            for (int j = 0; j < 3; j++)
            {
                if (buttons[0, j].Content?.ToString() != "" &&
                    buttons[0, j].Content?.ToString() == buttons[1, j].Content?.ToString() &&
                    buttons[1, j].Content?.ToString() == buttons[2, j].Content?.ToString())
                {
                    return true;
                }
            }

            // Проверка диагонали (левая верхняя → правая нижняя)
            if (buttons[0, 0].Content?.ToString() != "" &&
                buttons[0, 0].Content?.ToString() == buttons[1, 1].Content?.ToString() &&
                buttons[1, 1].Content?.ToString() == buttons[2, 2].Content?.ToString())
            {
                return true;
            }

            // Проверка диагонали (правая верхняя → левая нижняя)
            if (buttons[0, 2].Content?.ToString() != "" &&
                buttons[0, 2].Content?.ToString() == buttons[1, 1].Content?.ToString() &&
                buttons[1, 1].Content?.ToString() == buttons[2, 0].Content?.ToString())
            {
                return true;
            }

            return false;
        }

        private bool CheckDraw()
        {
            foreach (Button btn in buttons)
            {
                if (btn.Content == null || btn.Content.ToString() == "")
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
    }
}