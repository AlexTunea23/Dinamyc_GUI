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
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random randonGen = new Random();
        Random random1 = new Random();
        Random random2 = new Random();
        public string[] values;
        public string[] names;
        LineSeries lineSeries1 = new LineSeries();
        bool flag = false;
        SerialProvider serial;
        public bool flagFormat;
        int contor;
        DispatcherTimer timer = new DispatcherTimer();
        Color colorSet = new Color();
        List<ObservableCollection<KeyValuePair<double, double>>> sensor = new List<ObservableCollection<KeyValuePair<double, double>>>();
        public int dataPoint;
        KeyValuePair<double,double>xaxis=new KeyValuePair<double,double>();
        public bool dataPointFlag=false;
        public bool startingPointFlag = true;
        public MainWindow()
        {
            InitializeComponent();
            GetPorts();
            SeTBaud();
            DefaultConfig();
            axisTitle.Text = "X-Accel,Y-Accel,Z-Accel";
            axisTitle.IsEnabled = false;
            dataPointNumber.Text = "5000";
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

        private void DefaultConfig()
        {
            sendBoxA0.Text = "2000";
            sendBoxA1.Text = "2000";
            tresholdBox.Text = "250";
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
                ResetButtons();
            }
            else
            {
               
                try
                {
                    //serial.SerialClose();
                    ResetButtons();
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
                    if (!serial.IsOpen)
                    {
                        serial.SerialOpen();
                        serial.SerialReceive();
                    }
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.IsEnabled = true;
                    buttonStart.Content = "Stop";
                    checkDataPoint.IsEnabled = false;
                    dataPointNumber.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show("Port is not selected");
                }
            }
            else
            {
                if (serial.IsOpen)
                {
                    dataPointFlag = true;
                    startingPointFlag = false;
                    var maxX=sensor[0].Max(t=>t.Value);
                    var maxY = sensor[1].Max(t => t.Value);
                    var maxZ = sensor[2].Max(t => t.Value);

                    var minX = sensor[0].Min(t => t.Value);
                    var minY = sensor[1].Min(t => t.Value);
                    var minZ = sensor[2].Min(t => t.Value);

                    Application.Current.Dispatcher.Invoke(new Action(() => { maxValue.Text = "X:" + maxX+" " + "Y:" +maxY+ " " + "Z:" +maxZ; }));
                    Application.Current.Dispatcher.Invoke(new Action(() => { minValue.Text = "X:" + minX + " " + "Y:" + minY + " " + "Z:" + minZ; }));
             
               
              
                        timer.IsEnabled = false;
                    serial.SerialClose();
                    buttonStart.Content = "Start";
                   // dataPointNumber.IsEnabled = true;
                    
                }
            }
        }

        double i = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            var databuff = serial.GetFirstDataBuff();
            if(serial.contorReceive>0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { dataMissed.Text = "Number of packeges: "+serial.contorReceive; }));
                //dataMissed.Text = "";
            }
            if (databuff != null)
            {
                string[] values = databuff.Split(',');
                var axis = axisTitle.Text;
                string[] title = axis.Split(',');    
                if (flag == false)
                {
                    for (int k = 0; k < values.Length; k++)
                    {
                        sensor.Add(new ObservableCollection<KeyValuePair<double, double>>());
                        if(dataPoint==1)
                        {
                            CreateLineSeriesVisibleDataPoint(k, title[k]);
                            lineSeries1.DataPointStyle = DataPointStyle(1);
                        }
                        else
                        {
                            CreateLineSeries(k, title[k]);
                        }
                        
                    }
                }
                flag = true;
                var checkconversion = true;
                var setValues = new List<KeyValuePair<double, double>>();
                for (int j = 0; j < values.Length; j++)
                {
                    
                    double tempVal=0;
                    checkconversion = checkconversion && Double.TryParse(values[j], out tempVal);
                    if (!checkconversion)
                    {
                        contor += 1;
                        Application.Current.Dispatcher.Invoke(new Action(() => { receiveBlock.Text ="Number of unprocessed data: " +contor; }));
                        break; 
                    }
                    setValues.Add(new KeyValuePair<double, double>(i, tempVal));
                }
                if (checkconversion)
                {
                    for (int j = 0; j < setValues.Count; j++ )
                        sensor[j].Add(setValues[j]);
                }
                i = (i+0.38*100);
                string dataP=dataPointNumber.Text;
                int dataPoints = int.Parse(dataP);
                
                //if (startingPointFlag == true)
                // {
                    if (i > dataPoints)
                    {
                
                        sensor[0].RemoveAt(0);
                        sensor[1].RemoveAt(0);
                        sensor[2].RemoveAt(0);
                    }
                    dataPointFlag = false;
                //}
                //if (dataPointFlag == true)
                //{
                //    startingPointFlag = false;
                //    dataP = dataPointNumber.Text;
                //    dataPoints = int.Parse(dataP);
                //    double checking = 0;
                //    checking = i;
                //    if (sensor[0].Count() > dataPoints)
                //    {
                //        for (int q = 0; q > sensor[0].Count - dataPoints; q++)
                //        {
                //            sensor[0].RemoveAt(0);
                //            sensor[1].RemoveAt(0);
                //            sensor[2].RemoveAt(0);
                //        }
                //    }
                //    dataPointFlag = false;
                //    startingPointFlag = true;
                //}
            }
        }

        private void CreateLineSeries(int order,string title)
        {
            
            LineSeries lineSeries1 = new LineSeries();
            Style dataPointStyle = DataPointStyle(order);

            lineSeries1.Title = title;
            lineSeries1.DependentValuePath = "Value";
            lineSeries1.IndependentValuePath = "Key";
            lineSeries1.Background = Brushes.Blue;
            lineSeries1.ItemsSource = sensor[order];
            lineChart.Series.Add(lineSeries1);
            lineSeries1.DataPointStyle = dataPointStyle;
        }

        private void CreateLineSeriesVisibleDataPoint(int order, string title)
        {

            LineSeries lineSeries1 = new LineSeries();
            Style dataPointStyle = DataPointStyleVisibleDataPoint(order);

            lineSeries1.Title = title;
            lineSeries1.DependentValuePath = "Value";
            lineSeries1.IndependentValuePath = "Key";
            lineSeries1.Background = Brushes.Blue;
            lineSeries1.ItemsSource = sensor[order];
            lineChart.Series.Add(lineSeries1);
            lineSeries1.DataPointStyle = dataPointStyle;
        }

        public Style DataPointStyle(int number)
        {
            Color colorRan = ColorSetter(number);
            Style style = new Style(typeof(DataPoint));
            Setter st1 = new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(colorRan));
            Setter st2 = new Setter(DataPoint.BorderBrushProperty,new SolidColorBrush(Colors.White));
            Setter st3 = new Setter(DataPoint.BorderThicknessProperty, new Thickness(0.2));
            Setter st4 = new Setter(DataPoint.TemplateProperty, null);
           
            style.Setters.Add(st1);
            style.Setters.Add(st2);
            style.Setters.Add(st3);
            style.Setters.Add(st4);
         
            return style;
        }

        public Style DataPointStyleVisibleDataPoint(int number)
        {
            Color colorRan = ColorSetter(number);
            Style style = new Style(typeof(DataPoint));
            Setter st1 = new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(colorRan));
            Setter st2 = new Setter(DataPoint.BorderBrushProperty, new SolidColorBrush(Colors.White));
            Setter st3 = new Setter(DataPoint.BorderThicknessProperty, new Thickness(0.2));
            //Setter st4 = new Setter(DataPoint.TemplateProperty, null);

            style.Setters.Add(st1);
            style.Setters.Add(st2);
            style.Setters.Add(st3);

            return style;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "start=";
                serial.SerialSend(command);
            }
            catch
            {
                MessageBox.Show("Port is not selected!");
            }
        }

        private void sendPeriodA1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "setPeriodA1=";
                string periodA1 = sendBoxA1.Text;

                string setPeriodA1 = command + periodA1;

                serial.SerialSend(setPeriodA1);
                sendPeriodA1.Background= Brushes.Gray;
            }
            catch
            {
                MessageBox.Show("Port is not selected!");
                sendBoxA1.Text = "";
            }
        }

        private void sendPeriodA0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string command = "setPeriodA0=";
                string periodA0 = sendBoxA0.Text;

                string setPeriodA0 = command + periodA0;

                serial.SerialSend(setPeriodA0);
                sendPeriodA0.Background = Brushes.Gray;
            }
            catch
            {
                MessageBox.Show("Port is not selected!");
                sendBoxA0.Text = "";
            }
        }

        private void sendTreshold_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "setTreshold=";
                string treshold = tresholdBox.Text;

                string setTreshold = command + treshold;

                serial.SerialSend(setTreshold);
                sendTreshold.Background = Brushes.Gray;
            }
            catch
            {
                MessageBox.Show("Port is not available!");
                tresholdBox.Text = "";
            }
        }

        private void StartAcquisition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "start=";
                serial.SerialSend(command);
                StartAcquisition.IsEnabled = false;
                StopAcquisition.IsEnabled = true;
                sendTreshold.IsEnabled = false;
                sendPeriodA0.IsEnabled = false;
                sendPeriodA1.IsEnabled = false;

            }
            catch
            {
                MessageBox.Show("Port is not available!");
            }
        }

        private void StopAcquisition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = "stop=";
                serial.SerialSend(command);
                
                StartAcquisition.IsEnabled = true;
                ResetButtons();
                StopAcquisition.IsEnabled = false;
            }
            catch(Exception excep)
            {
                MessageBox.Show("Port is not available!");
            }
          
        }
        
        public void ResetButtons()
        {
             StartAcquisition.IsEnabled = true;
             StopAcquisition.IsEnabled = true;
             sendPeriodA0.IsEnabled = true;
             sendPeriodA1.IsEnabled = true;
             sendTreshold.IsEnabled = true;
             sendTreshold.Background = Brushes.LightGray;
             sendPeriodA0.Background = Brushes.LightGray;
             sendPeriodA1.Background = Brushes.LightGray;
        }

        public string[] ColourValues = new string[] { 
        "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF", "000000", 
        "800000", "008000", "000080", "808000", "800080", "008080", "808080", 
        "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0", 
        "400000", "004000", "000040", "404000", "400040", "004040", "404040", 
        "200000", "002000", "000020", "202000", "200020", "002020", "202020", 
        "600000", "006000", "000060", "606000", "600060", "006060", "606060", 
        "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0", 
        "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0", 
    };
        public Color ColorSetter(int setter)
        {
           
            if (setter == 0)
            {
                Color c1 = Color.FromRgb(173, 255, 47);
                colorSet = c1;
            }
            else if(setter==1)
            {
                Color c2 = Color.FromRgb(0, 191, 255);
                colorSet = c2;
            }
            else if(setter==2)
            {
           
                Color c3 = Color.FromRgb(255, 0, 0);
                colorSet = c3;
            }
            else if(setter==3)
            {
                Color c4= Color.FromRgb(250,250,210);
            }
            return colorSet;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            dataPoint = 1;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            dataPoint = 0;
        }
    
}
    
    
    }





 
       

    

