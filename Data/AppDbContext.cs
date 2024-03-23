﻿using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApplication2.Areas.ProjectManagement.Models;


namespace WebApplication2.Data
{
    public class AppDbContext : DbContext


    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)

        {   }


        

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
    }
}
