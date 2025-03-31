namespace CleanUps.Shared.ErrorHandling
{
    public class Result<T>
    {
        //Can read more about http status codes here: https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status

        public int StatusCode { get; private set; }
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300; // HTTP success range
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        //Private Constructors to enforce factory method usage

        //Success range with data
        // - For status code: 200 Ok(T data)
        // - For status code: 201 Created(T data)
        private Result(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        //Success range with no data
        //For status code: 204 NoContent()
        //For status code: 304 NotModified()
        private Result(int statusCode)
        {
            StatusCode = statusCode;
        }

        //Error range
        //For status code: 400 BadRequest(string error)
        //For status code: 401 Unauthorized(string error)
        //For status code: 403 Forbidden(string error)
        //For status code: 404 NotFound(string error)
        //For status code: 409 Conflict(string error)
        //For status code: 500 InternalServerError(string error)
        private Result(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        // Factory methods for success scenarios
        public static Result<T> Ok(T data) => new(200, data);
        public static Result<T> Created(T data) => new(201, data);
        public static Result<T> NoContent() => new(204);
        public static Result<T> NotModified() => new(304);

        // Factory methods for error scenarios
        public static Result<T> BadRequest(string errorMsg) => new(400, errorMsg);
        public static Result<T> Unauthorized(string errorMsg) => new(401, errorMsg);
        public static Result<T> Forbidden(string errorMsg) => new(403, errorMsg);
        public static Result<T> NotFound(string errorMsg) => new(404, errorMsg);
        public static Result<T> Conflict(string errorMsg) => new(409, errorMsg);
        public static Result<T> InternalServerError(string errorMsg) => new(500, errorMsg);
    }

}
