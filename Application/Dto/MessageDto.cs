namespace Application.Dto
{
    public record class MessageDto<T>(string Message, bool Success, T? Data)
    {
        public static MessageDto<T> SuccessResult(T? data, string? message = null)
            => new(message ?? "Operação realizada com sucesso.", true, data);

        public static MessageDto<T> FailureResult(string? message = null)
            => new(message ?? "Erro na operação.", false, default);
    }
}
