using System;
using System.Collections.Generic;
using Turnkey.code;
using Turnkey.config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Turnkey.Tests.Models
{
    [TestClass]
    public class VoidTest
    {
        [TestMethod]
        public void AuthToVoidExTestCall()
        {
            /*Init appliction configuration*/
            ApplicationConfig config = ObjectFactory.config;

            // TOKENIZE
            Dictionary<String, String> tokenizeParams = new Dictionary<string, string>();
            tokenizeParams.Add("number", "5454545454545454");
            tokenizeParams.Add("nameOnCard", "test");
            tokenizeParams.Add("expiryYear", "2023");
            tokenizeParams.Add("expiryMonth", "04");

            TokenizeCall tokenizeCall = new TokenizeCall(config, tokenizeParams);
            Dictionary<String, String> tokenizeResult = tokenizeCall.Execute();

            Dictionary<String, String> authParams = new Dictionary<String, String>();
            authParams.Add("amount", "20.0");
            authParams.Add("channel", Channel.ECOM.GetCode());
            authParams.Add("currency", CurrencyCode.EUR.GetCode());
            authParams.Add("paymentSolutionId", "500");
            authParams.Add("customerId", tokenizeResult["customerId"]);
            authParams.Add("specinCreditCardToken", tokenizeResult["cardToken"]);
            authParams.Add("specinCreditCardCVV", "111");
            authParams.Add("merchantNotificationUrl", "http://localhost:8080/api/TransactionResultCallback");

            AuthCall authCall = new AuthCall(config, authParams);
            Dictionary<String, String> authResult = authCall.Execute();

            Assert.AreEqual(authResult["result"], "success");

            if (authResult["result"] == "success" && authResult["status"] == "NOT_SET_FOR_CAPTURE")
            {
                // VOID
                Dictionary<String, String> inputParams = new Dictionary<String, String>();
                inputParams.Add("originalMerchantTxId", authResult["merchantTxId"]);

                VoidCall call = new VoidCall(config, inputParams);
                Dictionary<String, String> result = call.Execute();

                Assert.AreEqual(result["result"], "success");
            }

        }

        [TestMethod]
        public void CaptureToVoidExTestCall()
        {
            /*Init appliction configuration*/
            ApplicationConfig config = ObjectFactory.config;

            // TOKENIZE
            Dictionary<String, String> tokenizeParams = new Dictionary<string, string>();
            tokenizeParams.Add("number", "5454545454545454");
            tokenizeParams.Add("nameOnCard", "test");
            tokenizeParams.Add("expiryYear", "2023");
            tokenizeParams.Add("expiryMonth", "04");

            TokenizeCall tokenizeCall = new TokenizeCall(config, tokenizeParams);
            Dictionary<String, String> tokenizeResult = tokenizeCall.Execute();

            Dictionary<String, String> authParams = new Dictionary<String, String>();
            authParams.Add("amount", "20.0");
            authParams.Add("channel", Channel.ECOM.GetCode());
            authParams.Add("currency", CurrencyCode.EUR.GetCode());
            authParams.Add("paymentSolutionId", "500");
            authParams.Add("customerId", tokenizeResult["customerId"]);
            authParams.Add("specinCreditCardToken", tokenizeResult["cardToken"]);
            authParams.Add("specinCreditCardCVV", "111");
            authParams.Add("merchantNotificationUrl", "http://localhost:8080/api/TransactionResultCallback");

            AuthCall authCall = new AuthCall(config, authParams);
            Dictionary<String, String> authResult = authCall.Execute();

            Assert.AreEqual(authResult["result"], "success");

            if (authResult["result"] == "success" && authResult["status"] == "NOT_SET_FOR_CAPTURE")
            {
                // CAPTURE
                Dictionary<String, String> captrueInputParams = new Dictionary<String, String>();
                captrueInputParams.Add("originalMerchantTxId", authResult["merchantTxId"]);
                captrueInputParams.Add("amount", "20.0");

                CaptureCall captureCall = new CaptureCall(config, captrueInputParams);
                Dictionary<String, String> captureResult = captureCall.Execute();
                if (captureResult["result"] == "success")
                {
                    // VOID
                    Dictionary<String, String> inputParams = new Dictionary<String, String>();
                    inputParams.Add("originalMerchantTxId", authResult["merchantTxId"]);

                    VoidCall call = new VoidCall(config, inputParams);
                    Dictionary<String, String> result = call.Execute();

                    Assert.AreEqual(result["result"], "success");
                }

            }

        }

        [TestMethod]
        public void PurchaseToVoidTestCall()
        {
            /*Init appliction configuration*/
            ApplicationConfig config = ObjectFactory.config;

            // TOKENIZE
            Dictionary<String, String> tokenizeParams = new Dictionary<string, string>();
            tokenizeParams.Add("number", "5454545454545454");
            tokenizeParams.Add("nameOnCard", "test");
            tokenizeParams.Add("expiryYear", "2023");
            tokenizeParams.Add("expiryMonth", "04");

            TokenizeCall tokenizeCall = new TokenizeCall(config, tokenizeParams);
            Dictionary<String, String> tokenizeResult = tokenizeCall.Execute();

            Dictionary<String, String> authParams = new Dictionary<String, String>();
            authParams.Add("amount", "20.0");
            authParams.Add("channel", Channel.ECOM.GetCode());
            authParams.Add("currency", CurrencyCode.EUR.GetCode());
            authParams.Add("paymentSolutionId", "500");
            authParams.Add("customerId", tokenizeResult["customerId"]);
            authParams.Add("specinCreditCardToken", tokenizeResult["cardToken"]);
            authParams.Add("specinCreditCardCVV", "111");
            authParams.Add("merchantNotificationUrl", "http://localhost:8080/api/TransactionResultCallback");

            PurchaseCall authCall = new PurchaseCall(config, authParams);
            Dictionary<String, String> purchaseResult = authCall.Execute();

            Assert.AreEqual(purchaseResult["result"], "success");

            if (purchaseResult["result"] == "success")
            {
                // VOID
                Dictionary<String, String> inputParams = new Dictionary<String, String>();
                inputParams.Add("originalMerchantTxId", purchaseResult["merchantTxId"]);

                VoidCall call = new VoidCall(config, inputParams);
                Dictionary<String, String> result = call.Execute();

                Assert.AreEqual(result["result"], "failure");
            }

        }
    }
}
