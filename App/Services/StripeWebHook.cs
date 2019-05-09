using System;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;
using Stripe;
using GMaster.Common.Stripe;

namespace GMaster.Services
{
    public class StripeWebHook : Service
    {
        public StripeWebHook(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public string Endpoint(){
            var logId = 0;
            try
            {
                //build Stripe Event object from request body and verify Stripe signing secret
                var ev = EventUtility.ConstructEvent(requestBody, context.Request.Headers["Stripe-Signature"], Settings.Stripe.Webhook.SigningSecret);

                //log Stripe Event
                logId = Query.LogStripeWebhooks.Create(ev.Type, Serializer.WriteObjectToString(ev));

                //determine what to do with event
                switch (ev.Type)
                {
                    case "invoice.finalized":
                        Invoices.InvoiceFinalized(ev.Data.Object as Invoice);
                        break;

                    case "invoice.payment_succeeded":
                        Payments.PaymentSucceeded(ev.Data.Object as Invoice);
                        break;

                    case "invoice.payment_failed":
                        Payments.PaymentFailed(ev.Data.Object as Invoice);
                        break;

                    case "invoice.payment_action_required":
                        Payments.PaymentActionRequired(ev.Data.Object as Invoice);
                        break;
                    default:
                        return BadRequest();
                }

                return "";
            }
            catch (Exception ex)
            {
                //log webhook error
                Query.LogErrors.Create(0, "StripeWebhook" + (logId > 0 ? " (" + logId + ")" : ""), "", ex.Message, ex.StackTrace);
                return BadRequest("Bad Request to Stripe Webhook");
            }
        }
    }
}
