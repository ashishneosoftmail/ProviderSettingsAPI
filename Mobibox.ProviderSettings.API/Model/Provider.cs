using Amazon.DynamoDBv2.DataModel;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;



namespace Mobibox.ProviderSettings.API.Model
{
    [DynamoDBTable("MobiboxConfigurations")]
    public class Provider
    {
        [JsonPropertyName("PK")]
        [DynamoDBHashKey]
        public string? PK { get; set; }

        [JsonPropertyName("SK")]
        [DynamoDBRangeKey]

        public string? SK { get; set; }

        [JsonPropertyName("ID")]
        public string? ID { get; set; }

        [JsonPropertyName("Status")]
        public int? Status { get; set; }

        [JsonPropertyName("IDService")]
        public int? IDService { get; set; }

        [JsonPropertyName("IDClient")]
        public int? IDClient { get; set; }

        [JsonPropertyName("IDCountry")]
        public int? IDCountry { get; set; }

        [JsonPropertyName("IDOperator")]
        public int? IDOperator { get; set; }

        [JsonPropertyName("IDProvider")]
        public int? IDProvider { get; set; }

        [JsonPropertyName("RegDate")]
        public DateTime? RegDate { get; set; }

        [JsonPropertyName("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [JsonPropertyName("RequestSettings")]
        public RequestSettings? RequestSettings { get; set; }

        [JsonPropertyName("ResponseSettings")]
        public ResponseSettings? ResponseSettings { get; set; }        

    }

    public class RequestSettings
    {
        [JsonPropertyName("BasicURL")]
        public string? BasicURL { get; set; }

        [JsonPropertyName("DataType")]
        public string? DataType { get; set; }

        [JsonPropertyName("Action")]
        public string? Action { get; set; }

        [JsonPropertyName("Parameters")]
        public List<Parameters>? Parameters { get; set; }

        [JsonPropertyName("Headers")]
        public List<Headers>? Headers { get; set; }
    }

    public class Parameters
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }
    public class Headers
    {
        [JsonPropertyName("Key")]
        public string? Key { get; set; }

        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }

    public class ResponseSettings
    {
        [JsonPropertyName("DataType")]
        public string? ResponseType { get; set; }
    }
}
