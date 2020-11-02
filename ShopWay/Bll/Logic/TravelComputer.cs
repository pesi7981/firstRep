using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bll.DTO;
using Bll.Utilities;
using Dal;


namespace Bll.Logic
{
    public class TravelComputer
    {
        public static int CodeShop { get; set; }

        public static List<Goal> MainComputeTravel(int[] CodeProducts, Point pStart, int numShop, bool endCash,string path)
        {
            CodeShop = numShop;
            GeneralDB dB = new GeneralDB();
            Shop s = dB.MyDb.Shops.Where(x => x.Code == numShop).FirstOrDefault();
            List<GetawayProcI_Result> getawayProcI_Results = dB.MyDb.GetawayProcI(numShop).ToList();
            int width = Convert.ToInt32(s.Walls.Max(x => x.X2)) + 1;
            int height = (int)s.Walls.Max(x => x.Y2) + 1;
            //TODO: להחליט אם מחשבים כאן 
            int[][] matShop = MatShopComputer.ComputeMat(s.Walls.ToArray(), s.Stands.ToArray(), s.Getaways.ToArray(), width, height);
            Cell[,] baseDistanceMatrix = dtoShop.ReadDistanceMatrix(s,path);
           // Cell[,] baseDistanceMatrix = DijkstraFunction.ComputeDikjstra(getawayProcI_Results,s.Connections.ToList());
            List<Wall> cashes = null;
            if (endCash)
                cashes = s.Walls.Where(x => x.Alias.TextAlias == "cash").ToList();
            List<Product> product = dB.MyDb.Products.Where(p => CodeProducts.Contains(p.Code)).Distinct().ToList();
            if (pStart == null)
                pStart =UtilitiesFunctions.MidPoint( Converts.ToDtoGetawayI(getawayProcI_Results.Where(x => x.Code == s.CodeGetaway).First()));
            //foreach (var item in productShops)
            //{
            //    MessageBox.Show(item.Product.Alias.TextAlias);
            //}
            //levels
            Dictionary<Stand, List<Product>> ExtraStand = Level1GroupProductsByStand(product.ToArray());
            //TODO: לעשות שיקבץ לאזורים רק אם צריך
            Dictionary<KeyArea, Area> productArea;
            //if (ExtraStand.Count() <= 10)
            //    productArea = Level2NotGroupStandsByArea(ExtraStand, getawayProcI_Results);
            //else
                productArea = Level2GroupStandsByArea(ExtraStand, getawayProcI_Results,s);
            List<KeyValuePair<KeyArea, Area>> productAreaList = Level3ConvertDictToList( pStart, productArea, matShop, getawayProcI_Results, s.Connections.ToList(), cashes,Convert.ToInt32(s.CodeGetaway),s.Stands.ToList(),s);
            Cell[,] matrix = Level4ComputeDistanceMatrix(productAreaList, baseDistanceMatrix, pStart, cashes);
            Cell[,] matrixToTsp = Level5CutMatrix(matrix, productAreaList.Count, matrix.GetLength(1));

            return Level6ComputeGoalsTravel(matrix, matrixToTsp, matrix.GetLength(1) - matrixToTsp.GetLength(1) - 1, matrixToTsp.GetLength(1), productAreaList, getawayProcI_Results, pStart, cashes.Count());

        }



        #region function level1 מקבץ מוצרים לפי סטנד
        public static Dictionary<Stand, List<Product>> Level1GroupProductsByStand(Product[] products)
        {
            Dictionary<Stand, List<Product>> ExtraStand = new Dictionary<Stand, List<Product>>();
            //level 1: group the product by the shelves
            //key: The common stand, value: list of the product [in the shop]
            //הייתי רוצה שבמקום הלולאה יהיה בשאילתת לינק פשוטה, איך עושים שלכל סטנד יהיה רשימת מוצרים
            //  var v= products.GroupBy(x => x.ProductShelves.First().Shelf.Stand).ToDictionary(y=>y.Key);

            foreach (var item in products)
            {
                //TODO: לטפל במוצרים שאין להם מיקום
                if (item.ProductShelves.Count == 0) continue;
                Stand s = item.ProductShelves.First().Shelf.Stand;
                Stand anotherKey = ExtraStand.Keys.FirstOrDefault(x => x.Equals(s));
                if (anotherKey==null)
                    ExtraStand.Add(s, new List<Product>() { item });
                else //או
                    ExtraStand[anotherKey].Add(item);
            }
            return ExtraStand;
        }
        #endregion
        //TODO: להחליף קוד 
        #region function level2 מקבץ את הסטנדים לפי איזור
        public static Dictionary<KeyArea, Area> Level2GroupStandsByArea(Dictionary<Stand, List<Product>> ExtraStand,List<GetawayProcI_Result> getaways,Shop shop)
        {
            //level 2: מקבץ את הסטנדים שבאותו אזור -כלומר יש להם אותם השערים
            //key: המפתח הוא השערים של האזור וגם השערים הכי קרובים של האזור
            //value: הערך הוא איזור המכיל רשימת סטנדים, נקודת התחלת אזור, נקודת סוף אזור
            Dictionary<KeyArea, Area> productArea = new Dictionary<KeyArea, Area>();
            List<Connection> connections;
            Stand stand;
            foreach (var productStand in ExtraStand)
            {
                KeyArea key = new KeyArea();
                stand = productStand.Key;
                connections = stand.GetConnections(shop);
                key.Nearestes = connections.Where(x => x.Nearest == true).Select(x => x.Getaway).ToList();
                key.Getaways = Converts.ToGetawayProcResult(connections.Select(x => x.Getaway).ToList(), dtoShop.GetawayProcI_Results(CodeShop));                     
                KeyArea anotherKey = productArea.Keys.FirstOrDefault(x => x.Equals(key));
                if (anotherKey == null)
                {
                    Area ee = new Area();
                    ee.ExtraStand[productStand.Key] = productStand.Value;
                    productArea.Add(key, ee);
                }
                else
                {
                    productArea[anotherKey].ExtraStand.Add(productStand.Key, productStand.Value);
                }
            }
            return productArea;
        }
        #endregion

        #region function level3 מעביר את המילון לרשימה, מחשב איזור נקודת התחלה ומוסיף לרשימה, מוסיף סוף, מחשב נקודות איזור
        public static List<KeyValuePair<KeyArea, Area>> Level3ConvertDictToList( Point pStart, Dictionary<KeyArea, Area> productArea,int [][] matShop,List<GetawayProcI_Result> getaways,List<Connection> connections,List<Wall> cashes, int CodeStartGetaway,List<Stand> stands,Shop shop)
        {
            //level 3: אחרי שקבצנו עלינו:
            //להוסיף אזור התחלה, להוסיף אינדקס של רשימה 
            //ולחשב נקודת התחלה וסוף לכל אזור
            List<KeyValuePair<KeyArea, Area>> productAreaList = new List<KeyValuePair<KeyArea, Area>>();
            //ראשית כל אזור א' הוא אזור ההתחלה
            KeyArea SPkeyArea = new KeyArea()
            { Getaways=UtilitiesFunctions.StartPointManagment(pStart,matShop,getaways,  CodeStartGetaway,stands,shop) };
           //אזור ההתחלה הוא ללא סטנדים /מוצרים כלל 
            productAreaList.Add( new KeyValuePair<KeyArea, Area>(SPkeyArea,new Area() { P1=pStart,P2=pStart}));
            foreach (var pair in productArea)
            {
                //חישוב נקודות התחלה וסוף של אזור
                pair.Value.calculatePoints();
                //מעבירים לרשימה
                productAreaList.Add(pair);
            }
            //נקודות סיום
            foreach (var c in cashes)
            {
            KeyArea EPkeyArea2 = new KeyArea()
            { Getaways = Converts.ToGetawayProcResult(connections.Where(x => x.TypeDest == Convert.ToInt32(eTypeConnection.wall) && x.CodeDest == c.Code).ToList().Select(x => x.Getaway).ToList(), getaways), Nearestes = null };
                dtoWall w = Converts.ToDtoWall(c);
            //אזור הסוף הוא ללא סטנדים /מוצרים כלל 
            productAreaList.Add(new KeyValuePair<KeyArea, Area>(EPkeyArea2, new Area() { P1=w.P1,P2=w.P2}));
            }
            return productAreaList;
        }
        #endregion

        #region function level4 תרחיב את המטריצת סמיכויות של החנות גם אזורים
       public static Cell[,] Level4ComputeDistanceMatrix(List<KeyValuePair<KeyArea, Area>> productAreaList, Cell[,] baseDistanceMatrix,Point pStart,List<Wall> cashes)
        {
            int numCashes = cashes.Count();
            //שלב 4 מרחיבים את טבלת המרחקים הבסיסית של החנות
            //תוך התחשבות בכך ש'אזור' ההתחלה הוא ה'איזור' הראשון של טבלת החנות
            int LenBaseDistanceMatrix = baseDistanceMatrix.GetLength(1);
            int lenBig = LenBaseDistanceMatrix + productAreaList.Count();
            Cell[,] BigMatrix = new Cell[lenBig, lenBig];
            //מעתיקים את המטריצה של הדיקסטרה למטריצה הגדולה
            for (int i = 0; i < LenBaseDistanceMatrix; i++)
                for (int j = 0; j < LenBaseDistanceMatrix; j++)
                    BigMatrix[i, j] = baseDistanceMatrix[i, j];
            //עובר על האיזורים, א,ב,ג
            for (int i = LenBaseDistanceMatrix; i < lenBig; i++)
            {
                #region מילוי מטריצה מאיזור לשערים שלו
                //עובר על רשימת שערים של האיזור ומשבץ מרחק במטריצה
                foreach (GetawayProcI_Result getway in productAreaList[i - LenBaseDistanceMatrix].Key.Getaways)
                {
                    Cell c = new Cell();
                    //עבור האזור הראשון או האחרון המרחק מתמלא באופן ייחודי, מלשאר האזורים
                    //או אתה מנקודות הסיום       || או שאתה נקודת התחלה
                    if (i == LenBaseDistanceMatrix || i >= lenBig - numCashes && i < lenBig)
                    {
                        Point p;
                        if (i == LenBaseDistanceMatrix)
                            p = pStart;
                        else
                        {
                            dtoWall dtoWall = Converts.ToDtoWall(cashes[i - lenBig + numCashes]);
                            p = UtilitiesFunctions.MidPoint(dtoWall.P1, dtoWall.P2);
                        }
                        c.distance = UtilitiesFunctions.getDistanceFromSP(getway, p);
                       
                    }
                    else //אם זה איזור רגיל ולא נקודת התחלה/סיום
                        c.distance = UtilitiesFunctions.MinDist(productAreaList[i - LenBaseDistanceMatrix].Value, Converts.ToDtoGetawayI(getway));

                    c.i = i;
                    c.j = Convert.ToInt32(getway.I);
                    c.Parent = Convert.ToInt32(getway.I); /*כי יש לי קשת אליו, כי זה גיטוואי שלי*/
                    BigMatrix[i, Convert.ToInt32(getway.I)] = c;
                    //לצד המקביל
                    BigMatrix[Convert.ToInt32(getway.I), i] = c;
                }
                #endregion
                #region מילוי מטריצה מהאיזור לכל השערים הזרים
                for (int j = 1; j < LenBaseDistanceMatrix; j++)
                {
                    //בודקת איך הכי קצר להגיע אליך מנקודות הגישה שלי
                    double min = int.MaxValue;
                    Cell minCell = new Cell();
                    minCell.i = i;
                    minCell.j = j;
                    //אם אתה נקודת גישה שלי תמשיך כי  שחישבתי קודם
                    if (BigMatrix[i, j] != null)
                        continue;
                    else //אם אתה לא אחד מנקודות הגישה שלי
                        foreach (GetawayProcI_Result getway in productAreaList[i - LenBaseDistanceMatrix].Key.Getaways)
                        {
                            //המרחק א' לבי+המרחק מבי לסי 
                            if (BigMatrix[Convert.ToInt32(getway.I), j].distance + BigMatrix[i, Convert.ToInt32(getway.I)].distance < min)
                            {
                                min = BigMatrix[Convert.ToInt32(getway.I), j].distance + BigMatrix[i, Convert.ToInt32(getway.I)].distance;
                                minCell.distance = Convert.ToInt32(min);
                                minCell.Parent = Convert.ToInt32(getway.I);
                            }
                        }
                    BigMatrix[i, j] = minCell;
                    BigMatrix[j, i] = minCell;//לצד המקביל
                }
                #endregion
                // תמלא את המרחק שלך לעצמך, 0
                Cell c2 = new Cell();
                c2.distance = 0;
                c2.Parent = 0;
                c2.i = i;
                c2.j = i;
                BigMatrix[i, i] = c2;
            }
            //בשלב זה המטריצה הגדולה מלאה בחציה. חושב המרחק מכל איזור לכל השערים. יש לחשב מרחק בין איזור לאיזור
     
            //להגיע מאיזור לאיזור, א,ב, ג,
            for (int i = LenBaseDistanceMatrix; i < lenBig; i++)
            {
                for (int j = LenBaseDistanceMatrix; j < lenBig; j++)
                {
                    if (i != j &&BigMatrix[i,j]==null)
                    {
                        int min = int.MaxValue;
                        Cell c = new Cell();
                        c.i = i;
                        c.j = j;
                        // במקרה שאזורים אלו בעלי אותם השערים - זה אומר שאפשר לגשת ישירות ביניהם. נחשב אמצע קטע של איזור ראשון ואמצע קטע של איזור שני ופשוט נחשב מרחק
                        if (UtilitiesFunctions.IsSame(productAreaList[i - LenBaseDistanceMatrix].Key.Getaways, productAreaList[j - LenBaseDistanceMatrix].Key.Getaways))
                        {
                            //אם כל נקודת גישה של א היא גם נקודת גישה של ב
                            //א=i
                            //ב=j
                            c.Parent = i;
                            //להגיע מאיזור לאיזור כשהם צמודים ולא צריך לעבור דרך שער
                            //אם אחד מהם זה נקודת התחלה אין לו ערך, נחשב לחוד
                            I2Points a = productAreaList[i - LenBaseDistanceMatrix].Value;
                            I2Points b = productAreaList[j - LenBaseDistanceMatrix].Value;

                            c.distance = UtilitiesFunctions.MinDist(a, b);
                            BigMatrix[i, j] = c;
                            Cell c2 = new Cell() { i = j, j = i, distance = c.distance, Parent = j };
                            BigMatrix[j, i] = c2;
                        }
                        //אם האיזורים אינם בעלי אותם שערים, תעבור על השערים של המקור, של אי
                        else
                        {
                            foreach (GetawayProcI_Result getway in productAreaList[i - LenBaseDistanceMatrix].Key.Getaways)
                            {
                                if (BigMatrix[j, Convert.ToInt32(getway.I)].distance + BigMatrix[i, Convert.ToInt32(getway.I)].distance < min)
                                {
                                    c.Parent =Convert.ToInt32( getway.I);
                                    min = BigMatrix[j, Convert.ToInt32(getway.I)].distance + BigMatrix[i, Convert.ToInt32(getway.I)].distance;
                                    c.distance = min;
                                }
                            }
                            BigMatrix[i, j] = c;
                            //לצד המקביל
                            Cell c2 = new Cell() { i = j, j = i, distance = c.distance, Parent = c.Parent };
                            BigMatrix[j, i] = c2;
                        }
                    }
                }


            }
           // printMat(BigMatrix); ;
            return BigMatrix;
        }

       

        #endregion

        #region function level5  צור מטריצת סמיכויות מהאיזורים בלבד
        public static  Cell[,] Level5CutMatrix(Cell[,] matrix, int countAreas, int matrixLen)
        {
            int from = matrixLen - countAreas;
            Cell[,] AreasMatrix = new Cell[countAreas, countAreas];
            for (int i = from; i < matrixLen; i++)
            {
                for (int j = from; j < matrixLen; j++)
                    AreasMatrix[i - from, j - from] = matrix[i, j];
            }

            //TODOO: האם אין אפשרות פשוט לשנות את המצביע???? איזה סי שארפ מטופשת
            //לא לשכוח שהאינדקס משתנה של האיזורים [לכאורה בערך אינדקס משאירים את האינדקס של האיזור]
            return AreasMatrix;
        }
        #endregion

        #region level6 החזר את מסלול האיסוף בחנות 
        public static List<Goal> Level6ComputeGoalsTravel(Cell[,] BigMatrix, Cell[,] matrixToTsp, int numGeteway, int numArea, List<KeyValuePair<KeyArea, Area>> productAreaList,List<GetawayProcI_Result> getawayProcI_Results,Point pStart,int numCashes)
        {
            //חישוב מסלול אופטימלי 
            int[] order =TspFunctions.ManageTsp(matrixToTsp,numArea-numCashes, numArea);
            //יצור מערך של לקיטה-שערים ומוצרים על פי המסלול שחושב
            List<Goal> way = ComputeWayAnswer(BigMatrix, order,numArea-numCashes, numGeteway,productAreaList,getawayProcI_Results,pStart);
            foreach(Goal goal in way)
            {
                if (goal.kind == 'g')
                {
                    GetawayProcI_Result g = getawayProcI_Results.Where(x => x.Code == goal.num).First();
                    goal.midPoint = UtilitiesFunctions.MidPoint(new Point(Convert.ToInt32(g.X1.ToString()), Convert.ToInt32(g.Y1.ToString())), new Point(Convert.ToInt32(g.X2.ToString()), Convert.ToInt32(g.Y2.ToString())));
                }
                else if (goal.midPoint == null)
                {
                    //נקודת קופה או סיום היא ריקה יש לשאוב אותה מאן שהוא
                }
            }
            return way;
            }
            

        private static List<Goal> ComputeWayAnswer(Cell[,] BigMatrix, int[] order,int numAreas,int numGetaways, List<KeyValuePair<KeyArea, Area>> productAreaList,List<GetawayProcI_Result> getawayProcI_Results,Point pStart)
        {
            
            List<Goal> way = new List<Goal>();
            List<Goal> AddWay = new List<Goal>();

            int from, to;
            int gFrom, gTo;
            //ובכן בזאת מוסיפים את האזור הראשון שהוא רק התחלה ללא מוצרים
            way.Add(new Goal( 'p',0, null,pStart) );
            Goal w;//אמור להיות ריק-

            //עוברים על כל האיזורים
            for (int i = 0; i < numAreas ; i++)
            {
                from = order[i] + numGetaways + 1;
                to = order[i + 1] + numGetaways + 1;
                gFrom = BigMatrix[from, to].Parent;
                if (gFrom == 0) continue;
                //אם לא מגיעים ישירות מאיזור לאיזור אלא עוברים דרך נקודות גישה
                if (!(gFrom == from || gFrom == to))
                {
                    gTo = BigMatrix[gFrom, to].Parent;
                    if (gFrom == gTo) { gTo = gFrom; gFrom = BigMatrix[from, gFrom].Parent;  }
                    AddWay.Add(new Goal('g',gFrom,null,null));
                    HelpComputeAnsFromGtoG(BigMatrix, gFrom, gTo, AddWay, false, 0);
                    AddWay.Add(new Goal('g', gTo, null,null));
                }
                    way.AddRange(AddWay);
                    AddWay.Clear();
                //הגענו לאיזור היעד

                Area area=productAreaList[order[i+1]].Value;
                if (area != null) { 
                    w = new Goal('a',to- numGetaways - 1,null,UtilitiesFunctions.MidPoint(area.P1,area.P2));
                 OpenArea(way.Last(), w.num, productAreaList,way,getawayProcI_Results);
                   way.Add(w);   }
            
            }               
            return way;
        }




        public static void HelpComputeAnsFromGtoG(Cell[,] BigMatrix, int from, int to, List<Goal> answers, bool b, int reelTo)
        {
            int cur = BigMatrix[from, to].Parent;
            if (cur == from || cur == to ||cur==0)
                return;
            else
            {
             HelpComputeAnsFromGtoG(BigMatrix, from, cur, answers, false, 0);
             answers.Add(new Goal('g',cur,null,null));
            }
        }


        private static void OpenArea(Goal gw, int num, List<KeyValuePair<KeyArea, Area>> productAreaList, List<Goal> way,List<GetawayProcI_Result> getawayProcI_Results)
        {
          
            GetawayProcI_Result g =getawayProcI_Results.Where(x => x.Code == gw.num).FirstOrDefault();
            if(g==null)
            {
                g = new GetawayProcI_Result();
                g.X1 = way.Last().midPoint.X;
                g.X2 = way.Last().midPoint.X;
                g.Y1 = way.Last().midPoint.Y;
                g.Y2 = way.Last().midPoint.Y;
            }
            List<ProductShop> products = new List<ProductShop>();
            //למיין את הסטנדים לפי המרחק מנקודת גישה
           Area area = productAreaList[num].Value;
            if (area != null)
            {
                List<int> orederStand = area.sortStands(Converts.ToDtoGetawayI(g));
                foreach (var item in orederStand)
                {
                    var curExtraStand = area.ExtraStand.Where(x => x.Key.Code == item).First();
                    Point p = UtilitiesFunctions.MidPoint(new Point(Convert.ToInt32(curExtraStand.Key.X1.ToString()), Convert.ToInt32(curExtraStand.Key.Y1.ToString())), new Point(Convert.ToInt32(curExtraStand.Key.X2.ToString()), Convert.ToInt32(curExtraStand.Key.Y2.ToString())));
                    way.Add(new Goal('s',item,Converts.ToDtoProducts( curExtraStand.Value) ,p));
                }
            }
        }

        #endregion

        public List<Goal> DOmain(List<Product> products, Point pStart, Wall cash, int numShop, List<GetawayProcI_Result> getaways, List<Connection> connections)
        {
            string path = @"C:\ProgramData\superQuick";
            CodeShop = numShop;
            GeneralDB DB = new GeneralDB();
            Shop shop = DB.MyDb.Shops.Where(x => x.Code == numShop).First();
            List<GetawayProcI_Result> getawayProcI_Results = DB.MyDb.GetawayProcI(1).ToList();
             Cell[,] distanceMatrix = DijkstraFunction.ComputeDikjstra(DB.MyDb.GetawayProcI(1).ToList(),shop.Connections.ToList());
            //Cell[,] distanceMatrix = dtoShop.ReadDistanceMatrix(shop,path);
            ShopWayEntities shopWay = new ShopWayEntities();
            pStart.X = 8; pStart.Y = 22;
            int[][] matShop = MatShopComputer.ComputeMat(shop.Walls.ToArray(), shop.Stands.ToArray(), shop.Getaways.ToArray(), 29, 29);
            string[] arr = { "מים", "שוגי", "ציפס קידס", "קורונפלקס", "כריות", "סוכריות אדומות", "במבה", "תן צאפ", "ביסלי", "ציפס מקסיקני", "פופקורן", "במבה אדומה", "דוריטוס" };
            //string[] arr = { "watter", "shugi", "chips kids", "coronflex", "cariot", "red swits", "bamba", "ten chap", "bisli", "chips meksicani", "popcoren", "red bamba", "new snak" };
            //List<Product> productsTrap = DB.MyDb.Products.Where(x => arr.Contains(x.Alias.TextAlias) && x.GetProductShops().Where(y => y.CodeShop == numShop).ToList().Count > 0).ToList();
            List<ProductShop> productShops = shop.GetProductShop();
            bool b = productShops.Where(x => x.Code == 1).FirstOrDefault() != null;
            List<Product> productsShop = shop.ProductShops.Select(x => x.Product).ToList();
            
            List<Product> productsTrap = productsShop.Where(x => arr.Contains(x.Alias.TextAlias)).ToList();
                //List<Product> productsTrap&& productShops.Where(y=>y.CodeProduct==x.Code).FirstOrDefault()!=null).ToList();
            Dictionary<Stand, List<Product>> ExtraStand = Level1GroupProductsByStand(productsTrap.ToArray());
            Dictionary<KeyArea, Area> productArea;
            //TODO: לעשות שלא יקבץ לאזורים אם לא צריך
            //if (ExtraStand.Count()<=10)
            //    productArea= Level2NotGroupStandsByArea(ExtraStand, getawayProcI_Results);
            //else
                productArea = Level2GroupStandsByArea(ExtraStand, getawayProcI_Results,shop);
            List<Wall> cashes = shop.Walls.Where(w => w.Alias.TextAlias == "cash").ToList();
            List<KeyValuePair<KeyArea, Area>> productAreaList = Level3ConvertDictToList( pStart, productArea, matShop, getaways, connections, cashes,Convert.ToInt32( shop.CodeGetaway),shop.Stands.ToList(),shop);
            Cell[,] matrix = Level4ComputeDistanceMatrix(productAreaList, distanceMatrix, pStart, cashes);
            Cell[,] matrixToTsp = Level5CutMatrix(matrix, productAreaList.Count, matrix.GetLength(1));
            return Level6ComputeGoalsTravel(matrix, matrixToTsp, matrix.GetLength(1) - matrixToTsp.GetLength(1) - 1, matrixToTsp.GetLength(1), productAreaList, getawayProcI_Results, pStart, cashes.Count());

        }

        private static Dictionary<KeyArea, Area> Level2NotGroupStandsByArea(Dictionary<Stand, List<Product>> ExtraStand, List<GetawayProcI_Result> getawayProcI_Results,Shop shop)
        {
            //key: המפתח הוא השערים של האזור וגם השערים הכי קרובים של האזור
            //value: הערך הוא איזור המכיל רשימת סטנדים, נקודת התחלת אזור, נקודת סוף אזור
            Dictionary<KeyArea, Area> productArea = new Dictionary<KeyArea, Area>();
            foreach (var productStand in ExtraStand)
            {
                KeyArea key = new KeyArea();
                Stand s = productStand.Key;
                key.Nearestes = null;
                List<Connection> connections = s.GetConnections(shop);
                key.Getaways = Converts.ToGetawayProcResult(connections.Select(x => x.Getaway).ToList(), dtoShop.GetawayProcI_Results(CodeShop));
                KeyArea anotherKey = productArea.Keys.FirstOrDefault(x => x.Equals(key));
                    Area ee = new Area();
                    ee.ExtraStand[productStand.Key] = productStand.Value;
                    productArea.Add(key, ee);
            }
            return productArea;
        }

        private static void printMat(Cell[,] bigMatrix)
        {
            string[] alefBet = { "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט", "י", "יא", "יב", "יג", "יד", "טו", "טז", "יז" };
            string c; int temp;
            string str = ""; string strp = "";
            for (int i = 1; i < bigMatrix.GetLength(0); i++)
            {
                c = alefBet[i - 1];
                if(str=="") str += " \t  " + c;
                else str += "  \t  " + c;
            }
            str += "      \t";


            str += "\n";
            str += "___________________________________________________________________________________________________________________________________________________________________________________";
            str += "\n";

            for (int i = 1; i < bigMatrix.GetLength(0); i++)
            {
                c = alefBet[i - 1];
                str += " " + c+"    \t";
                for (int j = 1; j < bigMatrix.GetLength(1); j++)
                {
                    str += bigMatrix[i, j].distance + "     \t";
                }
                str += "\n";
                str += "_______________________________________________________________________________________________________________________________________________________________________";
                str += "\n";
            }
            MessageBox.Show(str);
        }
    }
}
