using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll.Logic;
using Bll.Utilities;
using Dal;

namespace Bll.DTO
{
    public class dtoAlias
    {
        public int Code { get; set; }
        public string TextAlias { get; set; }
        public bool IsPrivate { get; set; }
        public int? Parent { get; set; }
        public string Color { get; set; }

        public static List<dtoAlias> GetAliases()
        {
            GeneralDB db = new GeneralDB();
            List<dtoAlias> dtoAliases = Converts.ToDtoAliases(db.MyDb.Aliases.ToList());
            return dtoAliases;
        }

        public dtoAlias Insert()
        {
            GeneralDB db = new GeneralDB();
            Alias a = Converts.ToAlias(this);
            db.MyDb.Aliases.Add(a);
            db.MyDb.SaveChanges();
            a = db.MyDb.Aliases.ToList().Last();
            dtoAlias dtoAlias = Converts.ToDtoAlias(a);
            return dtoAlias;
        }
    }
}
