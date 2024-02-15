namespace SecOpsApi.Options;

public class SwaggerOptions
{
    public const string SectionName = "Swagger";

    public required string AuthorizationUrl { get; set; }
    public required string TokenUrl { get; set; }
    public required string ClientId { get; set; }
    public required string[] Scopes { get; set; }
}