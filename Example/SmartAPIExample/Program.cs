using System;
using System.Collections.Generic;
using System.Configuration;
using SmartAPIAO;
using Newtonsoft.Json;

namespace AngelBrokingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            //Initialize Samart API using clientcode and password.

            string Client_Code = ConfigurationManager.AppSettings[Common.ClientID];  //YOUR CLIENT CODE
            string Password = ConfigurationManager.AppSettings[Common.Secret]; //YOUR MPIN
            string Api_Key = ConfigurationManager.AppSettings[Common.Api_Key];
            string JWTToken = "";  // optional
            string RefreshToken = ""; // optional

            SmartApi connection = new SmartApi(Api_Key, JWTToken, RefreshToken);

            OutputBaseClass output = new OutputBaseClass();

            //Login by client code,mpin and totp(authenticator app otp)
            Console.WriteLine("Please enter Totp received on Authenticator App");
            string otp = Console.ReadLine();
            output = connection.GenerateSession(Client_Code, Password,otp);
            //Get Token
            AngelToken token = output.TokenResponse; 

            //Get historical data for a instrument
            var result = connection.GetCandleData(new CandleRequest
            {
                exchange = "NSE",
                symboltoken = "99926000",
                interval = "THIRTY_MINUTE",
                fromdate = "2024-01-12 09:15",
                todate = "2024-01-12 15:15"
            });
           
            
            //Get Profile
            output = connection.GetProfile();
            GetProfileResponse gp = output.GetProfileResponse;

            Console.WriteLine("------GetProfile call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(gp));
            Console.WriteLine("----------------------------------------------");

            //Place Order
            OrderInfo ord = new OrderInfo();
            ord.variety = Constants.VARIETY_NORMAL;
            ord.tradingsymbol = "SBIN-EQ";
            ord.symboltoken = "3045";
            ord.transactiontype = Constants.TRANSACTION_TYPE_BUY;
            ord.exchange = Constants.EXCHANGE_NSE;
            ord.ordertype = Constants.ORDER_TYPE_LIMIT;
            ord.producttype = Constants.PRODUCT_TYPE_INTRADAY;
            ord.duration = Constants.VALIDITY_DAY.ToString();
            ord.price = "350.00";
            ord.squareoff = "0";
            ord.stoploss = "0";
            ord.quantity = "10";
            ord.triggerprice = "0";  //OPTIONAL PARAMETER
            //ord.triggerprice = "350";  //OPTIONAL PARAMETER            

            output = connection.placeOrder(ord);
            OrderResponse Ores = output.PlaceOrderResponse;

            Console.WriteLine("------placeOrder call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(Ores));
            Console.WriteLine("----------------------------------------------");

            //Modify Order
            OrderInfo mord = new OrderInfo();
            mord.orderid = "211029001245777";
            mord.variety = Constants.VARIETY_NORMAL;
            mord.tradingsymbol = "SBIN-EQ";
            mord.symboltoken = "3045";
            mord.transactiontype = Constants.TRANSACTION_TYPE_BUY;
            mord.exchange = Constants.EXCHANGE_NSE;
            mord.ordertype = Constants.ORDER_TYPE_LIMIT;
            mord.producttype = Constants.PRODUCT_TYPE_DELIVERY;
            mord.duration = Constants.VALIDITY_DAY.ToString();
            mord.price = "340.00";
            mord.squareoff = "0";
            mord.stoploss = "0";
            mord.quantity = "20";
            mord.triggerprice = "0";  //OPTIONAL PARAMETER
                                      //mord.triggerprice = "357";  //OPTIONAL PARAMETER

            output = connection.modifyOrder(mord);
            OrderResponse mOres = output.ModifyOrderResponse;

            Console.WriteLine("------modifyOrder call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(mOres));
            Console.WriteLine("----------------------------------------------");

            //cancel Order
            OrderInfo ord2 = new OrderInfo();
            ord2.orderid = "211029001245777";
            ord2.variety = Constants.VARIETY_NORMAL;

            output = connection.cancelOrder(ord2);
            OrderResponse cOrs = output.CancelOrderResponse;

            Console.WriteLine("------cancelOrder call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(cOrs));
            Console.WriteLine("----------------------------------------------");


            //get Order Book
            output = connection.getOrderBook();
            GetOrderBookResponse book = output.GetOrderBookResponse;

            Console.WriteLine("------getOrderBook call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(book));
            Console.WriteLine("----------------------------------------------");

            //get Trade Book
            output = connection.getTradeBook();
            GetTradeBookResponse trade = output.GetTradeBookResponse;

            Console.WriteLine("------getTradeBook call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(trade));
            Console.WriteLine("----------------------------------------------");

            // Get Get LTP Data 
            LTPDataRequest lreq = new LTPDataRequest();
            lreq.exchange = Constants.EXCHANGE_NSE;
            lreq.symboltoken = "3045";
            lreq.tradingsymbol = "SBIN-EQ";
            output = connection.GetLTPData(lreq);
            GetLTPDataResponse ltp = output.GetLTPDataResponse;

            Console.WriteLine("------GetLTPData call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(ltp));
            Console.WriteLine("----------------------------------------------");

            //get Holding
            output = connection.getHolding();
            GetHoldingResponse holding = output.GetHoldingResponse;

            Console.WriteLine("------getHolding call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(holding));
            Console.WriteLine("----------------------------------------------");

            //get Position
            output = connection.getPosition();
            GetPositionResponse position = output.GetPositionResponse;

            Console.WriteLine("------getPosition call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(position));
            Console.WriteLine("----------------------------------------------");

            //Convert Position
            ConvertPositionRequest req = new ConvertPositionRequest();
            req.exchange = Constants.EXCHANGE_NSE.ToString();
            req.oldproducttype = Constants.PRODUCT_TYPE_DELIVERY;
            req.newproducttype = Constants.PRODUCT_TYPE_MARGIN;
            req.tradingsymbol = "SBIN-EQ";
            req.transactiontype = Constants.TRANSACTION_TYPE_BUY;
            req.quantity = 1;
            req.type = Constants.VALIDITY_DAY;

            output = connection.convertPosition(req);
            PositionConversionResponse cc = output.PositionConversionResponse;

            Console.WriteLine("------convertPosition call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(cc));
            Console.WriteLine("----------------------------------------------");

            //get RMS
            output = connection.getRMS();
            GetRMSLimitResponse gmres = output.GetRMSLimitResponse;

            Console.WriteLine("------getRMS call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(gmres));
            Console.WriteLine("----------------------------------------------");

            //Create Rule
            CreateRuleRequest crreq = new CreateRuleRequest();
            //crreq.id = 0;
            crreq.tradingsymbol = "SBIN-EQ";
            crreq.symboltoken = "3045";
            crreq.exchange = Constants.EXCHANGE_NSE;
            crreq.transactiontype = Constants.TRANSACTION_TYPE_BUY;
            crreq.producttype = Constants.PRODUCT_TYPE_MARGIN;
            crreq.price = "350";
            crreq.qty = "10";
            crreq.triggerprice = "370";
            crreq.disclosedqty = "10";
            crreq.timeperiod = "20";

            output = connection.CreateRule(crreq);
            RuleResponse rr = output.CreateRuleResponse;

            Console.WriteLine("------CreateRule call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(rr));
            Console.WriteLine("----------------------------------------------");

            //Rule Details
            string RuleID = "1000118";
            output = connection.ruleDetails(RuleID);
            RuleDetailsResponse rd = output.RuleDetailsResponse;

            Console.WriteLine("------ruleDetails call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(rd));
            Console.WriteLine("----------------------------------------------");

            //Modify Rule
            CreateRuleRequest crreq2 = new CreateRuleRequest();
            crreq2.id = "1000118";
            crreq2.tradingsymbol = "SBIN-EQ";
            crreq2.symboltoken = "3045";
            crreq2.exchange = Constants.EXCHANGE_NSE;
            crreq2.transactiontype = Constants.TRANSACTION_TYPE_BUY;
            crreq2.producttype = Constants.PRODUCT_TYPE_MARGIN;
            crreq2.price = "350";
            crreq2.qty = "10";
            crreq2.triggerprice = "360";
            crreq2.disclosedqty = "10";
            crreq2.timeperiod = "20";

            output = connection.modifyRule(crreq2);
            RuleResponse rm = output.ModifyRuleResponse;

            Console.WriteLine("------modifyRule call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(rm));
            Console.WriteLine("----------------------------------------------");

            //Cancel Rule
            CancelRuleRequest creq = new CancelRuleRequest();
            creq.id = "1000117";
            creq.symboltoken = "3045";
            creq.exchange = Constants.EXCHANGE_NSE;

            output = connection.cancelRule(creq);
            RuleResponse rc = output.CancelRuleResponse;

            Console.WriteLine("------cancelRule call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(rc));
            Console.WriteLine("----------------------------------------------");

            //Rule List
            RuleListRequest rreq = new RuleListRequest();
            rreq.status = new List<string>();
            rreq.status.Add("NEW");
            rreq.status.Add("CANCELLED");
            rreq.page = 1;
            rreq.count = 10;

            output = connection.ruleList(rreq);
            RuleListResponse rl = output.RuleListResponse;

            Console.WriteLine("------ruleList call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(rl));
            Console.WriteLine("----------------------------------------------");

            //Get Candle Data
            CandleRequest cdreq = new CandleRequest();
            cdreq.exchange = Constants.EXCHANGE_NSE;
            cdreq.symboltoken = "3045";
            cdreq.interval = Constants.INTERVAL_MINUTE;
            cdreq.fromdate = "2021-02-08 09:00";
            cdreq.todate = "2021-02-08 09:15";

            output = connection.GetCandleData(cdreq);
            CandleDataResponse cd = output.GetCandleDataResponse;

            Console.WriteLine("------GetCandleData call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(cd));
            Console.WriteLine("----------------------------------------------");

            //LogOut
            output = connection.LogOut(Client_Code);
            LogOutResponse lg = output.LogOutResponse;

            Console.WriteLine("------LogOut call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(lg));
            Console.WriteLine("----------------------------------------------");

        }      
    }
}
