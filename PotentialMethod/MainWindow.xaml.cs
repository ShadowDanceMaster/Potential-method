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

namespace PotentialMethod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //переменная изображения точки
        //the field of the point's image
        Rectangle rect = new Rectangle();
        //здесь будет фон
        //the background will be here
        Rectangle rectBG = new Rectangle();
        //здесь будет точка нажатия
        //we'll put the point here
        Point point = new Point();
        //здесь будут центры кругов
        //the centers of the cirles will be here
        Point pointGreen = new Point();
        Point pointBrown = new Point();
        Point pointGolden = new Point();

        public MainWindow()
        {
            InitializeComponent();
            btnCount.IsEnabled = false;
            btnClear.IsEnabled = false;
            this.Title = "Potential function method / Метод потенциальных функций";
            //добавляем невидимую точку
            //add the invisible point
            rect.Visibility = Visibility.Hidden;
            grid.Children.Add(rect);
            //создаём фон
            //create the background
            grid.Children.Add(rectBG);
            rectBG.Visibility = Visibility.Visible;
            rectBG.Width = grid.Width;
            rectBG.Height = grid.Height;
            rectBG.Fill = new SolidColorBrush(Colors.White);
            rectBG.SetValue(Grid.RowProperty, 0);
            rectBG.SetValue(Grid.ColumnProperty, 0);
            rectBG.SetValue(Grid.RowSpanProperty, grid.RowDefinitions.Count);
            rectBG.SetValue(Grid.ColumnSpanProperty, grid.ColumnDefinitions.Count);
            rectBG.SetValue(Grid.ZIndexProperty, -1);
            //устанавливаем расположение кругов
            //set the spot of each circle
            pointGreen.Y = Convert.ToInt32(elGreen.GetValue(Grid.RowProperty)) * (grid.Height / grid.RowDefinitions.Count) + (elGreen.Width / 2);
            pointGreen.X = Convert.ToInt32(elGreen.GetValue(Grid.ColumnProperty)) * (grid.Width / grid.ColumnDefinitions.Count) + (elGreen.Width / 2);
            pointBrown.Y = Convert.ToInt32(elBrown.GetValue(Grid.RowProperty)) * (grid.Height / grid.RowDefinitions.Count) + (elBrown.Width / 2);
            pointBrown.X = Convert.ToInt32(elBrown.GetValue(Grid.ColumnProperty)) * (grid.Width / grid.ColumnDefinitions.Count) + (elBrown.Width / 2);
            pointGolden.Y = Convert.ToInt32(elGolden.GetValue(Grid.RowProperty)) * (grid.Height / grid.RowDefinitions.Count) + (elGolden.Width / 2);
            pointGolden.X = Convert.ToInt32(elGolden.GetValue(Grid.ColumnProperty)) * (grid.Width / grid.ColumnDefinitions.Count) + (elGolden.Width / 2);
        }
        //при клике по сетке
        //called when you click on the grid
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //сохраняем позицию курсора
            //save the cursor's position
            point = Mouse.GetPosition(grid);
            //цвет точки
            //the colour of the point
            Color pointColor = Colors.Black;

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // считаем ряды в сетке
            //count the grid's rows
            foreach (var rowDefinition in grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // считаем колонке в сетке
            //count the grid's columns
            foreach (var columnDefinition in grid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
            //устанавливаем место точки
            //set the exact spot of the point
            point.Y = (grid.RowDefinitions[row].ActualHeight * (row + 1)) - grid.RowDefinitions[row].ActualHeight / 2;
            point.X = (grid.ColumnDefinitions[col].ActualWidth * (col + 1)) - grid.ColumnDefinitions[col].ActualWidth / 2;
            //создаём изображение точки
            //create the picture of the point
            CreateSquare((double)20, row, col, pointColor);
            btnCount.IsEnabled = true;
            btnClear.IsEnabled = true;
        }
        /// <summary>
        /// The point's creation / Создание точки
        /// </summary>
        /// <param name="width"></param>
        /// <param name="desiredRow"></param>
        /// <param name="desiredColumn"></param>
        /// <param name="color"></param>
        void CreateSquare( double width, int desiredRow, int desiredColumn, Color color)
        {
            rect.Visibility = Visibility.Visible;
            rect.Width = width;
            rect.Height = width;
            rect.Fill = new SolidColorBrush(color);
            rect.SetValue(Grid.RowProperty, desiredRow);
            rect.SetValue(Grid.ColumnProperty, desiredColumn);
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            Potentials pot = new Potentials();
            List<Point> circleCenters = new List<Point>();
            circleCenters.Add(pointGreen);
            circleCenters.Add(pointBrown);
            circleCenters.Add(pointGolden);
            List<double> widths= new List<double>();
            widths.Add(elGreen.Width);
            widths.Add(elBrown.Width);
            widths.Add(elGolden.Width);
            CircleEnum nearest = CircleEnum.Error;
            try
            {
                nearest = pot.MainFunc(widths, point, circleCenters);
                
            }
            catch (Exception ex)
            {
                rtb.AppendText(Environment.NewLine + ex.Message);
                return;
            }
            rtb.AppendText(Environment.NewLine + "Nearest neighbour / Ближайший сосед: " + nearest.ToString());
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            rtb.Document.Blocks.Clear();
        }
    }
}
