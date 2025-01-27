namespace AutoJournal.Common.Response
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        public static Response<T> Success(T data, string message = "Operation Succesful.")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                ErrorCode = null
            };
        }
        /// <summary>
        /// Creates a failure response.
        /// </summary>
        public static Response<T> Failure(string message, string errorCode = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Data = default,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
