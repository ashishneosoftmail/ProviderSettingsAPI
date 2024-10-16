using Amazon.DynamoDBv2.DataModel;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;



namespace Mobibox.ProviderSettings.API.Model
{
    [DynamoDBTable("MobiboxConfigurations")]
    public class Provider
    {
        //Partition Key(PK)
        [DynamoDBHashKey]
        public string? PK { get; set; }  // Example: "PROVIDER#<ProviderID>"

        //Sort Key(SK) - Optional, useful for secondary sorting
        [DynamoDBRangeKey]
        public string? SK { get; set; }  // Example: "METADATA#<Timestamp>"
        public string? ID { get; set; }

        public string? Status { get; set; }

        public string? IDService { get; set; }

        public string? IDClient { get; set; }

        public string? IDCountry { get; set; }

        public string? IDOperator { get; set; }

        public string? IDProvider { get; set; }

       
        //public string? inputValue { get; set; }

        public DateTime? RegDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public RequestSettings? RequestSettings { get; set; }

        public ResponseSettings? ResponseSettings { get; set; }        

    }

    public class RequestSettings
    {
        public string? BasicURL { get; set; }

        [JsonPropertyName("DataType")]
        public string? DataType { get; set; }
        public string? Action { get; set; }
        public List<Attributes>? Attributes { get; set; }
        public List<Headers>? Headers { get; set; }
    }

    public class Attributes
    {
        //public string AttributevalueType { get; set; }
        //public string attributestaticValue { get; set; }
        //public bool? boolValue { get; set; }
        //public string datatype { get; set; }

        //public string dynamicstaticValue { get; set; }
        //public string paramKey { get; set; }
        //public string selectedType { get; set; }

        [JsonPropertyName("ParameterName")]
        public string? ParameterName { get; set; }

        [JsonPropertyName("Type")]
        public string? DataType { get; set; }

        [JsonPropertyName("Value")]
        public string? AttributeValueType { get; set; }
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
