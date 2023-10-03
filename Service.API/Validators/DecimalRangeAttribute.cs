using System.ComponentModel.DataAnnotations;

namespace Service.API.Validators
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter
        , AllowMultiple = false)]
    public class DecimalRangeAttribute : RangeAttribute
    {
        public DecimalRangeAttribute(string minimum = "0", string maximum = "79228162514264337593543950335") 
            : base(typeof(decimal), decimal.Parse(minimum).ToString(), decimal.Parse(maximum).ToString())
        {
        }
    }
}
