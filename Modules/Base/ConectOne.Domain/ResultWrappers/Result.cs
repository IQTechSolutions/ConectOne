namespace ConectOne.Domain.ResultWrappers
{
    /// <summary>
    /// Represents the result of an operation, including success status and messages.
    /// </summary>
    public class Result : IBaseResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        public Result()
        {
        }

        /// <summary>
        /// Gets or sets the messages associated with the result.
        /// </summary>
        public List<string> Messages { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        /// <returns>A failed result.</returns>
        public static IBaseResult Fail()
        {
            return new Result { Succeeded = false };
        }

        /// <summary>
        /// Creates a failed result with a message.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A failed result with a message.</returns>
        public static IBaseResult Fail(string message)
        {
            return new Result { Succeeded = false, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Creates a failed result with multiple messages.
        /// </summary>
        /// <param name="messages">The failure messages.</param>
        /// <returns>A failed result with multiple messages.</returns>
        public static IBaseResult Fail(List<string> messages)
        {
            return new Result { Succeeded = false, Messages = messages };
        }

        /// <summary>
        /// Creates a failed result asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<IBaseResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        /// <summary>
        /// Creates a failed result with a message asynchronously.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<IBaseResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        /// <summary>
        /// Creates a failed result with multiple messages asynchronously.
        /// </summary>
        /// <param name="messages">The failure messages.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<IBaseResult> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful result.</returns>
        public static IBaseResult Success()
        {
            return new Result { Succeeded = true };
        }

        /// <summary>
        /// Creates a successful result with a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful result with a message.</returns>
        public static IBaseResult Success(string message)
        {
            return new Result { Succeeded = true, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Creates a successful result asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<IBaseResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        /// <summary>
        /// Creates a successful result with a message asynchronously.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<IBaseResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    /// <summary>
    /// Represents the result of an operation with a data payload, including success status and messages.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class Result<T> : Result, IBaseResult<T>
    {
        /// <summary>
        /// Gets or sets the data payload of the result.
        /// </summary>
        public T Data { get; set; } = default!;

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        /// <returns>A failed result.</returns>
        public new static Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        /// <summary>
        /// Creates a failed result with a message.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A failed result with a message.</returns>
        public new static Result<T> Fail(string message)
        {
            return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Creates a failed result with multiple messages.
        /// </summary>
        /// <param name="messages">The failure messages.</param>
        /// <returns>A failed result with multiple messages.</returns>
        public new static Result<T> Fail(List<string> messages)
        {
            return new Result<T> { Succeeded = false, Messages = messages };
        }

        /// <summary>
        /// Creates a failed result asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public new static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        /// <summary>
        /// Creates a failed result with a message asynchronously.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public new static Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        /// <summary>
        /// Creates a failed result with multiple messages asynchronously.
        /// </summary>
        /// <param name="messages">The failure messages.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public new static Task<Result<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful result.</returns>
        public new static Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        /// <summary>
        /// Creates a successful result with a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A successful result with a message.</returns>
        public new static Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Creates a successful result with a data payload.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <returns>A successful result with a data payload.</returns>
        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        /// <summary>
        /// Creates a successful result with a data payload and a message.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <param name="message">The success message.</param>
        /// <returns>A successful result with a data payload and a message.</returns>
        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { message } };
        }

        /// <summary>
        /// Creates a successful result with a data payload and multiple messages.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <param name="messages">The success messages.</param>
        /// <returns>A successful result with a data payload and multiple messages.</returns>
        public static Result<T> Success(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = messages };
        }

        /// <summary>
        /// Creates a successful result asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public new static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        /// <summary>
        /// Creates a successful result with a message asynchronously.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public new static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        /// <summary>
        /// Creates a successful result with a data payload asynchronously.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        /// <summary>
        /// Creates a successful result with a data payload and a message asynchronously.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <param name="message">The success message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}

