using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ConnectionStrings
    {
        //No va a recibir valores nullos, siempre tendra un valor
        public string CadenaSQL { get; set; } = null!;
    }
}
