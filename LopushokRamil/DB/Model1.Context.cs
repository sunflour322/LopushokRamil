﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LopushokRamil.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LopushokRamilEntities : DbContext
    {
        public LopushokRamilEntities()
            : base("name=LopushokRamilEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Material> Material { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductMaterial> ProductMaterial { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<TypeMaterial> TypeMaterial { get; set; }
        public DbSet<TypeProduct> TypeProduct { get; set; }
    }
}
