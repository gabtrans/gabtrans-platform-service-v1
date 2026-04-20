using System;
namespace GabTrans.Application.DataTransfer
{
	public class ApiResponse
	{
        public string Message { get; set; }
        public bool Success { get; set; }
        public object? Data { get; set; }
    }
}

