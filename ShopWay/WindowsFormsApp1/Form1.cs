using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll;
using Bll.Utilities;
using Bll.DTO;
using System.Diagnostics;
using System.Threading;
//using Dal;
using Bll;
using Bll.Logic;
using Dal;
using System.Xml.Serialization;
using System.IO;

namespace WindowsFormsApp1
{ 
    public partial class Form1 : Form
    {
      public Stopwatch stopwatch = new Stopwatch();
        string path = @"C:\ProgramData\superQuick";
        public Form1()
        {
            InitializeComponent();
        }
        //private int rec(int x)
        //{
        //    if(stopwatch.Elapsed.Minutes>=2|| x >= 10 || x == 0)
        //    {
        //        return x;
        //        stopwatch.Stop();
        //    }
                
        //    return rec(x+1);
        //}
     int  randomNum(int from, int to) { Random rnd = new Random();

            return rnd.Next(from, to);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ShopWayEntities shopWayEntities = new ShopWayEntities();
            //List<Alias> aliases = new List<Alias>();
            //aliases = shopWayEntities.Aliases.ToList();
            //treeView1.CreateObjRef(aliases[0]);
           




            //dataGridView1.DataSource =  dtoShop.GetDtoShops();

            //stopwatch.Start();
            //int y = rec(1);
            //MessageBox.Show(y.ToString() + " is " + stopwatch.Elapsed.Minutes.ToString());
            int[] arr = { 1, 2, 3, 4, 5, 6 };
            arr.OrderBy(g => randomNum(1, 5));
            string str = "";
            arr.ToList().ForEach(a=>str=str+' '+a);
            //MessageBox.Show(str);


            Bll.Logic.TravelComputer l = new TravelComputer();
         List<Goal> listans= l.DOmain(shopWayEntities.Products.ToList(),new Point(),shopWayEntities.Walls.ToList().Last(),shopWayEntities.Shops.First().Code,shopWayEntities.GetawayProcI(1).ToList(),shopWayEntities.Connections.ToList());

            string s =":"+ "המסלול שלך"+"\n";
            string p = "";
            for (int i = 0; i < listans.Count(); i++)
            {
                s += " kind: " + listans[i].kind + " code: " + listans[i].num+" point: (" + listans[i].midPoint.X+","+ listans[i].midPoint.Y+")" + "\n" ;
                if (listans[i].kind == 's' && listans[i].products != null)
                    MessageBox.Show("i am stand number "+listans[i].num+"and i have product "+listans[i].products[0].Alias.TextAlias);
            }
            MessageBox.Show(s);
            //ShopWayEntities s = new ShopWayEntities();
            //dataGridView1.DataSource = s.Products.ToList();         
        }



        private void button3_Click(object sender, EventArgs e)
        {
            dtoShop s1 = new dtoShop() { Code = 80, Connections = null,  NameShop = "ממתקולד" };
            dtoShop s2 = new dtoShop() { Code = 80, Connections = null,  NameShop = "סופרזול" };
            dtoShop s3 = new dtoShop() { Code = 80, Connections = null,  NameShop = "ממתיק" };
            dtoShop s4 = new dtoShop() { Code = 80, Connections = null,  NameShop = "מספרה" };
            s1.Insert("1234",path);
            s2.Insert("1234",path);
            s3.Insert("1234",path);
            s4.Insert("1234",path);
            MessageBox.Show("הוספו 3 "+ s2.NameShop);
            ShopWayEntities sho = new ShopWayEntities();
            dataGridView1.DataSource = sho.Shops.ToList() ;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
           List<ExtraAlias> extraAliases= ExtraAlias.GetAliasesShop(1);
            foreach (var item in extraAliases)
            {
                dataGridView1.DataSource = item.products;
                MessageBox.Show(item.Code.ToString());
            }
            dataGridView1.DataSource = extraAliases;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string str = "";
            for (int i = 1,j=21; i < 79; i++,j++)
            {
                str = str + "insert into ProductShelf values("+j+","+ i+") \r\n";
            }
            this.textBox2.Text = str;
        }
        //יוצר XML
        private void button6_Click(object sender, EventArgs e)
        {
            ShopWayEntities shopWayEntities = new ShopWayEntities();
            Shop s2 = shopWayEntities.Shops.ToList().First();
            dtoShop s3 = Converts.ToDtoShop(s2);
            XmlSerializer xml = new XmlSerializer(typeof(dtoShop));
            StreamWriter sw = new StreamWriter(@"E:\Data\shop.xml");
            xml.Serialize(sw, s3);
        }
        //מחלץ XML
        private void button2_Click(object sender, EventArgs e)
        {
            dtoShop s3 = new dtoShop();
            XmlSerializer xml = new XmlSerializer(typeof(dtoShop));
            StreamReader sw = new StreamReader(@"D:\Data\shop.xml");
            s3 = (dtoShop)xml.Deserialize(sw);
            MessageBox.Show("ok, get shop " + s3.NameShop);
            s3.Insert("1234",path);
        }
    }
}
