using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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
        private const string TABLE_NAME = "drugs";
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public DrugProvider(IAmazonDynamoDB dynamoDB)
        {
            _amazonDynamoDB = dynamoDB;
        }

        public async Task<Drug[]> GetDrugsAsync()
        {
            var result = await _amazonDynamoDB.ScanAsync(new ScanRequest
            {
                TableName = TABLE_NAME
            });

            if (result != null && result.Items != null)
            {
                var drugs = new List<Drug>();

                foreach (var item in result.Items)
                {
                    item.TryGetValue("drugName", out var name);
                    item.TryGetValue("cyps", out var cyps);

                    drugs.Add(new Drug
                    {
                        DrugName = name?.S,
                        Cyps = cyps?.SS
                    });
                }

                return drugs.ToArray();
            }

            return Array.Empty<Drug>();
        }

        public async Task<List<string>> DrugsInteractAsync(string drug1, string drug2)
        {
            var cypsDrug1 = await GetCypsForDrug(drug1);
            var cypsDrug2 = await GetCypsForDrug(drug2);

            var commonCYPs = cypsDrug1.Intersect(cypsDrug2).ToList();
            return commonCYPs;
        }

        private async Task<List<string>> GetCypsForDrug(string drugName)
        {
            var table = Table.LoadTable(_amazonDynamoDB, TABLE_NAME);
            var document = await table.GetItemAsync(drugName);

            if (document != null)
            {
                return document["cyps"].AsListOfString();
            }

            return new List<string>();
        }

    }
}
