using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sharedlayer.Dto
{
    public class Response<T> where T :class
    {
        public T Data { get;private set; }
        public int StatusCode { get;private set; }
        public ErrorDto Error { get;private set; }
        [JsonIgnore]
        public bool IsSuccessful { get;private set; }//kendi iç yapım için kullancam bunu
        
        public static Response<T> Success(T data,int statuscode)
        {
            return new Response<T>()
            {
                Data = data,
                StatusCode = statuscode,
                IsSuccessful=true
            };
        }
        public static Response<T> Success(int statuscode)
        {
            return new Response<T>() { Data = default, StatusCode = statuscode,IsSuccessful=true };
        }
        public static Response<T> Fail(ErrorDto dto,int statuscode)
        {
            return new Response<T>() { Error = dto, StatusCode = statuscode,IsSuccessful=false };
        }
        public static Response<T> Fail(string message,int statuscode,bool isshow)
        {
            var errordto = new ErrorDto(message, isshow);
            return new Response<T>() { Error = errordto, StatusCode = statuscode,IsSuccessful=false };
        }
       



    }
}
