using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BillTender.Messaging
{
    [XmlInclude(typeof(BillTender.Budget.Messages.CreateBill))]
    [XmlInclude(typeof(BillTender.Budget.Messages.UpdateBill))]
    public abstract class Message
    {
    }
}
