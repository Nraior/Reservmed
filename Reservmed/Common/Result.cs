namespace Reservmed.Common
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }

        protected Result(bool success, string? msg)
        {
            IsSuccess = success;
            Message = msg ?? String.Empty;
        }

        public static Result Success(string? msg) => new Result(true, msg);
        public static Result Error(string? errorMsg) => new Result(false, errorMsg);
    }

    public class Result<T> : Result
    {
        public T? Payload { get; private set; }

        protected Result(bool success, string msg, T? data) : base(success, msg)
        {
            Payload = data;
        }

        public static Result<T> Success(T data, string msg)
        {
            return new Result<T>(true, msg, data);
        }

        public static new Result<T> Error(string msg)
        {
            return new Result<T>(false, msg, default);
        }
    }
}
