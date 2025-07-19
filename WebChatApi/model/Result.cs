namespace WebChatApi.model
{
    public class Result
    {
        public bool IsSuccess { get; }


        public dynamic data { get; }
        private Result(bool isSuccess, dynamic data)
        {
            IsSuccess = isSuccess;
            this.data = data;
        }

        public static Result Success(dynamic data)
        {
            return new Result(true, data);
        }
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Failure(dynamic errors)
        {
            return new Result(false, errors);
        }
    }
}
