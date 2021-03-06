﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ShopWayEntities : DbContext
    {
        public ShopWayEntities()
            : base("name=ShopWayEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alias> Aliases { get; set; }
        public virtual DbSet<Connection> Connections { get; set; }
        public virtual DbSet<Getaway> Getaways { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAlia> ProductAlias { get; set; }
        public virtual DbSet<ProductShelf> ProductShelves { get; set; }
        public virtual DbSet<Shelf> Shelves { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<Stand> Stands { get; set; }
        public virtual DbSet<Wall> Walls { get; set; }
        public virtual DbSet<ProductShop> ProductShops { get; set; }
    
        public virtual ObjectResult<GetawayProcI_Result> GetawayProcI(Nullable<int> codeShop)
        {
            var codeShopParameter = codeShop.HasValue ?
                new ObjectParameter("codeShop", codeShop) :
                new ObjectParameter("codeShop", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetawayProcI_Result>("GetawayProcI", codeShopParameter);
        }
    }
}
