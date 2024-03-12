using System;
using System.Text.Json.Serialization;
using Amazon.SQS.Model;

namespace MottuService.Notifications
{
    public class NotificationMessage : IMessage
    {
        

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("driverId")]
        public Guid DriverId { get; set; }

        public string MessageTypeName => nameof(NotificationMessage);

        public static explicit operator NotificationMessage(Message v)
        {
            throw new NotImplementedException();
        }
    }
}
