using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalRampa
    {
        public List<Muestra> Muestras { get; set; }

        //Propiedades
        public SeñalRampa()
        {
            Muestras = new List<Muestra>();
        }
        
        //Logica Evaluacion
        public double EvaluarRampa(double tiempo)
        {
            double resultado=0;

            if (tiempo >= 0)
            {
                resultado = tiempo;
            }
            else
                if (tiempo < 0)
            {
                resultado = 0;
            }

            return resultado;
        }

    }
}
