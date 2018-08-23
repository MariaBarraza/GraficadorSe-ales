using System;
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
            SeñalSenoidal señal = new SeñalSenoidal(amplitud, fase, frecuencia);

            //calcular el periodo 

            double periodoMuestreo = 1 / frecuenciaMuestreo;

            //delimitar el tiempo en el que se grafica y limpiar la grafica al terminar

            plnGrafica.Points.Clear();

            for(double i= tiempoInicial; i<= tiempoFinal; i += periodoMuestreo)
            {
                // multiplicar da escalas
                plnGrafica.Points.Add(new Point(i * scrContenedor.Width ,(señal.Evaluar(i) * ((scrContenedor.Height/2) - 30) * -1) + (scrContenedor.Height/2)));
            }
           
        }
    }
}
