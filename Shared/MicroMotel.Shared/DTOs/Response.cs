using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace MicroMotel.Shared.DTOs
{
    public class Response<T>
    {
        public T Data { get;  set; }
        [JsonIgnore]
        public int Status { get; private set; }
        [JsonIgnore]
        public bool IsSuccesful { get; private set; }

        public List<string> Errors { get; set; }
        
        public static Response<T>Success(T data,int status)
        {
            return new Response<T> { Data = data, Status = status,IsSuccesful=true };
        }
        public static Response<T>Success(int status)
        {
            return new Response<T> { Data=default(T),Status = status, IsSuccesful = true };
        }
        public static Response<T>Fail(List<string> error,int statuscode)
        {
            return new Response<T>
            {
                Errors=error,
                Status = statuscode,
                IsSuccesful=false
            };
        }
        public static Response<T> Fail (string error,int statuscode) 
        {
            return new Response<T>
            {
                Errors = new List<string>() { error },
                Status = statuscode,
                IsSuccesful = false
            };
        }

    }
}
