namespace Reddnet.Application.Validation
{
    public record Result
    {
        public bool IsError { get; init; }
        public string Message { get; init; }

        public static Result Ok()
            => new Result
            {
                IsError = false,
                Message = "OK"
            };

        public static Result Failed(string message)
            => new Result
            {
                IsError = true,
                Message = message
            };

        public override string ToString()
            => (this.IsError ? "Failed" : "Succeeded") + $" with message {this.Message}.";
    }

    public record Result<T> : Result
    {
        #pragma warning disable CS8632
        public T? Data { get; init; }
        #pragma warning restore CS8632

        public static Result<T> Ok(T data)
            => new Result<T>
            {
                Data = data,
                IsError = false,
                Message = "OK"
            };

        public new static Result<T> Failed(string message)
            => new Result<T>
            {
                IsError = true,
                Message = message
            };

        public override string ToString()
            => (this.IsError ? "Failed" : "Succeeded") + $" with message {this.Message}.";
    }
}
