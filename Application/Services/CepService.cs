using System.Text.Json;
using Application.Dto;
using Application.Interfaces;

namespace Application.Services
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<MessageDto<CepInfo>> SearchZipCodeAsync(string zipCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                return MessageDto<CepInfo>.FailureResult("CEP inválido");

            var url = $"https://viacep.com.br/ws/{zipCode}/json/";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return MessageDto<CepInfo>.FailureResult("Erro ao buscar CEP");

            var json = await response.Content.ReadAsStringAsync();
            var cepInfo = JsonSerializer.Deserialize<CepInfo>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return MessageDto<CepInfo>.SuccessResult(cepInfo);
        }
    }
}
