using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PotentialMethod
{
    public class Potentials
    {
        Dictionary<CircleEnum, double> circleDistances = new Dictionary<CircleEnum, double>();
        Dictionary<CircleEnum, double> circlePotentials = new Dictionary<CircleEnum, double>();
        private int BinaryFunc(double r)
        {
            return r <= 1 ? 1 : 0;
        }
        /// <summary>
        /// Quartic kernel / Квартическое ядро 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private double Quartic(double r)
        {
            return (15.0D / 16.0D) * Math.Pow((1 - r * r), 2) * BinaryFunc(r);
        }
        /// <summary>
        /// The distance between two points / Расстояние между точками
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        /// <summary>
        /// Potential method or the k nearest neighbours method / Метод потенциальных функций или k ближайших соседей
        /// </summary>
        /// <param name="widths">They need to be sorted by CircleEnum / Должны быть отсортированы по CircleEnum</param>
        /// <param name="point">User's point / Точка отмеченная пользователем</param>
        /// <param name="centersOfCircles">They need to be sorted by CircleEnum / Должны быть отсортированы по CircleEnum</param>
        /// <returns></returns>
        public CircleEnum MainFunc(List<double> widths, Point point, List<Point> centersOfCircles)
        {
            //проверка
            //check
            var v = Enum.IsDefined(typeof(CircleEnum), (centersOfCircles.Count - 1));
            var q = Enum.IsDefined(typeof(CircleEnum), (widths.Count - 1));
            if (!v || !q)
                throw new Exception("Неверное количество входных данных");
            for(int i=0; i < centersOfCircles.Count; i++)//count the distances to the centers / считаем расстояния до центров
            {
                //в словаре ключ - номер круга, значение - расстояние от точки до его центра
                // the dictionary's key is the number of the circle, the value is the distance between the point and the center of the circle
                circleDistances.Add((CircleEnum)i, Distance(point, centersOfCircles[i]));
            }
            //сортировка по увеличению расстояния
            //sort by increasing distance
            circleDistances= circleDistances.OrderBy(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
            double max = 0;
            CircleEnum result = CircleEnum.NoCircle;
            for (int i = 0; i < circleDistances.Count; i++)
            {
                //интенсивности принимаются равными 1. Считаем потенциальные функции
                //Intensities = 1. Let's count the potential functions.
                circlePotentials.Add(circleDistances.ElementAt(i).Key, Quartic(circleDistances.ElementAt(i).Value / widths[(int)circleDistances.ElementAt(i).Key]));
                if (circlePotentials.ElementAt(i).Value > max)//choose max value/выбираем максимальное значение
                {
                    max = circlePotentials.ElementAt(i).Value;
                    result = circlePotentials.ElementAt(i).Key;
                }
            }
            return result;
        }
    }
}
