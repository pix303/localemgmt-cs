using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;


namespace EventSourcingStore;


public class DynamoEventStoreMigration
{

    private readonly string PK = "aggregateId";
    private readonly string SK = "createdAt";

    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public DynamoEventStoreMigration(IAmazonDynamoDB client, string tableName)
    {
        _client = client;
        _tableName = tableName;
    }

    public async Task<bool> TableExistsAsync()
    {
        try
        {
            var response = await _client.DescribeTableAsync(_tableName);
            return response.Table.TableStatus == TableStatus.ACTIVE;
        }
        catch (ResourceNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> CreateTableAsync()
    {
        if (await TableExistsAsync())
        {
            Console.WriteLine($"La tabella {_tableName} esiste già.");
            return false;
        }

        var request = new CreateTableRequest
        {
            TableName = _tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = PK,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = SK,
                    AttributeType = ScalarAttributeType.S
                }
            },

            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = PK,
                    KeyType = KeyType.HASH
                },
                new KeySchemaElement
                {
                    AttributeName = SK,
                    KeyType = KeyType.RANGE
                }
            },

            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 2,
                WriteCapacityUnits = 2
            },
        };

        try
        {
            var response = await _client.CreateTableAsync(request);
            Console.WriteLine($"Tabella {_tableName} creata con successo. Stato: {response.TableDescription.TableStatus}");

            await WaitForTableToBeActiveAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la creazione della tabella: {ex.Message}");
            return false;
        }
    }

    private async Task WaitForTableToBeActiveAsync()
    {
        bool tableIsActive = false;
        while (!tableIsActive)
        {
            var response = await _client.DescribeTableAsync(_tableName);
            tableIsActive = response.Table.TableStatus == TableStatus.ACTIVE;

            if (!tableIsActive)
            {
                Console.WriteLine($"Attendo che la tabella sia attiva... Stato attuale: {response.Table.TableStatus}");
                await Task.Delay(2000);
            }
        }
    }
}
