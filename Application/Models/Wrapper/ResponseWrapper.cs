using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Wrapper
{
    public class ResponseWrapper<T> : IResponseWrapper , IResponseWrapper<T>
    {
        public ResponseWrapper()
        {
            
        }
        public List<string> Messages { get; set; } = [];
        public bool IsSuccessful { get ; set; }

        public T Data { get; set; }

        public new static ResponseWrapper<T> Fail()
        {
            return new ResponseWrapper<T> { IsSuccessful = false};
        }
        public static ResponseWrapper<T> Fail(string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = false , Messages = new List<string> { message } };
        }
        public static ResponseWrapper<T> Fail(List<string> messages)
        {
            return new ResponseWrapper<T> { IsSuccessful = false, Messages = messages };
        }
        public new static Task<ResponseWrapper<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<ResponseWrapper<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }
       
        public new static Task<ResponseWrapper<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static ResponseWrapper<T> Success()
        {
            return new ResponseWrapper<T> { IsSuccessful = true };
        }
        public static ResponseWrapper<T> Success(string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = true, Messages = new List<string> { message } };
        }
        public static ResponseWrapper<T> Success(T data)
        {
            return new ResponseWrapper<T> { IsSuccessful =true , Data = data };
        }
        public static ResponseWrapper<T> Success( T data ,string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = true , Data = data , Messages = new List<string> { message } };
        }
        public static ResponseWrapper<T> Success(T data  ,List<string> messages)
        {
            return new ResponseWrapper<T> { IsSuccessful = true , Data = data, Messages = messages };
        }

        public static Task<ResponseWrapper<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<ResponseWrapper<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<ResponseWrapper<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success (data , message));
        }

        
    }
}
