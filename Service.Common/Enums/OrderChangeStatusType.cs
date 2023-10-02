using System.ComponentModel;
using Service.Common.Constants;

namespace Service.Common.Enums
{
    public enum OrderChangeStatusType
    {
        [Description(OrderChangeStatusDescription.Undefined)]
        Undefined = 0,

        [Description(OrderChangeStatusDescription.Created)]
        Created,

        [Description(OrderChangeStatusDescription.AddProduct)]
        AddProduct,

        [Description(OrderChangeStatusDescription.RemoveProduct)]
        RemoveProduct,

        [Description(OrderChangeStatusDescription.ChangeStatus)]
        ChangeStatus
    }
}
