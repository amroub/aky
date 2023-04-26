namespace aky.Foundation.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;

    public class SubscriptionMessage<T> : SubscriptionMessage
    {
        public SubscriptionMessage(Message message)
            : base(message)
        {
        }

        public T GetBody()
        {
            string json = Encoding.UTF8.GetString(this.EventMessage.Body);

            if (typeof(T).IsInterface)
            {
                // Dynamic proxy
                return JsonConvert.DeserializeObject<T>(json);
            }

            return JsonConvert.DeserializeObject<T>(json, new GuidelineJsonSerializerSettings());
        }
    }

    public class SubscriptionMessage
    {
        public SubscriptionMessage(Message message)
        {
            this.EventMessage = message;
        }

        public string ContentType
        {
            get
            {
                return this.EventMessage.ContentType;
            }
        }

        public string CorrelationId
        {
            get
            {
                return this.EventMessage.CorrelationId;
            }
        }

        public DateTime ExpiresAtUtc
        {
            get
            {
                return this.EventMessage.ExpiresAtUtc;
            }
        }

        public string Label
        {
            get
            {
                return this.EventMessage.Label;
            }
        }

        public string MessageId
        {
            get
            {
                return this.EventMessage.MessageId;
            }

            set
            {
                this.EventMessage.MessageId = value;
            }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                return this.EventMessage.UserProperties;
            }
        }

        public string ReplyTo
        {
            get
            {
                return this.EventMessage.ReplyTo;
            }
        }

        public string ReplyToSessionId
        {
            get
            {
                return this.EventMessage.ReplyToSessionId;
            }
        }

        public DateTime ScheduledEnqueueTimeUtc
        {
            get
            {
                return this.EventMessage.ScheduledEnqueueTimeUtc;
            }
        }

        public string SessionId
        {
            get
            {
                return this.EventMessage.SessionId;
            }
        }

        public long Size
        {
            get
            {
                return this.EventMessage.Size;
            }
        }

        public TimeSpan TimeToLive
        {
            get
            {
                return this.EventMessage.TimeToLive;
            }
        }

        public string To
        {
            get
            {
                return this.EventMessage.To;
            }
        }

        public bool IsDeadLettered
        {
            get;
            private set;
        }

        public bool IsAbandoned { get; private set; }

        public bool IsComplete { get; private set; }

        public bool IsSuspended { get; private set; }

        public bool IsActioned
        {
            get { return this.IsAbandoned || this.IsComplete || this.IsDeadLettered || this.IsSuspended; }
        }

        protected Message EventMessage { get; }

        public void Suspend()
        {
            this.IsSuspended = true;
        }

        public string GetBodyString()
        {
            return Encoding.UTF8.GetString(this.EventMessage.Body);
        }

        public string GetHeadersString()
        {
            return JsonConvert.SerializeObject(this.Properties, new GuidelineJsonSerializerSettings());
        }

        public Message GetMessage()
        {
            return this.EventMessage;
        }
    }
}
