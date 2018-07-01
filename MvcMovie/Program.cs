﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace MvcMovie
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            BuildWebHost(args).Run();
//        }

//        public static IWebHost BuildWebHost(string[] args) =>
//            WebHost.CreateDefaultBuilder(args)
//                .UseStartup<Startup>()
//                .Build();
//    }
//}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie;

namespace MvcMovie
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var context = services.GetRequiredService<MvcMovieContext>();
					context.Database.Migrate();
					SeedData.Initialize(services);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}