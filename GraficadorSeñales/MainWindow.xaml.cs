﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            //para obtener el valor del text box se usa la propiedad .Text
            double amplitud = double.Parse(txtAmplitud.Text);
            double fase = double.Parse(txtFase.Text);
            double frecuencia = double.Parse(txtFrecuencia.Text);
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            //instancia de la clase señal senoidal
            Señal señal;
            switch(cbTipoSeñal.SelectedIndex)
            {
                //señal senoidal
                case 0: 
                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 1:
                    señal = new SeñalRampa();
                    break;
                default:
                    señal = null;
                    break;
            }

            //se establecen los valores para la funcion
            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;
            
            //se ejecuta la funcion
            señal.construirSeñalDigital();

            // limpiar la grafica
            plnGrafica.Points.Clear();

 
           if(señal != null)
            {
                //recorrer una coleccion o arreglo
                //muestra toma el valor de señal.muestra en cada recorrido del ciclo
                foreach (Muestra muestra in señal.Muestras)
                {
                    //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                    plnGrafica.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / señal.AmplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1) + (scrContenedor.Height / 2)));
                }

                //cambiar los valores de la etiqueta
                lblAmplitudMaximaPositivaY.Text = señal.AmplitudMaxima.ToString();
                lblAmplitudMaximaNegativaY.Text = "-" + señal.AmplitudMaxima.ToString();
            }

            //Graficando el eje de X
            plnEjeX.Points.Clear();
            //Punto de inicio.
            plnEjeX.Points.Add(new Point(0, (scrContenedor.Height / 2)));
            //Punto de fin.
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, (scrContenedor.Height / 2)));

            //Graficando el eje de Y
            plnEjeY.Points.Clear();
            //Punto de inicio.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height));
            //Punto de fin.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height * -1));

        }

        private void btnGraficarRampa_Click(object sender, RoutedEventArgs e)
        {
            //tomar valores
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            //instancia
            SeñalRampa señal = new SeñalRampa();

            //calcular periodomuestreo
            double periodoMuestreo = 1 / frecuenciaMuestreo;

            //limpiar puntos
            plnGrafica.Points.Clear();

            //ciclo para conseguir las muestras
            for (double i = tiempoInicial; i <= tiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = señal.Evaluar(i);
                señal.Muestras.Add(new Muestra(i, valorMuestra));

                //se van añadiendo las muestras a las listas
                señal.Muestras.Add(new Muestra(i, valorMuestra)); 

            }

            //ciclo para recorrer muestras
            foreach (Muestra muestra in señal.Muestras)
            {
                //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                plnGrafica.Points.Add(new Point(muestra.X * scrContenedor.Width, (muestra.Y * ((scrContenedor.Height / 2) - 30) * -1) + (scrContenedor.Height / 2)));
            }

        }
    }
}

