﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Webapi.Models
{
    public partial class OpenposeContext : DbContext
    {
        public OpenposeContext()
        {
        }

        public OpenposeContext(DbContextOptions<OpenposeContext> options)
            : base(options)
        {
        }

        public DbSet<ImageFile> ImageFile { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<SystemFeedback> SystemFeedback { get; set; }
        public DbSet<Detection> Detection { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<DetectionFeedback> DetectionFeedback{get;set;}
    }
}
