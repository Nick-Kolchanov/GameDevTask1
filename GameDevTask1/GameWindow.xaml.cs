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
using System.Windows.Shapes;

namespace GameDevTask1
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        bool circleTurn;
        bool endGame;
        GameBackCode backCode;

        public GameWindow()
        {
            InitializeComponent();
            backCode = new GameBackCode(3);
            circleTurn = true;
            endGame = false;
            resultLabel.Visibility = Visibility.Hidden;
            exitButton.IsEnabled = false;

            tile_00.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_01.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_02.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_10.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_11.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_12.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_20.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_21.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
            tile_22.Source = new BitmapImage(new Uri("/assets/empty.png", UriKind.Relative));
        }

        private void tile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (endGame)
                return;
            
            Image imageTile = sender as Image;
            bool ok = backCode.MakeTurn((int)char.GetNumericValue(imageTile.Name[^2]), (int)char.GetNumericValue(imageTile.Name[^1]));
            if (!ok)
                return;

            if (circleTurn)
            {
                imageTile.Source = new BitmapImage(new Uri("/assets/circle.png", UriKind.Relative));
            }
            else
            {
                imageTile.Source = new BitmapImage(new Uri("/assets/cross.png", UriKind.Relative));
            }
            circleTurn = !circleTurn;

            if (backCode.IsEndGame())
            {
                endGame = true;
                int res = backCode.IsPlayerWon();
                if (res == 1)
                    resultLabel.Content = "Игрок 1 победил!";
                else if (res == 2)
                    resultLabel.Content = "Игрок 2 победил!";
                else if(res == 0)
                    resultLabel.Content = "Ничья!";
                resultLabel.Visibility = Visibility.Visible;

                exitButton.IsEnabled = true;
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Top = Top;
            mainWindow.Left = Left;
            mainWindow.Show();
            Close();
        }
    }
}
