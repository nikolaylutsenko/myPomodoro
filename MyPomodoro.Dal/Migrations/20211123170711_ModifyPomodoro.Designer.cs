// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPomodoro.Dal.Context;

#nullable disable

namespace MyPomodoro.Dal.Migrations
{
    [DbContext(typeof(Context.AppContext))]
    [Migration("20211123170711_ModifyPomodoro")]
    partial class ModifyPomodoro
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("MyPomodoro.Core.Entities.Pomodoro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PomodoroDate")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly?>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Pomodoros");
                });
#pragma warning restore 612, 618
        }
    }
}
