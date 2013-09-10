using System;
using BillTender.Budget.Models;

namespace BillTender.Budget.ViewModels
{
    public class BillEditedEventArgs
    {
        public Bill Bill { get; set; }
        public Action Completed { get; set; }
        public Action Cancelled { get; set; }
    }
}
