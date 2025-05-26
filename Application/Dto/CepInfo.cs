using System.Text.Json.Serialization;

public class CepInfo
{
    [JsonPropertyName("cep")]
    public string ZipCode { get; set; }

    [JsonPropertyName("logradouro")]
    public string Street { get; set; }

    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("localidade")]
    public string City { get; set; }

    [JsonPropertyName("uf")]
    public string State { get; set; }

    [JsonPropertyName("erro")]
    public string Error { get; set; }
}