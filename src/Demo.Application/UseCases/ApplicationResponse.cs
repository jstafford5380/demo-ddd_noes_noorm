using Demo.Application.UseCases.ManagingPets;

namespace Demo.Application.UseCases
{
    public class ApplicationResponse<T>
    {
        public bool IsSuccess { get; set; }
        
        public ResponseType ResponseType { get; }

        public T Entity { get; set; }

        public string Message { get; set; }

        public ApplicationResponse(bool isSuccess, ResponseType responseType, string message)
        {
            IsSuccess = isSuccess;
            ResponseType = responseType;
            Message = message;
        }

        public ApplicationResponse(bool isSuccess, ResponseType responseType, T entity, string message)
        {
            IsSuccess = isSuccess;
            ResponseType = responseType;
            Entity = entity;
            Message = message;
        }
    }
}