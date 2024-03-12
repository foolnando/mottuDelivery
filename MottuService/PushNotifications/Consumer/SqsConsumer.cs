using Newtonsoft.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MottuService.DataBase;
using MottuService.Notifications;

public class SqsConsumer: BackgroundService {
    private readonly IAmazonSQS _sqs;
    private readonly List<string> _messageAttributeNames = new(){"*"};
    private readonly IServiceProvider _serviceProvider;


    public SqsConsumer(IAmazonSQS sqs,  IServiceProvider serviceProvider) {
        var awsAccessKeyId = "AKIA2UC3E3CMH5XWUUU2";
        var awsSecretAccessKey = "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";
        var sqsConfig = new AmazonSQSConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.USEast1 
        };
        _sqs = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, sqsConfig);
        _serviceProvider = serviceProvider;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = "https://sqs.us-east-1.amazonaws.com/730335598744/mottu-push-notification-queue";     
        var receiveRequest = new ReceiveMessageRequest{
            QueueUrl = queueUrl,
            MessageAttributeNames = _messageAttributeNames,
            AttributeNames = _messageAttributeNames
        }   ;
      while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MottuDataBaseContext>();

                var messageResponse = await _sqs.ReceiveMessageAsync(receiveRequest, stoppingToken);

                foreach(var message in messageResponse.Messages){
                    
                    NotificationMessage notificationRequest = JsonConvert.DeserializeObject<NotificationMessage>(message.Body);


      
                    var notification =  new OrderNotification(notificationRequest.DriverId, notificationRequest.OrderId);
                    await dbContext.Notifications.AddAsync(notification);
                    await dbContext.SaveChangesAsync(); 

                    await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
                }
            }
        }

    }
}