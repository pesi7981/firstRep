using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll;
using Dal;
using Bll.Utilities;
using System.Data.Entity;
using System.Windows.Forms;
using Bll.Logic;
using System.Xml.Serialization;
using System.IO;

namespace Bll.DTO
{
   public class dtoShop
    {
        public int Code { get; set; }
        public string NameShop { get; set; }
        public int CodeGetaway { get; set; }
        //רשימת קירות, רשימת עמודות, רשימת מוצרי חנות, רשימת נקודות גישה
        public List<dtoWall> Walls { get; set; }
        public List<dtoStand> Stands { get; set; }
        public List<dtoConnection> Connections { get; set; }
        public List<dtoGetaway> Getaways { get; set; }



        GeneralDB generalDB = new GeneralDB();

        public dtoShop()
        {       
        }
        public dtoShop(Shop s) {
            dtoShop s2 = new dtoShop();
            s2.Code = s.Code;
            s2.NameShop = s.NameShop;
            s2.NameShop = s.NameShop;
        }
        public static List<dtoStand> GetStands(int codeShop)
        {
            GeneralDB generalDB = new GeneralDB();
            //generalDB.MyDb.Shops.First().Stands.First().Shelves.First().ProductShelves.Add();
            return Converts.ToDtoStands(generalDB.MyDb.Stands.Where(s => s.CodeShop == codeShop).ToList());

        }
        public static List<GetawayProcI_Result> GetawayProcI_Results(int Code)
        {
            GeneralDB generalDB = new GeneralDB();
            return generalDB.MyDb.GetawayProcI(Code).ToList();
        }
        public static List<dtoShop> GetDtoShops()
        {
            GeneralDB generalDB = new GeneralDB();
            List<Shop> l = generalDB.MyDb.Shops.ToList();
            return Converts.ToDtoShops(l);
        }
     public static bool comparePassword(string str,int code)
        {
            GeneralDB generalDB = new GeneralDB();
            Shop s = generalDB.MyDb.Shops.Where(x=>x.Code == code).First();
            return s.Password == str;
        }
        public RequestResponse Insert(string password,string path)
        {
            RequestResponse r = new RequestResponse();
            Shop s =Converts.ToShop(this,password);

            //generalDB.Shops.Add(s);
            //MessageBox.Show("befor you have "+generalDB.MyDb.Shops.Count());
            s = resetCode(s);

            generalDB.MyDb.Shops.Add(s);
            //TODO: להעביד אותו חזרה
            try
            {
         generalDB.MyDb.SaveChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.InnerException.Message);
            }
            //MessageBox.Show("after you have "+generalDB.MyDb.Shops.Count());
            s=   generalDB.MyDb.Shops.ToList().Last();
            dtoShop dtos = Converts.ToDtoShop(s);
            if(dtos.Connections!=null&&dtos.Connections.Count>0)
                WriteDistanceMatrix(s,path);
            dtos.Connections = new List<dtoConnection>();
            r.Data = dtos;
            return r;          
        }

        private Shop resetCode(Shop s)
        {
            //s.CodeGetaway=s.CodeGetaway==0?   null:s.CodeGetaway;
            s.CodeGetaway = null;
            s.Walls.ToList().ForEach(w=>w.Alias=null);
            s.Stands.ToList().ForEach(a => a.Alias = null);

            if (s.Stands != null) s.Stands.ToList().ForEach((a) => { if (a.Shelves != null) a.Shelves.ToList().ForEach(z => z.Alias = null); else a = a; });
         // var v1=  s.Stands!=null? s.Stands.ToList()[0].Shelves!=null? s.Stands.ToList()[0].Shelves.ToList()[0].ProductShelves.ToList()[0].ProductShop.Product.Alias:null:null;
            //s.Stands.ToList().ForEach(y=> {
            //    y.Shelves != null ? y.Shelves.ToList().ForEach(z => z.ProductShelves.ToList().ForEach(x => x.ProductShop.Product.Alias.Alias1 = null))
            //    : null
            //    });
            return s;
        }

        public static void WriteDistanceMatrix(Shop s,string path)
        {
            Cell[,] distanceMatrix = DijkstraFunction.ComputeDikjstra(GetawayProcI_Results(s.Code), s.Connections.ToList());
            Cell[][] matrix = Converts.ToMatrix(distanceMatrix);
            Matrix m = new Matrix() { Mat = matrix };
            XmlSerializer xml = new XmlSerializer(typeof(Matrix));
            //StreamWriter sw = new StreamWriter(@"..\..\..\packages\files\"+s.NameShop+".xml");
            StreamWriter sw = new StreamWriter(@"..\..\ProgramData\superQuick\" + s.NameShop + ".xml");

            xml.Serialize(sw, m);
        }
        public static Cell[,] ReadDistanceMatrix(Shop s,string path)
        {
            Matrix m1 = new Matrix();
            XmlSerializer xml = new XmlSerializer(typeof(Matrix));
            //StreamReader sw = new StreamReader(@".\packages\files\" + s.NameShop + ".xml");
            //TODO: לעשות ניתוב לשרת
            //StreamReader sw = new StreamReader(@".\" + s.NameShop + ".xml");
            path = path + @"\" + s.NameShop + ".xml";
            StreamReader sw = new StreamReader(path);
            m1 = (Matrix)xml.Deserialize(sw);
            Cell[,] m2 = Converts.ToMat(m1);
            return m2;
        }

         /*   public RequestResponse Update()
        {
            //TODO: לעשות שיעדכן ולא יוסיף שוב נתונים כפולים
             RequestResponse r = new RequestResponse();
            Shop s =Converts.ToShop(this,"");

            //generalDB.Shops.Add(s);
            MessageBox.Show("befor you have "+generalDB.MyDb.Shops.Count());
            generalDB.MyDb.Shops.Add(s);
            generalDB.MyDb.SaveChanges();
            MessageBox.Show("after you have "+generalDB.MyDb.Shops.Count());
             s=   generalDB.MyDb.Shops.ToList().Last();
            dtoShop dtos = Converts.ToDtoShop(s);
            dtos.Connections = new List<dtoConnection>();
            r.Data = dtos;
            //כאן לעשות לקובץ זמל את דיקסטרת החנות 
            WriteDistanceMatrix(s);

            //Matrix m = new Matrix();
            //int width = Convert.ToInt32(s.Walls.Max(x => x.X2));
            //int height = (int)s.Walls.Max(x => x.Y2);
            //m.Mat = MatShopComputer.ComputeMat(s.Walls.ToArray(), s.Stands.ToArray(), s.Getaways.ToArray(), width, height);

            return r;          
        }*/

        public static  int [][] GetMap(int Code)
        {
            GeneralDB g = new GeneralDB();
            Shop s = g.MyDb.Shops.Where(x => x.Code == Code).FirstOrDefault();
            if (s == null)
                return null;
            int[][] matShop = new int[1][] ;
            try
            {
                var x =Convert.ToInt32( s.Walls.Max(a => a.X2));
                var y = Convert.ToInt32(s.Walls.Max(b => b.Y2));
                matShop = MatShopComputer.ComputeMat(s.Walls.ToArray(),s.Stands.ToArray(), s.Getaways.ToArray(), x+1,y+1);
               
            }
            catch { MessageBox.Show("erro"); }
            return matShop;
        }
        
    }
}
