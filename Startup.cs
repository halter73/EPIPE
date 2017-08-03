using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace shalter_kestrel_issue_1978
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                if (context.Request.Method == "POST")
                {
                    // Fake some more complicated rendering of a partial view
                    for (int i = 0; i < 10; i++)
                    {
                        await Task.Delay(100);
                        await context.Response.WriteAsync(i.ToString() + "<br>");
                    }

		    await context.Response.WriteAsync("POST successful!");
                    return;
                }

                var html = @"
<html>
  <head>
    <title>jQuery</title>
    <script type='text/javascript' src='http://code.jquery.com/jquery-3.2.1.js'></script>
  </head>

  <form id='PostForm'>
    <input type='text' id='RackId'>
    <input type='text' id='Name'>
    <input type='submit'>
  </form>

  <div id='modal'></div>

  <script type='text/javascript'>
    $('#PostForm').submit(function( event ) {
        //Ajax call
        var xhr = $.ajax({
            type: 'POST',
            url: '/url/path/', 
            data: {
                'id': $('#RackId').val(),
                'Name': $('#Name').val()
            },
            success: function(result){
                $('#modal').html(result);
            }
        });

        //return false;
    });
  </script>

</html>";

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(html);
            });
        }
    }
}
