using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Logic
{
    class toDelete
    {
        //from tsp


        // Function to find the minimum weight Hamiltonian Cycle 
        //פונקציה למצוא משקל מינימלי של מעגל המילטון- יש הרבה מעגלים שעוברים בכל הנקודות, רוצים את המעגל המינימלי

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph">טבלת סמיכויות</param>
        /// <param name="v">מערך בוליאני האם ביקרו בנקודה</param>
        /// <param name="currPos">החוליה הנוכחית</param>
        /// <param name="n">מספר חוליות</param>
        /// <param name="count">מונה</param>
        /// <param name="cost">מחיר</param>
        /// <param name="ans">משתנה חיצוני, כל פעם אם הדרך הסופית שמצא יותר קצר  מחליף. המשקל המינימלי של המעגל</param>
        /// <returns>מערך של אינדקסים, סדר המסלול</returns>
        public static int tsp(int[,] graph, bool[] v, int currPos,
                  int n, int count, int cost, int ans, int[] ResultIndexes, int dest)
        {
            // If last node is reached and it has a link 
            // to the starting node i.e the source then 
            // keep the minimum value out of the total cost 
            // of traversal and "ans" 
            // Finally return to check for more possible values 
            //אם אפשר להגיע לחוליה האחרונה והיא מקושרת לחוליית ההתחלה אי.אי המקור אז תמשיך את הערך המינימלי מחוץ למחיר הכללי של הנסיעה והתשובה הסופית מחזירה לבדיקה לעוד ערכים אפשריים
            //אם זה החוליה האחרונה וגם יש גישה לחולית היעד, החוליה הראשונה
            if (count == n && graph[currPos, dest] > 0)
            {
                ans = Math.Min(ans, cost + graph[currPos, 0]);
                ResultIndexes[count - 1] = currPos;
                return ans;
            }


            // BACKTRACKING STEP 
            // Loop to traverse the adjacency list 
            // of currPos node and increasing the count 
            // by 1 and cost by graph[currPos,i] value 
            //תעבור על הטיולים של הרשימה הסמוכה לנקודה הנוכחית ותגדיל את המונה באחד ותתמחר עי גרף הסמיכויות 
            for (int i = 0; i < n; i++)
            {

                if (v[i] == false && graph[currPos, i] > 0)
                {
                    // Mark as visited 
                    //אם זה לא החוליה הנוכחית וגם עוד לא עברו שם אז תעבור שם
                    v[i] = true;
                    ResultIndexes[count] = i;
                    ans = tsp(graph, v, i, n, count + 1,
                        cost + graph[currPos, i], ans, ResultIndexes, dest);
                    // Mark ith node as unvisited 
                    v[i] = false;
                }
            }
            return ans;
        }







        public static Stopwatch stopwatch = new Stopwatch();
        static int numFunc = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph">טבלת סמיכויות</param>
        /// <param name="v">מערך בוליאני האם ביקרו בנקודה</param>
        /// <param name="currPos">החוליה הנוכחית</param>
        /// <param name="n">מספר חוליות</param>
        /// <param name="count">מונה</param>
        /// <param name="cost">מחיר</param>
        /// <param name="ans">משתנה חיצוני, כל פעם אם הדרך הסופית שמצא יותר קצר  מחליף. המשקל המינימלי של המעגל</param>
        /// <returns>מערך של אינדקסים, סדר המסלול</returns>
        public static int OurTsp(Cell[,] graph, bool[] v, int currPos,
                  int n, int count, int cost, int ans, int[] ResultIndexes, int[] arr, int[] worker, bool aproxy, int dest)
        {
            if (aproxy)
            {
                if (currPos == 0)
                    stopwatch.Restart();
                else
                    if (stopwatch.Elapsed.Minutes >= 1)
                    return ans;
            }

            worker[0]++;
            // If last node is reached and it has a link 
            // to the starting node i.e the source then 
            // keep the minimum value out of the total cost 
            // of traversal and "ans" 
            // Finally return to check for more possible values 
            //אם אפשר להגיע לחוליה האחרונה והיא מקושרת לחוליית ההתחלה אי.אי המקור אז תמשיך את הערך המינימלי מחוץ למחיר הכללי של הנסיעה והתשובה הסופית מחזירה לבדיקה לעוד ערכים אפשריים
            //אם זה החוליה האחרונה וגם יש גישה לחולית היעד, החוליה הנתונה
            if (count == n - 1 && graph[currPos, dest].distance > 0)
            {
                //ans = Math.Min(ans, cost + graph[currPos, 0]);
                //אם אתה מעדכן את המקסימום
                if (cost + graph[currPos, dest].distance < ans)
                {
                    ans = cost + Convert.ToInt32(graph[currPos, dest].distance);
                    arr[n - 1] = dest;
                    //תעתיק את המערך למערך התוצאה
                    //arr[count - 1] = currPos;/*??*/
                    for (int i = 0; i < n; i++)
                    {
                        ResultIndexes[i] = arr[i];
                    }
                    Console.Write("the func working: " + worker[0] + " the max now is " + ans + " : ");
                    //printer(ResultIndexes);
                }
                return ans;
            }
            // BACKTRACKING STEP 
            // Loop to traverse the adjacency list 
            // of currPos node and increasing the count 
            // by 1 and cost by graph[currPos,i] value 
            //תעבור על הטיולים של הרשימה הסמוכה לנקודה הנוכחית ותגדיל את המונה באחד ותתמחר עי גרף הסמיכויות 
            for (int i = 0; i < n - 1; i++)
            {

                if (v[i] == false && graph[currPos, i].distance > 0)
                {
                    // Mark as visited 
                    //אם זה לא החוליה הנוכחית וגם עוד לא עברו שם אז תעבור שם
                    v[i] = true;
                    arr[count] = i;
                    ans = OurTsp(graph, v, i, n, count + 1,
                        cost + Convert.ToInt32(graph[currPos, i].distance), ans, ResultIndexes, arr, worker, aproxy, dest);
                    // Mark ith node as unvisited 
                    v[i] = false;
                }
            }
            return ans;
        }
        //public static void printer(int[] arr)
        //{
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        Console.Write(arr[i] + " , ");
        //    }
        //    Console.WriteLine("");
        //}
        static void DOMain(string[] args)
        {
            int n;
            // n is the number of nodes i.e. V 

            //4 nodes
            //        int n = 4;
            //        int[,] graph = { 
            //    { 0, 10, 15, 20 }, 
            //    { 10, 0, 35, 25 }, 
            //    { 15, 35, 0, 30 }, 
            //    { 20, 25, 30, 0 } 
            //};

            //10 nodes
            //n = 10;
            //int[,] graph = {
            //      { 0, 1000, 15000, 20000,10000,20,24000,3000,500,7000},
            //      { 10, 0, 35, 50,10,207,400,30,58,700},
            //      { 20, 25, 30, 10,10,20,24,0,50,700} ,
            //      { 10, 10, 15, 20,0,20,24,30,50,70},
            //      { 10, 10, 35, 100,100,0,4,30,58,7000},
            //      { 15, 35, 10, 30,10,20,0,30,50,70},               
            //       { 15, 35, 0, 30,100,20,24,30,50,700},
            //      { 20, 25, 30, 0,1000,20,24,30,50,700} ,
            //      { 10, 10, 15, 20,10,20,24,30,0,70},
            //      { 10, 10, 35, 500,100,207,40,300,58,0}

            //};

            n = 11;
            int[,] graph = {
                  { 0, 1000, 15000, 20000,10000,20,24000,3000,500,7000,23},
                  { 10, 0, 35, 50,10,207,400,30,58,700,34},
                  { 20, 25, 30, 10,10,20,24,0,50,700,4} ,
                  { 10, 10, 15, 20,0,20,24,30,50,70,5},
                  { 10, 10, 35, 100,100,0,4,30,58,7000,43},
                  { 15, 35, 10, 30,10,20,0,30,50,70,23},
                   { 15, 35, 0, 30,100,20,24,30,50,700,43},
                  { 20, 25, 30, 0,1000,20,24,30,50,700,56} ,
                  { 10, 10, 15, 20,10,20,24,30,0,70,45},
                  { 10, 10, 35, 500,100,207,40,300,58,0,33},
                     { 10, 10, 35, 500,100,207,40,300,58,30,0}
            };

            //15 nodes
            //          n = 15;
            //       int[,] graph = {
            //      {0,2,3,4,5,6,7,8,9,10, 0, 10, 15, 20,100 }, 
            //      {10,0,3,4,5,6,7,8,9,10, 10, 0, 35, 50,10  }, 
            //      {10,20,0,4,5,6,7,8,9,10, 15, 35, 0, 30,100 }, 
            //      {10,20,3,0,5,6,7,8,9,10, 20, 25, 30, 30,1000 } ,
            //{10,20,3,4,0,6,7,8,9,10, 10, 10, 15, 20,30}, 
            //      {10,20,3,4,5,0,7,8,9,10, 10, 10, 35, 100,100  }, 
            //      {10,20,3,4,5,6,0,8,9,10, 15, 35, 10, 30,10}, 
            //      {10,20,3,4,5,6,7,0,9,10, 20, 25, 30, 10,10 } ,
            //{10,20,3,4,5,6,7,8,0,10, 10, 10, 15, 20,10}, 
            //      {10,20,3,4,5,6,7,8,9,0, 10, 10, 35, 500,100 },
            //      {10,20,37,43,65,56,77,68,59,170, 0, 10, 15, 20,100 }, 
            //      {10,20,73,74,35,66,77,87,97,170, 10, 0, 35, 50,10  }, 
            //      {10,20,36,64,53,66,73,83,69,10, 15, 35, 0, 30,100 }, 
            //      {10,20,53,34,65,66,77,78,39,160, 20, 25, 30, 0,1000 },
            //      {10,20,53,34,65,66,77,78,39,160, 20, 25, 30, 10,0 },      

            //  };

            // Boolean array to check if a node 
            // has been visited or not 
            bool[] v = new bool[n];
            int[] result = new int[n];
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = 0;
                arr[i] = 0;
            }
            Cell[,] c = new Cell[n, n];
            // Mark 0th node as visited 
            v[0] = true;
            int ans = int.MaxValue;
            int[] worker = new int[n];
            // Find the minimum weight Hamiltonian Cycle 
            //ans = tsp(graph, v, 0, n, 1, 0, ans, result); 
            ans = OurTsp(c, v, 0, n, 1, 0, ans, result, arr, worker, false, n - 1);
            // ans is the minimum weight Hamiltonian Cycle 
            Console.WriteLine("the result is " + ans);
            Console.WriteLine("the func run " + numFunc + " times for " + n + " nodes");
            Console.WriteLine();
            //DijkstraFunction.DOmain();
        }


    }
}
