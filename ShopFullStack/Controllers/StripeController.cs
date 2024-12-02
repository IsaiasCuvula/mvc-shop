using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopFullStack.Models;
using Stripe;

namespace ShopFullStack.Controllers;

public class StripeController: Controller
{

    private readonly StripeApiKeys _apiKeys;
    private readonly UserManager<IdentityUser> _userManager;
    

    public StripeController(IOptions<StripeApiKeys> apiKeys, UserManager<IdentityUser> userManager)
    {
        _apiKeys = apiKeys.Value;
        _userManager = userManager;
    }


    public IActionResult Pay()
    {
        // Retrieve the user from identity framework
        var user = _userManager.GetUserAsync(User).Result;
        var email = user == null ? HttpContext.Session.GetString("Email") : user.Email;

        // Set Stripe API key
        StripeConfiguration.ApiKey = _apiKeys.SecretKey;

        // Create a customer
        var customerService = new Stripe.CustomerService();
        var customerOptions = new CustomerCreateOptions
        {
            Email = email,
        };
        var customer = customerService.Create(customerOptions);

        // Define session options
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
                        UnitAmount = 30000, // Amount in cents (e.g., $300.00 = 30000 cents)
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Your Product Name",
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = Url.Action("OrdersPage", "Order", null, Request.Scheme), // Replace 'OrderPage' and 'Order' with actual action and controller names
            CancelUrl = Request.Headers["Referer"].ToString() ?? Url.Action("CartPage", "Cart", null, Request.Scheme), // Referer falls back to Cart
        };

        // Create the session
        var session = sessionService.Create(sessionOptions);
        
        // Return the session URL
        return Redirect(session.Url);
    }
}