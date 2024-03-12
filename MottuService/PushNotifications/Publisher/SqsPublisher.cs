using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

public class SqsPublisher {
    private readonly IAmazonSQS _sqs;

    public SqsPublisher(IAmazonSQS sqs){
        _sqs = sqs;
    }

    public async Task PublishAync<T>(string queueName, T message){
        var queueUrl = "https://sqs.us-east-1.amazonaws.com/730335598744/mottu-push-notification-queue";        
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };
        Console.WriteLine($"Sending request {request}");
        await _sqs.SendMessageAsync(request);

    }
}