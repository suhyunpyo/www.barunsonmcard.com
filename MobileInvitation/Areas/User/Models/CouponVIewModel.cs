using System;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 쿠폰 정보
    /// </summary>
    public class UseCouponInfo
    {
        public string CouponType { get; set; }
        public int CouponID { get; set; }
        public string CouponName { get; set; }
        public int CouponPublishID { get; set; }
        public bool IsCopuponUsing { get; set; }
        /// <summary>
        /// 상품 금액
        /// </summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// 실제 결제 금액
        /// </summary>
        public int PaymentPrice { get; set; }
        /// <summary>
        /// 쿠폰 적용 금액
        /// </summary>
        public int CouponPrice { get; set; }
        public string DiscountMethodCode { get; set; }
        public double? DiscountRate { get; set; }
        public int? DiscountPrice { get; set; }

        public string DiscountViewText
        {
            get
            {
                var result = string.Empty;
                if (DiscountMethodCode == "DMC01") //금액
                    result = $"{DiscountPrice:#,##0}원 할인";
                else if (DiscountMethodCode == "DMC02") //%
                    result = $"{DiscountRate}% 할인";
                else if (DiscountMethodCode == "DMC03") //전액
                    result = $"전액 할인";

                if (!IsCopuponUsing) result += "(사용불가)";
                return result;
            }
        }
        public string PeriodMethodCode { get; set; }
        public DateTime? PublishStartDate { get; set; }
        public DateTime? PublishEndDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? RegistDateTime { get; set; }

        public string DateViewText
        {
            get
            {
                var result = string.Empty;
                if (PeriodMethodCode == "PMC01") //기간입력
                    result = $"{PublishStartDate:yyyy-MM-dd}~{PublishEndDate:yyyy-MM-dd}";
                else if (PeriodMethodCode == "PMC02") //발행일로부터 X일
                    result = $"{RegistDateTime:yyyy-MM-dd}~{ExpirationDate:yyyy-MM-dd}";
                else if (PeriodMethodCode == "PMC03") //무제한
                    result = $"사용기간 제한 없음";
                return result;
            }
        }
    }

    internal class CouponDataInfo
    {
        public int Coupon_ID { get; }
        public string Coupon_Name { get; }
        public int Coupon_Publish_ID { get; }
        public string Expiration_Date { get; }
        public DateTime? Regist_DateTime { get; }
        public int? Standard_Purchase_Price { get; }
        public string Coupon_Apply_Code { get; }
        public int? Coupon_Apply_Product_ID { get; }
        public string Discount_Method_Code { get; }
        public double? Discount_Rate { get; }
        public int? Discount_Price { get; }
        public string Period_Method_Code { get; }
        public string Publish_Start_Date { get; }
        public string Publish_End_Date { get; }
        public string CouponType { get; }

        public CouponDataInfo(int coupon_ID, string coupon_Name, int coupon_Publish_ID, string expiration_Date, DateTime? regist_DateTime, int? standard_Purchase_Price, string coupon_Apply_Code, int? coupon_Apply_Product_ID, string discount_Method_Code, double? discount_Rate, int? discount_Price, string period_Method_Code, string publish_Start_Date, string publish_End_Date, string type)
        {
            Coupon_ID = coupon_ID;
            Coupon_Name = coupon_Name;
            Coupon_Publish_ID = coupon_Publish_ID;
            Expiration_Date = expiration_Date;
            Regist_DateTime = regist_DateTime;
            Standard_Purchase_Price = standard_Purchase_Price;
            Coupon_Apply_Code = coupon_Apply_Code;
            Coupon_Apply_Product_ID = coupon_Apply_Product_ID;
            Discount_Method_Code = discount_Method_Code;
            Discount_Rate = discount_Rate;
            Discount_Price = discount_Price;
            Period_Method_Code = period_Method_Code;
            Publish_Start_Date = publish_Start_Date;
            Publish_End_Date = publish_End_Date;
            CouponType = type;
        }

        public override bool Equals(object obj)
        {
            return obj is CouponDataInfo other &&
                   Coupon_ID == other.Coupon_ID &&
                   Coupon_Name == other.Coupon_Name &&
                   Coupon_Publish_ID == other.Coupon_Publish_ID &&
                   Expiration_Date == other.Expiration_Date &&
                   Regist_DateTime == other.Regist_DateTime &&
                   Standard_Purchase_Price == other.Standard_Purchase_Price &&
                   Coupon_Apply_Code == other.Coupon_Apply_Code &&
                   Coupon_Apply_Product_ID == other.Coupon_Apply_Product_ID &&
                   Discount_Method_Code == other.Discount_Method_Code &&
                   Discount_Rate == other.Discount_Rate &&
                   Discount_Price == other.Discount_Price &&
                   Period_Method_Code == other.Period_Method_Code &&
                   Publish_Start_Date == other.Publish_Start_Date &&
                   Publish_End_Date == other.Publish_End_Date &&
                   CouponType == other.CouponType;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Coupon_ID);
            hash.Add(Coupon_Name);
            hash.Add(Coupon_Publish_ID);
            hash.Add(Expiration_Date);
            hash.Add(Regist_DateTime);
            hash.Add(Standard_Purchase_Price);
            hash.Add(Coupon_Apply_Code);
            hash.Add(Coupon_Apply_Product_ID);
            hash.Add(Discount_Method_Code);
            hash.Add(Discount_Rate);
            hash.Add(Discount_Price);
            hash.Add(Period_Method_Code);
            hash.Add(Publish_Start_Date);
            hash.Add(Publish_End_Date);
            hash.Add(CouponType);
            return hash.ToHashCode();
        }
    }
}
