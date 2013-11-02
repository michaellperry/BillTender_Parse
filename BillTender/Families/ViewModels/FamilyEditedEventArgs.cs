using System;
using BillTender.Families.Models;

namespace BillTender.Families.ViewModels
{
    public class FamilyEditedEventArgs
    {
        public bool NewFamily { get; set; }
        public Family Family { get; set; }
        public Action Completed { get; set; }
        public Action Cancelled { get; set; }
    }
}
