﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241031134721_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Entities.ColorEntity", b =>
                {
                    b.Property<Guid>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ColorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ColorId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Infrastructure.Entities.ExtraImagesEntity", b =>
                {
                    b.Property<Guid>("ExtraImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExtraImageId");

                    b.ToTable("ExtraImages");
                });

            modelBuilder.Entity("Infrastructure.Entities.ExtraProductImagesEntity", b =>
                {
                    b.Property<Guid>("ArticleNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ExtraImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArticleNumber", "ExtraImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("ExtraProductImages");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductEntity", b =>
                {
                    b.Property<Guid>("ArticleNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("money");

                    b.Property<string>("Ingress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("PrimaryImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArticleNumber");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductVariantEntity", b =>
                {
                    b.Property<Guid>("ProductVariantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ColorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductArticleNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SizeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProductVariantId");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductArticleNumber");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductVariants");
                });

            modelBuilder.Entity("Infrastructure.Entities.SizesEntity", b =>
                {
                    b.Property<Guid>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SizeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Infrastructure.Entities.StockEntity", b =>
                {
                    b.Property<Guid>("ProductVariantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductVariantId");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("Infrastructure.Entities.ExtraProductImagesEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.ProductEntity", "Product")
                        .WithMany("ExtraProductImages")
                        .HasForeignKey("ArticleNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Entities.ExtraImagesEntity", "ExtraImages")
                        .WithMany("ExtraProductImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExtraImages");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductVariantEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.ColorEntity", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Entities.ProductEntity", "Product")
                        .WithMany("ProductVariants")
                        .HasForeignKey("ProductArticleNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Entities.SizesEntity", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("Infrastructure.Entities.StockEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.ProductVariantEntity", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("Infrastructure.Entities.ExtraImagesEntity", b =>
                {
                    b.Navigation("ExtraProductImages");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductEntity", b =>
                {
                    b.Navigation("ExtraProductImages");

                    b.Navigation("ProductVariants");
                });
#pragma warning restore 612, 618
        }
    }
}