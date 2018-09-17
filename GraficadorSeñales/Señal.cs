using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales 
{
    abstract class Señal
    {
        //Es un arreglo dinamico
        public List<Muestra> Muestras { get; set; }
        public double AmplitudMaxima { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecuenciaMuestreo { get; set; }
        

        public abstract double Evaluar(double tiempo);

        public void construirSeñalDigital()
        {
            //calcular el periodo 
            double periodoMuestreo = 1 / FrecuenciaMuestreo;

            //Se construye la señal digital, se delimita el tiempo con el for
            for (double i = TiempoInicial; i <= TiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = Evaluar(i);

                //se calcula el numero mas alto que puede tomar la señal
                if (Math.Abs(valorMuestra) > AmplitudMaxima)
                {
                   AmplitudMaxima = Math.Abs(valorMuestra);
                }

                //se van añadiendo las muestras a las listas
                Muestras.Add(new Muestra(i, valorMuestra));
            }

        }
    }
}
