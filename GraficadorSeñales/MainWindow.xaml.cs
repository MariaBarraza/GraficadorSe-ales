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
            InitializeComponent(); //cualquier codigo que se ponga debajo de esta se ejecuta al iniciar el programa



        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            //para obtener el valor del text box se usa la propiedad .Text
            
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            //instancia de la clase señal senoidal
            Señal señal;
            switch(cbTipoSeñal.SelectedIndex)
            {
                //señal senoidal
                case 0:
                    //el primer hijo del panel configuracion es la configuracion senoidal y es de otro tipo (ui collection)asi que se hace un casting y asi se puede acceder a sus propiedades, ademas como txtamplitud es tipo texto se usa parse
                    //nota: los hijos son los elementos de los contenedores
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal) panelConfiguracion.Children[0]).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal) panelConfiguracion.Children[0]).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal) panelConfiguracion.Children[0]).txtFrecuencia.Text);
                    
                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //rampa
                case 1:
                    señal = new SeñalRampa();
                    break;
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial) panelConfiguracion.Children[0]).txtAlpha.Text);
                    señal = new SeñalExponencial(alpha);
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

            if ((bool)cbAmplitud.IsChecked)
            {
                //Escalar
                double factorEscala = double.Parse(txtFactorEscalaAmplitud.Text);
                señal.escalar(factorEscala);
            }

            if ((bool)cbDesplazar.IsChecked)
            {
                //Desplazar
                double factorDesplazar = double.Parse(txtFactorDesplazamiento.Text);
                señal.desplazar(factorDesplazar);
            }

            if ((bool)cbTruncar.IsChecked)
            {
                //Truncar
                double factorTruncar = double.Parse(txtFactorTruncar.Text);
                señal.truncar(factorTruncar);

                señal.actualizarAmplitudMaxima();
            }
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
                double valorMuestra = señal.evaluar(i);
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

        private void cbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                //quita los elementos que puede tener el panel
                panelConfiguracion.Children.Clear();

                switch (cbTipoSeñal.SelectedIndex)
                {
                    case 0: //senoidal
                        //se añaden los elementos al panel (los hijos)
                        panelConfiguracion.Children.Add(
                          new ConfiguracionSeñalSenoidal() 
                            );

                        break;
                    case 1: //Rampa
                        break;
                    case 2:
                    panelConfiguracion.Children.Add(
                  new ConfiguracionSeñalExponencial()
                    );
                    break;
                default:
                        break;
                } 
        }

        private void cbAmplitud_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorEscalaAmplitud.IsEnabled = true;
           
        }

        private void cbAmplitud_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorEscalaAmplitud.IsEnabled = false;
        }

        

        private void cbDesplazar_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorDesplazamiento.IsEnabled = true;

        }

        private void cbDesplazar_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorDesplazamiento.IsEnabled = false;
        }


        private void cbTruncar_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorTruncar.IsEnabled = true;

        }

        private void cbTruncar_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorTruncar.IsEnabled = false;
        }
    }
}

