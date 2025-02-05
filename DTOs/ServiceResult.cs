namespace StudentManagementSystem.DTOs
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message) => new ServiceResult<T> { Success = true, Message = message, Data = data };
        public static ServiceResult<T> FailureResult(string message) => new ServiceResult<T> { Success = false, Message = message, Data = default };
    }
}
