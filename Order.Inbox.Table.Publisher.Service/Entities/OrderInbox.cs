using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Inbox.Table.Publisher.Service.Entities
{
    public class OrderInbox
    {
        public Guid Id { get; set; }
        public bool Processed { get; set; }
        public string Payload { get; set; }
    }
}
