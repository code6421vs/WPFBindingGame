using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TwoColumnViewModel> _ds = new List<TwoColumnViewModel>();
        DataObject data = new DataObject() { Header = "TEST", Value = 15 };
        public string DataText
        {
            get
            {
                return "this is binding";
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            _ds.Add(new TwoColumnViewModel("Header", data, "Value"));
            list.ItemsSource = _ds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data.Value = 17;
        }
    }

    public class TwoColumnViewModel : INotifyPropertyChanged
    {
        private INotifyPropertyChanged _attachObject;
        private PropertyInfo _attachProperty;
        private string _unit, _format;

        public string Name { get; set; }

        public string Value
        {
            get
            {
                if (_attachProperty?.PropertyType == typeof(double))
                {
                    double val = Math.Round((double)_attachProperty?.GetValue(_attachObject), 0);
                    return string.IsNullOrEmpty(_format) ? val.ToString() : val.ToString(_format) + _unit;
                }
                else if (_attachProperty?.PropertyType == typeof(uint))
                {
                    uint val = (uint)_attachProperty?.GetValue(_attachObject);
                    return string.IsNullOrEmpty(_format) ? val.ToString() : val.ToString(_format) + _unit;
                }
                else if (_attachProperty?.PropertyType == typeof(float?))
                {
                    float val = (float?)_attachProperty?.GetValue(_attachObject) ?? 0;
                    return string.IsNullOrEmpty(_format) ? val.ToString() : val.ToString(_format) + _unit;
                }
                return string.Empty;
            }
            set
            {
                _attachProperty?.SetValue(_attachObject, value);
            }
        }

        public TwoColumnViewModel(string name, INotifyPropertyChanged attachObject, string property, string format = "", string unit = "")
        {
            Name = name;
            _format = format;
            _unit = unit;
            _attachObject = attachObject;
            _attachProperty = attachObject.GetType().GetProperty(property);
            _attachObject.PropertyChanged += _attachObject_PropertyChanged;
        }

        private void _attachObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DataObject:INotifyPropertyChanged
    {
        private string _header;
        private double _value;
        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Header"));
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
