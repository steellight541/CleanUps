namespace CleanUps.Shared.ErrorHandling
{
    /// <summary>
    /// Represents the outcome of an operation, encapsulating either successful data or an error message.
    /// This class uses HTTP-like status codes to indicate the result of the operation.
    /// </summary>
    /// <typeparam name="T">The type of data returned in case of a successful operation.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets the HTTP-like status code indicating the result of the operation.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// Success is defined as a status code in the range 200-299.
        /// </summary>
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

        /// <summary>
        /// Gets the data associated with a successful operation.
        /// This will be null if the operation was not successful or if no data was returned.
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// Gets the error message if the operation failed.
        /// This will be null if the operation was successful.
        /// </summary>
        public string ErrorMessage { get; private set; }

        // Private Constructors to enforce factory method usage

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class for success scenarios with data.
        /// </summary>
        /// <param name="statusCode">The HTTP-like status code (200 or 201).</param>
        /// <param name="data">The data associated with the successful operation.</param>
        private Result(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class for success scenarios without data.
        /// </summary>
        /// <param name="statusCode">The HTTP-like status code (204 or 304).</param>
        private Result(int statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class for error scenarios.
        /// </summary>
        /// <param name="statusCode">The HTTP-like status code (e.g., 400, 404, 500).</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        private Result(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        // Factory methods for success scenarios

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating a successful operation with data (status code 200 OK).
        /// </summary>
        /// <param name="data">The data to include in the result.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 200 and the provided data.</returns>
        public static Result<T> Ok(T data) => new(200, data);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating a successful creation operation with data (status code 201 Created).
        /// </summary>
        /// <param name="data">The data to include in the result.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 201 and the provided data.</returns>
        public static Result<T> Created(T data) => new(201, data);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating a successful operation with no content (status code 204 No Content).
        /// </summary>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 204 and no data.</returns>
        public static Result<T> NoContent() => new(204);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating that the requested resource has not been modified (status code 304 Not Modified).
        /// </summary>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 304 and no data.</returns>
        public static Result<T> NotModified() => new(304);

        // Factory methods for error scenarios

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating a bad request (status code 400 Bad Request).
        /// </summary>
        /// <param name="errorMsg">The error message describing the bad request.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 400 and the provided error message.</returns>
        public static Result<T> BadRequest(string errorMsg) => new(400, errorMsg);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating an unauthorized access attempt (status code 401 Unauthorized).
        /// </summary>
        /// <param name="errorMsg">The error message describing the unauthorized access.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 401 and the provided error message.</returns>
        public static Result<T> Unauthorized(string errorMsg) => new(401, errorMsg);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating forbidden access (status code 403 Forbidden).
        /// </summary>
        /// <param name="errorMsg">The error message describing the forbidden access.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 403 and the provided error message.</returns>
        public static Result<T> Forbidden(string errorMsg) => new(403, errorMsg);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating that the requested resource was not found (status code 404 Not Found).
        /// </summary>
        /// <param name="errorMsg">The error message describing the not found condition.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 404 and the provided error message.</returns>
        public static Result<T> NotFound(string errorMsg) => new(404, errorMsg);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating a conflict with the current state of the resource (status code 409 Conflict).
        /// </summary>
        /// <param name="errorMsg">The error message describing the conflict.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 409 and the provided error message.</returns>
        public static Result<T> Conflict(string errorMsg) => new(409, errorMsg);

        /// <summary>
        /// Creates a <see cref="Result{T}"/> indicating an internal server error (status code 500 Internal Server Error).
        /// </summary>
        /// <param name="errorMsg">The error message describing the internal error.</param>
        /// <returns>A new instance of <see cref="Result{T}"/> with status code 500 and the provided error message.</returns>
        public static Result<T> InternalServerError(string errorMsg) => new(500, errorMsg);

        /// <summary>
        /// Transforms the data inside the result using the provided transformation function if the result is a success with data (status code 200 or 201).
        /// For other success statuses without data (e.g., 204 No Content), returns a new result with the same status code.
        /// For error statuses, returns a new result with the same status code and error message.
        /// </summary>
        /// <typeparam name="TResult">The type to which the data is transformed.</typeparam>
        /// <param name="transformDelegate">The function to apply to the data if it exists and the operation was successful.</param>
        /// <returns>A new <see cref="Result{TResult}"/> with the transformed data or the original status and error message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the data is null for a status code that should have data (200 or 201).</exception>
        public Result<TResult> Transform<TResult>(Func<T, TResult> transformDelegate)
        {
            if (StatusCode == 200 || StatusCode == 201)
            {
                if (Data == null)
                {
                    return new Result<TResult>(500, "Data is null for status code that should have data.");
                }
                return new Result<TResult>(StatusCode, transformDelegate(Data));
            }
            else if (IsSuccess)
            {
                return new Result<TResult>(StatusCode);
            }
            else
            {
                return new Result<TResult>(StatusCode, ErrorMessage);
            }
        }
    }
}