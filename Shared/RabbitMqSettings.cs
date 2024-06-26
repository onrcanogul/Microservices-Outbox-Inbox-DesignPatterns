using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class RabbitMqSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
        public const string Stock_InboxToStockEventQueue = "stock-inbox-to-stock-event-queue";
        public const string Inbox_StockProcessedEventQueue = "inbox-stock-processed-event-queue";
    }
}
