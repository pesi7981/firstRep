using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dal;
namespace Bll.Logic
{
    class MatShopComputer
    {
        public static int[][] ComputeMat(Wall[] w, Stand[] s, Getaway[] g, int width, int height)
        {
            //מצייר מטריצה מהקירות
            int[][] mat = new int[width + 1][];
            for (int i = 0; i < width+1; i++)
            {
                mat[i] = new int[height];
            }
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    mat[i][ j] = 0;
            int x1, x2, y1, y2;
            //עובר על הקירות
            foreach (var item in w)
            {
                x1 = Convert.ToInt32(item.X1);
                x2 = Convert.ToInt32(item.X2);
                y1 = Convert.ToInt32(item.Y1);
                y2 = Convert.ToInt32(item.Y2);
                for (int i = x1; i <= x2; i++)
                    for (int j = y1; j <= y2; j++)
                        mat[i][ j] = 1;                                             
            }
            //עובר על הסטנדים
            Point p1;
            Point p2;
            foreach (var item in s)
            {
                if (item.Code == 104)
                    item.Code = item.Code;
                p1 = new Point();
                p2 = new Point();
                if ((item.X1 + item.Y1) < (item.X2 + item.Y2))
                {
                    p1.X = Convert.ToInt32(item.X1); p1.Y = Convert.ToInt32(item.Y1);
                    p2.X = Convert.ToInt32(item.X2); p2.Y = Convert.ToInt32(item.Y2);
                }
                else
                {
                    p1.X = Convert.ToInt32(item.X2); p1.Y = Convert.ToInt32(item.Y2);
                    p2.X = Convert.ToInt32(item.X1); p2.Y = Convert.ToInt32(item.Y1);
                }
                for (int i = p1.X; i < p2.X + 1; i++)
                {
                    for (int j = p1.Y; j < p2.Y + 1; j++)
                    {
                        mat[i][ j] = 2 + item.Code * 100;
                    }
                }
            }
            //עובר על הנקודות גישה
            foreach (var item in g)
            {
                p1 = new Point() { X = Convert.ToInt32(item.X1), Y = Convert.ToInt32(item.Y1) };
                p2 = new Point() { X = Convert.ToInt32(item.X2), Y = Convert.ToInt32(item.Y2) };
                for (int i = p1.X; i < p2.X + 1; i++)
                {
                    for (int j = p1.Y; j < p2.Y + 1; j++)
                    {
                        mat[i][j] = 3 + item.Code * 100;
                    }
                }
            }
            return mat;
        }

        private static void calculateGetwaysForPStart()
        {
            int[][] mat = ComputeMat(null,null,null,0,0);
            int[,] SecondMat = (mat.Clone())as int[,];
            Point p = new Point() { X = 4, Y = 5 };
            Queue<Point> tor = new Queue<Point>();
            
            tor.Enqueue(p);
            while (tor != null)
            {
                if (tor.First().X < SecondMat.Length && tor.First().Y < SecondMat.LongLength &&
                    tor.First().X >= 0 && tor.First().Y >= 0 && SecondMat[tor.First().X, tor.First().Y] > 10 && SecondMat[tor.First().X, tor.First().Y] !=11)
                {
                    p = tor.Dequeue();
                    tor.Enqueue(new Point(p.X, p.Y - 1));
                    tor.Enqueue(new Point(p.X, p.Y + 1));
                    tor.Enqueue(new Point(p.X + 1, p.Y + 1));
                    tor.Enqueue(new Point(p.X - 1, p.Y + 1));
                    tor.Enqueue(new Point(p.X - 1, p.Y - 1));
                    tor.Enqueue(new Point(p.X + 1, p.Y));
                    tor.Enqueue(new Point(p.X - 1, p.Y));
                    tor.Enqueue(new Point(p.X + 1, p.Y - 1));
                    SecondMat[p.X, p.Y] = 11;
                    //if p is a gateaway- enter it to the result array
                }
                else tor.Dequeue();
            }
         }

        }
    }


