using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Empleado
    {
        public long Id { get; set; }
        public string NombreCompleto { get; set; }
        public long IdDepartamento { get; set; }
        public decimal Sueldo { get; set; }
        public DateTime FechaContrato { get; set; }
        public bool Eliminado { get; set; }
        public Departamento? Departamento { get; set; }

    }
}
