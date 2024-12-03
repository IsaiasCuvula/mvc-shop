using Microsoft.Extensions.Options;
using ShopFullStack.Models;
using ShopFullStack.Utilities;
using Stripe;

namespace ShopFullStack.Services;

public class StripePaymentService
{
    
    public StripePaymentService(IOptions<StripeApiKeys> apiKeys)
    {
        StripeConfiguration.ApiKey = apiKeys.Value.SecretKey; 
    }
      
     public string MakePayment(
         string email,
         string successUrl,
         string cancelUrl,
         Order order
     )
     {
         try
         {
             var customerService = new Stripe.CustomerService();
             var customerOptions = new CustomerCreateOptions
             {
                 Email = email,
             };
             var customer = customerService.Create(customerOptions);

             var sessionService = new Stripe.Checkout.SessionService();
             var sessionOptions = new Stripe.Checkout.SessionCreateOptions
             {
                 PaymentMethodTypes = new List<string> { "card" },
                 LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                 {
                     new Stripe.Checkout.SessionLineItemOptions
                     {
                         PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                         {
                             UnitAmount = (long)(order.Total * 100), 
                             Currency = "usd",
                             ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                             {
                                 Name = $"Order #{order.Id}",
                                 Metadata = new Dictionary<string, string>
                                 {
                                     { "OrderId", order.Id.ToString() },
                                     { "CustomerId", order.CustomerId.ToString() },
                                     { "ShippingAddress", order.ShippingAddress },
                                     { "CreatedAt", order.CreatedAt.ToString("o") },
                                     { "Status", order.Status.ToString() },
                                     { "PaymentStatus", PaymentStatus.Paid.ToString() }
                                 }
                             },
                         },
                         Quantity = 1,
                     },
                 },
                 Mode = "payment",
                 SuccessUrl = successUrl,
                 CancelUrl = cancelUrl
             };
             var session = sessionService.Create(sessionOptions);
             return session.Url;
         }
         catch (StripeException ex)
         {
             Console.WriteLine($"Failed to make payment: {ex.Message}");
             return string.Empty;
         }
     }
}
