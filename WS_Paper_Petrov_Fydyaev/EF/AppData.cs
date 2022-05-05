using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_Paper_Petrov_Fydyaev.EF
{
    class AppData
    {
        public static Entities Context { get; } = new Entities();
    }
}
