using System;

namespace Page_Navigation_App.Model
{
    public class PageModel
    {
        public int CustomerCount
        {
            get; set;
        }

        public string ExpectedKeyName
        {
            get; set;
        }

        public bool IsUserControlFocused
        {
            get;
            set;
        }

        public bool LocationStatus
        {
            get; set;
        }

        public string MessageText
        {
            get; set;
        }

        public DateOnly OrderDate
        {
            get; set;
        }

        public string ProductStatus
        {
            get; set;
        }

        public string RecevedKeyName

        {
            get; set;
        }

        public TimeOnly ShipmentDelivery
        {
            get; set;
        }

        public decimal TransactionValue
        {
            get; set;
        }

        public bool RightHandIsSet
        {
            get;
            set;
        }

        // This to save the type
        public Type ViewModelType
        {
            get; set;
        }
    }
}