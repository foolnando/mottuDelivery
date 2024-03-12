public record ICreateDeliveryOrderRequest(double value);
public record IConfirmOrderRequest(Guid orderId, Guid rentId);

