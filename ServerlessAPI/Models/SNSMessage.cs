namespace ServerlessAPI.Models
{
    public class SNSMessage
    {
        public string Type { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
        
        public string TopicArn { get; set; }
        
        public string SubscribeURL { get; set; }
    }
}
