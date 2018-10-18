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
        

        public abstract double evaluar(double tiempo);

        public void construirSeñalDigital()
        {
            //calcular el periodo 
            double periodoMuestreo = 1 / FrecuenciaMuestreo;

            //Se construye la señal digital, se delimita el tiempo con el for
            for (double i = TiempoInicial; i <= TiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = evaluar(i);

                //se calcula el numero mas alto que puede tomar la señal
                if (Math.Abs(valorMuestra) > AmplitudMaxima)
                {
                   AmplitudMaxima = Math.Abs(valorMuestra);
                }

                //se van añadiendo las muestras a las listas
                Muestras.Add(new Muestra(i, valorMuestra));
            }

        }

        public void escalar(double factor)
        {
            //por cara muestra se va a realizar esto
           foreach(Muestra muestra in Muestras)
            {
                //se multiplica por Y para escalar, no por X para conservar el instante de tiempo
                muestra.Y *= factor;
            }
        }

        public void actualizarAmplitudMaxima()
        {
            AmplitudMaxima = 0;
            
            foreach(Muestra muestra in Muestras)
            {
                if(Math.Abs(muestra.Y) > AmplitudMaxima)
                {
                    AmplitudMaxima = Math.Abs(muestra.Y);
                }
            }
        }

        public void desplazar(double factor)
        {
            //por cada muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se suma en Y para desplazar, no por X para conservar el instante de tiempo
                muestra.Y += factor;
            }
        }

        public void truncar(double umbral)
        {
            //por cada muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se suma en Y para desplazar, no por X para conservar el instante de tiempo
                if(muestra.Y>umbral)
                {
                    muestra.Y = umbral;
                }
                else
                    if(muestra.Y < -umbral)
                {
                    muestra.Y = -umbral;
                }
            }
        }

        public static Señal sumar(Señal sumando1,Señal sumando2)
        {

            return null;
        }

    }
}
