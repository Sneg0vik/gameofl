using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace GameOfL
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int NumCellsWidth = 30;
        const int NumCellsHeight = 30;
        bool PauseGame = false;
        Rectangle[,] Neighbour = new Rectangle[NumCellsWidth, NumCellsHeight];
        DispatcherTimer timer = new DispatcherTimer();
        dbSql db = new dbSql();
        SaveGames SavegamesF = new SaveGames();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DeathOrLife();
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < NumCellsHeight; i++)
            {
                for (int j = 0; j < NumCellsWidth; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = fieldGame.ActualWidth / NumCellsHeight - 2.0;
                    rectangle.Height = fieldGame.ActualHeight / NumCellsWidth - 2.0;
                    rectangle.Fill = Brushes.Red;
                    fieldGame.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, j * fieldGame.ActualWidth / NumCellsWidth);
                    Canvas.SetTop(rectangle, i * fieldGame.ActualHeight / NumCellsHeight);
                    rectangle.MouseDown += M_Click;

                    Neighbour[i, j] = rectangle;


                }

            }
        }
        private void RandomLife_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < NumCellsHeight; i++)
            {
                for (int j = 0; j < NumCellsWidth; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = fieldGame.ActualWidth / NumCellsHeight - 2.0;
                    rectangle.Height = fieldGame.ActualHeight / NumCellsWidth - 2.0;
                    rectangle.Fill = (random.Next(0, 2) == 1) ? Brushes.Black : Brushes.Red;
                    fieldGame.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, j * fieldGame.ActualWidth / NumCellsWidth);
                    Canvas.SetTop(rectangle, i * fieldGame.ActualHeight / NumCellsHeight);
                    rectangle.MouseDown += M_Click;

                    Neighbour[i, j] = rectangle;
                }

            }
        }

        private void M_Click(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.Red) ? Brushes.Black : Brushes.Red;
        }

        private void StepLife_Click(object sender, RoutedEventArgs e)
        {
            DeathOrLife();
        }

        public void DeathOrLife()
        {

            int[,] NumNeig = new int[NumCellsWidth, NumCellsHeight];
            for (int i = 0; i < NumCellsHeight; i++)
            {
                for (int j = 0; j < NumCellsWidth; j++)
                {

                    int iLink1 = i - 1;
                    if (iLink1 < 0)
                    { iLink1 = NumCellsHeight - 1; }

                    int iLink2 = i + 1;
                    if (iLink2 >= NumCellsHeight)
                    { iLink2 = 0; }

                    int jLink1 = j - 1;
                    if (jLink1 < 0)
                    { jLink1 = NumCellsWidth - 1; }

                    int jLink2 = j + 1;
                    if (jLink2 >= NumCellsWidth)
                    { jLink2 = 0; }

                    int fields = 0;

                    if (Neighbour[iLink1, jLink1].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[iLink1, j].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[iLink1, jLink2].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[i, jLink1].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[i, jLink2].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[iLink2, jLink1].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[iLink2, j].Fill == Brushes.Black)
                    { fields++; }
                    if (Neighbour[iLink2, jLink2].Fill == Brushes.Black)
                    { fields++; }

                    NumNeig[i, j] = fields;
                }
            }
            for (int i = 0; i < NumCellsWidth; i++)
            {
                for (int j = 0; j < NumCellsHeight; j++)
                {
                    if (NumNeig[i, j] < 2 || NumNeig[i, j] > 3)
                    {
                        Neighbour[i, j].Fill = Brushes.Red;
                    }
                    else if (NumNeig[i, j] == 3)
                    {
                        Neighbour[i, j].Fill = Brushes.Black;
                    }
                }
            }
        }

        private void Animation_Click(object sender, RoutedEventArgs e)
        {

            if (PauseGame)
            {
                timer.Stop();
                PauseGame = false;
                Animation.Content = "Авто Жизнь";
                SaveButton.IsEnabled = true;
            }
            else
            {
                timer.Start();
                PauseGame = true;
                Animation.Content = "Пауза";
                SaveButton.IsEnabled = false;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            SavegamesF.ShowDialog();
            string NameSaveGame = SavegamesF.textBox.Text;


            for (int i = 0; i < NumCellsHeight; i++)
            {
                for (int j = 0; j < NumCellsWidth; j++)
                {
                    if (Neighbour[i, j].Fill == Brushes.Black)
                    {
                        db.SaveGame(i, j, 1, NameSaveGame);
                    }
                    else
                    {
                        db.SaveGame(i, j, 0, NameSaveGame);
                    }
                }
            }
        }
    }
}

     
