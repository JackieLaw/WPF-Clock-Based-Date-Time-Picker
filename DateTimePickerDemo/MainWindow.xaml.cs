using System;
using System.Windows;

namespace DateTimePickerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DateTimePicker.SelectedValue = new DateTime(2015, 3, 21, 13, 15, 0);
            DateTimePicker2.SelectedValue = new DateTime(2016, 3, 21, 13, 15, 0);
        }
    }
}
