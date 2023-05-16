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
        string d1 = "";
        string d2 = "";

        if (request.QueryStringParameters != null)
        {
            if (request.QueryStringParameters.ContainsKey("d1") && request.QueryStringParameters.ContainsKey("d2"))
            {
                d1 = request.QueryStringParameters["d1"];
                d2 = request.QueryStringParameters["d2"];

                var drugProvider = new DrugProvider(new AmazonDynamoDBClient());
                var drugs = await drugProvider.DrugsInteractAsync(d1, d2);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(drugs)
                };
            }
            else if (request.QueryStringParameters.ContainsKey("d1"))
            {
                d1 = request.QueryStringParameters["d1"];
                var drugProvider = new DrugProvider(new AmazonDynamoDBClient());
                var drugs = await drugProvider.GetCypsForDrug(d1);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(drugs)
                };
            }            
        }
        else
        {
            var drugProvider = new DrugProvider(new AmazonDynamoDBClient());
            var drugs = await drugProvider.GetDrugsAsync();

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(drugs)
            };
        }

        return new APIGatewayProxyResponse
        {
            StatusCode = 400,
            Body = "Invalid query string."
        };
    }
}
