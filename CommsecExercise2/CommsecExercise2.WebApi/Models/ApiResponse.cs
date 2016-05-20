namespace CommsecExercise2.WebApi.Models
{
    public class ApiResponse<T>
    {
        public ApiStatus Status { get; set; }
        public T Data { get; set; }
        public ApiResponse()
        {
            Status = new ApiStatus() { IsSuccess = true };
        }
    }

    public class ApiStatus
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
    }
}