using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Sharedlayer.Dto;
using Sharedlayer.ExceptionCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Sharedlayer.Extensions
{
    public static class CustomExceptionHandle
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(cfg =>
            {
                cfg.Run(async context =>
                {
                    context.Response.StatusCode = 500;//kendi içimde hata meydana geldi budan dolayı 500
                    context.Response.ContentType = "application/json";

                    var errorfeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorfeature!=null)
                    {
                        var ex = errorfeature.Error;

                        ErrorDto errorDto = null;

                        if (ex is CustomException)
                        {
                            errorDto = new ErrorDto(ex.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(ex.Message, false);

                        }

                        var response = Response<ErrorDto>.Fail(errorDto, 500);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }

                });
            });
        }
    }
}
