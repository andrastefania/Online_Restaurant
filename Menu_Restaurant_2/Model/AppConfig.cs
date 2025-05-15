namespace Menu_Restaurant_2.Model
{
    public class AppConfig
    {
        public DiscountConfig DiscountAboveTotal { get; set; }
        public LoyaltyDiscountConfig LoyaltyDiscount { get; set; }
        public DeliveryFeeConfig DeliveryFee { get; set; }
        public int StockAlertThreshold { get; set; }
        public MenuDiscountConfig MenuDiscount { get; set; } 
    }

    public class DiscountConfig
    {
        public double ThresholdAmount { get; set; }
        public double DiscountPercent { get; set; }
    }

    public class LoyaltyDiscountConfig
    {
        public int RequiredOrders { get; set; }
        public int TimeIntervalDays { get; set; }
        public double DiscountPercent { get; set; }
    }

    public class DeliveryFeeConfig
    {
        public double FreeAbove { get; set; }
        public double FeeBelow { get; set; }
    }

    public class MenuDiscountConfig  // 🆕 Noua clasă pentru secțiunea adăugată în JSON
    {
        public double DiscountPercent { get; set; }
    }
}
