using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Management;
using System.IO;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random randonGen = new Random();
        public string[] values;
        public string[] names;
        LineSeries lineSeries1 = new LineSeries();
        bool flag = false;
        SerialProvider serial;


        List<ObservableCollection<KeyValuePair<double, double>>> sensor = new List<ObservableCollection<KeyValuePair<double, double>>>();
        //List<LineSeries> lineSeries= new List<LineSeries>();

        public MainWindow()
        {
            InitializeComponent();
            GetPorts();
            SeTBaud();
        }


        private void GetPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            Ports.Items.Clear();
            foreach (string port in ports)
            {
                Ports.Items.Add(port);
            }
        }

        private void SeTBaud()
        {
            Bauds.Items.Add("2400");
            Bauds.Items.Add("4800");
            Bauds.Items.Add("9600");
            Bauds.Items.Add("19200");
            Bauds.Items.Add("38400");
            Bauds.Items.Add("57600");
            Bauds.Items.Add("115200");
        }

        //private void GetSerialName()
        //{
        //    string result = "";
        //    using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
        //    {
        //        string[] portnames = SerialPort.GetPortNames();
        //        var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
        //        var tList = (from n in portnames join p in ports on n equals p["DeviceID"].ToString() select n + " - " + p["Caption"]).ToList();

        //        foreach (string s in tList)
        //        {
        //            result = result + s;
        //        }
        //    }
        //}


        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            if ((string)buttonConnect.Content == "Connect")
            {
                try
                {
                    string selectedPort = Ports.Text;
                    string Baud = Bauds.Text;
                    int selectedBaud = int.Parse(Baud);
                    serial = new SerialProvider(selectedPort, selectedBaud);
                }
                catch
                {
                    MessageBox.Show("Port is not selected!");
                }
                buttonConnect.Content = "Disconect";
            }
            else
            {
                try
                {
                    serial.SerialClose();
                    buttonConnect.Content = "Connect";
                }
                catch
                {
                    buttonConnect.Content = "Connect";
                }
            }

        }

        private void buttonStartClick(object sender, RoutedEventArgs e)
        {
            if ((string)buttonStart.Content == "Start")
            {
                try
                {
                    serial.SerialOpen();
                    serial.SerialReceive();
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.IsEnabled = true;
                    buttonStart.Content = "Stop";
                }
                catch
                {
                    MessageBox.Show("Port is not selected");
                }
            }
            else
            {
               serial.SerialClose();
               buttonStart.Content = "Start";
            }
        }








        double i = 0;
        private void timer_Tick(object sender, EventArgs e)
        {   
            var databuff = serial.GetFirstDataBuff();
            if (databuff != null)
            {
                string[] receiveData = databuff.Split('@');


                string[] axis = receiveData[0].Split(';');



                values = receiveData[1].Split(',');

                if (flag == false)
                {
                    for (int k = 0; k < values.Length; k++)
                    {
                        sensor.Add(new ObservableCollection<KeyValuePair<double, double>>());
                        CreateLineSeries(k ,axis[k]);
                    }
                }
                flag = true;
                for (int j = 0; j < values.Length; j++)
                {
                    var sensorValue = Double.Parse(values[j]);
                    sensor[j].Add(new KeyValuePair<double, double>(i, sensorValue));
                }
                i += 1; 
            }
        ////Application.Current.Dispatcher.Invoke(new Action(() => { textBlock.Text = "X:" + xValue + "Y:" + yValue + "Z:" + zValue + "\n"; }));
        }

        private void CreateLineSeries(int order, string title)
        {
           
            LineSeries lineSeries1 = new LineSeries();
            Style dataPointStyle = DataPointStyle();

            lineSeries1.Title = title;
            lineSeries1.DependentValuePath = "Value";
            lineSeries1.IndependentValuePath = "Key";
            lineSeries1.Background = Brushes.Blue;
            lineSeries1.ItemsSource = sensor[order];
            lineChart.Series.Add(lineSeries1);
            lineSeries1.DataPointStyle = dataPointStyle;
        }



        public Style DataPointStyle()
        {
            Color randomColor = Color.FromRgb((byte)randonGen.Next(256), (byte)randonGen.Next(256), (byte)randonGen.Next(256)); 
            Style style = new Style(typeof(DataPoint));
            Setter st1 = new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(randomColor));
            Setter st2 = new Setter(DataPoint.BorderBrushProperty,new SolidColorBrush(Colors.White));
            Setter st3 = new Setter(DataPoint.BorderThicknessProperty, new Thickness(0.1));
            Setter st4 = new Setter(DataPoint.TemplateProperty, null);
            style.Setters.Add(st1);
            style.Setters.Add(st2);
            style.Setters.Add(st3);
            style.Setters.Add(st4);
         
            return style;
        }


        private void SendData(object sender, RoutedEventArgs e)
        {
            //serial.SerialOpen();
            try
            {
                serial.SerialSend(sendBox.Text);
                sendBox.Text = "";
                //serial.SerialClose();
            }
            catch
            {
                MessageBox.Show("Port is not selected!");
                sendBox.Text = "";
            }
        }

       

    }
}

 
       

    

