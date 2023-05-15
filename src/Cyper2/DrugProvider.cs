using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyper2
{
    public class DrugProvider : IDrugProvider
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public DrugProvider(IAmazonDynamoDB dynamoDB)
        {
            _amazonDynamoDB = dynamoDB;
        }

        public async Task<Drug[]> GetDrugsAsync()
        {
            var result = await _amazonDynamoDB.ScanAsync(new ScanRequest
            {
                TableName = "drugs"
            });

            if (result != null && result.Items != null)
            {
                var drugs = new List<Drug>();

                foreach (var item in result.Items)
                {
                    item.TryGetValue("name", out var name);
                    item.TryGetValue("cyps", out var cyps);

                    drugs.Add(new Drug
                    {
                        Name = name?.S,
                        Cyps = cyps?.SS
                    });
                }

                return drugs.ToArray();
            }

            return Array.Empty<Drug>();
        }
    }
}
