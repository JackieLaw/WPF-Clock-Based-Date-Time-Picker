using System.ComponentModel;

namespace PeteEvans.PickerComponents
{
    public class StringObject : INotifyPropertyChanged
    {
        private string content = string.Empty;

        public string Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}