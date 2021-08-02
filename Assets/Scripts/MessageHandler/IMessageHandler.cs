using System;

public interface IMessageHandler{
    void SubscribeMessage<TMessage>(Action<TMessage> callback);
    void UnsubscribeMessage<TMessage>(Action<TMessage> callback);
    void SendMessage<TMessage>(TMessage message);
}