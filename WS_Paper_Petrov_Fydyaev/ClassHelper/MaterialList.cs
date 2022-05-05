using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_Paper_Petrov_Fydyaev.EF
{
    public partial class VM_MaterialList
    {
        public string GetTypeAndName { get => $"{TypeMaterial} | {NameMaterial}"; }
        public string GetMinCount { get => $"Минимальное количество: {MinimumCount} шт"; }
        public string GetSuppliar { get => $"Поставщики: {SuppliarName}"; }
        public string GetCountInStock { get => $"Остаток: {CountInStock} шт"; }

        public string GetColor
        {
            get
            {
                if (CountInStock < MinimumCount)
                {
                    return "#f19292";
                }
                else if (MinimumCount * 3 > CountInStock)
                {
                    return "#ffba01";
                }
                else
                {
                    return "#ffffff";
                }
            }
        }

    }
}
