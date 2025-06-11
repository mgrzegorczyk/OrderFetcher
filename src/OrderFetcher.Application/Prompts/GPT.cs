namespace OrderFetcher.Application.Prompts;

public static class GPT
{
    public const string OrderExtractor = @"
You are an assistant that extracts order information from an email body in HTML format.
Extract the order data and return it as JSON with the following structure:

{{
  ""OrderNumber"": ""string"",
  ""OrderDate"": ""string (ISO 8601)"",
  ""Amount"": 0.0,
  ""TotalAmount"": 0.0,
  ""Currency"": ""string"",
  ""ShippingMethod"": ""string"",
  ""PaymentMethod"": ""string"",
  ""Items"": [
    {{
      ""ProductName"": ""string"",
      ""Quantity"": 0,
      ""Price"": 0.0
    }}
  ],
  ""BillingAddress"": {{
    ""FullName"": ""string"",
    ""Street"": ""string"",
    ""BuildingNumber"": ""string"",
    ""PostalCode"": ""string"",
    ""City"": ""string"",
    ""PhoneNumber"": ""string"",
    ""Email"": ""string""
  }},
  ""ShippingAddress"": {{
    ""FullName"": ""string"",
    ""Street"": ""string"",
    ""BuildingNumber"": ""string"",
    ""PostalCode"": ""string"",
    ""City"": ""string"",
    ""PhoneNumber"": ""string"",
    ""Email"": ""string""
  }}
}}

If the shipping address is not present in the email, set \""ShippingAddress\"" to null.

HTML email body:
{0}

Return ONLY the JSON object, no markdown, no code block, no explanations.
";
}