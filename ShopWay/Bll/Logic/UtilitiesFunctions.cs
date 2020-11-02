using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll.Utilities;
using Dal;
using Bll.DTO;
namespace Bll.Logic
{
    public static class UtilitiesFunctions
    {       
        #region טיפול בנקודת התחלה
        //נקודת התחלת הקניה
        public static List<GetawayProcI_Result> StartPointManagment(Point pStart, int[][] matShop, List<GetawayProcI_Result> getaways,int CodeStartGetaway,List<Stand> stands,Shop shop)
        {
            List<int []> listAns= GetStartGetaway(matShop, pStart, matShop.Length, matShop[0].Length);         
            List<GetawayProcI_Result> l = new List<GetawayProcI_Result>();
            listAns.ForEach(z => l.Add(getaways.Where(d => d.Code == z[0] && z[1] ==Convert.ToInt32( eMatMeaning.getaway)).FirstOrDefault()));
            l.RemoveAll(z => z == null);
            if (l == null||l.Count()==0)
            {
              int code= listAns.Where(x => x[1] == Convert.ToInt32(eMatMeaning.stand)).First()[0];
               Getaway g = stands.Where(y => y.Code == code).First().GetConnections(shop).First().Getaway;
                l.Add(Converts.ToGetawayProcResult(g, getaways));
            }
            if (l == null || l.Count() == 0)
                l.Add(getaways.Where(x => x.Code == CodeStartGetaway).First());

            //החזרת אזור התחלה שיוסף לרשימת האזורים
            return l;
        }
        public static List<int[]> GetStartGetaway(int[][] mat, Point pStart, int n, int m)
        {
            List<int[]> lis = new List<int[]>();
            //לך קדימה, אחורה, ימינה, שמאלה
            int i, j;
            i = pStart.X; j = pStart.Y;
            while (i < n && mat[i][j] == 0) i++;
            if (i < n)
                if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.getaway))
                    lis.Add(new int[2] { mat[i][j] / 100, 3 });
                else if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.stand))
                    lis.Add(new int[2] { mat[i][j] / 100, 2 });
            i = pStart.X;
            while (i >= 0 && mat[i][j] == 0) i--;
            if (i >= 0)
                if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.getaway))
                    lis.Add(new int[2] { mat[i][j] / 100, 3 });
                else if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.stand))
                    lis.Add(new int[2] { mat[i][j] / 100, 2 });
            i = pStart.X;

            while (j < m && mat[i][j] == 0) j++;
            if (j < m)
                if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.getaway))
                    lis.Add(new int[2] { mat[i][j] / 100, 3 });
                else if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.stand))
                    lis.Add(new int[2] { mat[i][j] / 100, 2 });
            j = pStart.Y;
            while (j >= 0 && mat[i][j] == 0) j--;
            if (j >= 0)
                if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.getaway))
                    lis.Add(new int[2] { mat[i][j] / 100, 3 });
                else if (mat[i][j] % 100 == Convert.ToInt32(eMatMeaning.stand))
                    lis.Add(new int[2] { mat[i][j] / 100, 2 });
            return lis;
        }

        /*לא טוב, רקורסיה
        public static void getStartGetawayBed(int[,] mat, int x, int y, int distance, int n, int m, List<SPGetaway> listAns)
        {
            if (mat[x, y] / 10 > 0)
            {
                int num = mat[x, y] / 10;
                if (listAns != null)
                {
                    var v = listAns.Where(z => z.code == num);
                    if (v != null)
                    {
                        var v2 = v.First();
                        if (distance < v2.dist) v2.dist = distance;
                    }
                    else
                    {
                        listAns.Add(new SPGetaway() { code = num, dist = distance });
                    }
                }
            }
            else if (mat[x, y] > 0)
                return;
            else if (x == n && y == m) return;
            else if (distance >= Max(m, n)) return;
            else
            {
                if (x + 1 < n) getStartGetawayBed(mat, x + 1, y, distance + 1, n, m, listAns);
                if (x - 1 > 0) getStartGetawayBed(mat, x - 1, y, distance + 1, n, m, listAns);
                if (y + 1 < m) getStartGetawayBed(mat, x, y + 1, distance + 1, n, m, listAns);
                if (y - 1 > 0) getStartGetawayBed(mat, x, y - 1, distance + 1, n, m, listAns);
            }
        }
        */

        #endregion

        #region פונקציות לטיפול בממשק 2 נקודות
        public static int Max(int m, int n)
        {
            return m > n ? m : n;
        }

        public static int getDistanceFromSP(GetawayProcI_Result getaway, Point pStart)
        {
            dtoGetaway g = Converts.ToDtoGetawayI(getaway);
            Point p3 = MidPoint(g.P1, g.P2);
            return CalculteDist(pStart, p3);
        }

        public static bool IsSame(List<GetawayProcI_Result> getaways1, List<GetawayProcI_Result> getaways2)
        {
            if (getaways1.Count() != getaways2.Count) return false;
            foreach (var item in getaways1)
            {
                if (!getaways2.Contains(item))
                    return false;
            }
            return true;
        }

        /*
        //למיין סטנדים פסי
        public static Dictionary<Stand, List<ProductShop>> OrderStandsByGetaway(I2Points g, Dictionary<Stand, List<ProductShop>> extraStand)
        {

            List<OrderStand> orders = new List<OrderStand>();
            OrderStand minOrder = new OrderStand() { s = null, dist = int.MaxValue };
            int dist;
            I2Points before = null;
            for (int i = 0; i < extraStand.Count(); i++)
            {
                foreach (var item in extraStand)
                {
                    if (orders.Where(x => x.s.Code == item.Key.Code).Count() == 0)
                    {
                        dist = MinDist(Converts.ToDtoStand(item.Key), g);
                        if (dist < minOrder.dist) { before = Converts.ToDtoStand(item.Key); minOrder.dist = dist; minOrder.s = item.Key; }
                    }
                }
                orders.Add(minOrder);
                g = before;
            }
            Dictionary<Stand, List<ProductShop>> liststand = new Dictionary<Stand, List<ProductShop>>();
            for (int i = 0; i < extraStand.Count; i++)
                liststand.Add(orders[i].s, orders[i].listproduct);
            extraStand = liststand;
            return liststand;
        }

    */
        public static Point MidPoint(I2Points i2)
        {
            return MidPoint(i2.P1, i2.P2);
        }
            public static Point MidPoint(Point p1, Point p2)
        {

            Point p3 = new Point();
            p3.X = (p1.X + p2.X) / 2;
            p3.Y = (p1.Y + p2.Y) / 2;
            return p3;

        }



        //המרת נקודות ממספרים
        public static void Dopoint(Stand stand, Getaway g)
        {
            Point p1 = new Point() { X = Convert.ToInt32(stand.X1), Y = Convert.ToInt32(stand.Y1) };
            Point p2 = new Point() { X = Convert.ToInt32(stand.X2), Y = Convert.ToInt32(stand.Y2) };
            Point p3 = MidPoint(p1, p2);
            Point p1g = new Point() { X = Convert.ToInt32(g.X1), Y = Convert.ToInt32(g.Y1) };
            Point p2g = new Point() { X = Convert.ToInt32(g.X2), Y = Convert.ToInt32(g.Y2) };
        }

        public static int MinDist(I2Points s1, I2Points s2)
        {
            Point ps1 = MidPoint(s1.P1, s1.P2);
            Point ps2 = MidPoint(s2.P1, s2.P2);
            return CalculteDist(ps1, ps2);
        }

        public static int CalculteDist(Point p1, Point p2)
        {
            //להפעיל את הsql
            double d = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            d = Math.Sqrt(d);
            return Convert.ToInt32(d);
        }

    }
    #endregion
}
