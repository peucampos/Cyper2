using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Cyper2;

public class CyperFunction
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> CyperFunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        string name = "none";

        if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("name"))
        {
            name = request.QueryStringParameters["name"];
        }

        if (request.HttpMethod == "POST")
        {
            // POST Method
        }

        var drugProvider = new DrugProvider(new AmazonDynamoDBClient());
        var drugs = await drugProvider.GetDrugsAsync();

        context.Logger.Log($"Got name {name}");
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = JsonConvert.SerializeObject(drugs)
        };
    }
}
