using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;
using System.Security.Cryptography.X509Certificates;

namespace StockMarketSolution.Filters.ActionFilters
{
    public class RedirectActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before logic
            if (context.Controller is TradeController tradeController)
            {
                if (!tradeController.ModelState.IsValid)
                {
                    var order = context.ActionArguments.ContainsKey("order") ? context.ActionArguments["order"] : null;
                    StockTrade stockTrade = new StockTrade();

                    if (order is BuyOrder buyOrder)
                    {
                        stockTrade.StockSymbol = buyOrder.StockSymbol;
                        stockTrade.StockName = buyOrder.StockName;
                        stockTrade.Price = buyOrder.Price;
                        stockTrade.Quantity = buyOrder.Quantity;
                    }
                    else if (order is SellOrder sellOrder)
                    {
                        stockTrade.StockSymbol = sellOrder.StockSymbol;
                        stockTrade.StockName = sellOrder.StockName;
                        stockTrade.Price = sellOrder.Price;
                        stockTrade.Quantity = sellOrder.Quantity;
                    }

                    tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();


                    context.Result = tradeController.View("Index", stockTrade);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            //After logic
        }
    }
}
