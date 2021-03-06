﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Context:DbContext
    {
        public Context() : base("name=Constr")
        {

        }
        public DbSet<Area> Area { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<ShopRentOrTransfer> ShopTransfer { get; set; }
        public DbSet<ShopBegRent> ShopBegRent { get; set; }
        public DbSet<ErrorUrl> ErrorUrl { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<ShopRentOrTransfer>().ToTable("ShopRentOrTransfer");
            modelBuilder.Entity<ShopBegRent>().ToTable("ShopBegRent");
            modelBuilder.Entity<ErrorUrl>().ToTable("ErrorUrl");

        }
    }
}
