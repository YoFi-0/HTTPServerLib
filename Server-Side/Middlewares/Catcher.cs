using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Text;
using Server_Side.Services.Classes;
using Domain.ReturnsModel;
using Newtonsoft.Json;

namespace Darkom_App.Middlewares
{
    public class Catcher
    {
        private readonly RequestDelegate _next;

        public Catcher(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _next(context).Wait();
            }
            catch (Exception ex)
            {
                var err_msg = "Server Error !!!";
                var json_res = new ReturnModel
                {
                    IsSucceeded = false,
                    Comment = err_msg
                };
                Console.WriteLine("Handler Catched: " + ex.Message);
                if (ex.Message.Contains("!!--!!-"))
                {
                    Console.WriteLine("coco");
                    json_res.Comment = ex.Message.Replace("!!--!!-", "").Replace("One or more errors occurred. (", "").Replace($"{ex.Message[ex.Message.Length - 1]}", "");
                }
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json_res));
                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }

        }
    }
}