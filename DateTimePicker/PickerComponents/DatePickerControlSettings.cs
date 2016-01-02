using System;
using System.ComponentModel;

namespace PeteEvans.PickerComponents
{
    public class DatePickerControlSettings : INotifyPropertyChanged
    {
        private DateTime date;

        private bool isHighlighted;

        private bool isSelected;

        private bool isValid;

        private string text = string.Empty;

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public bool IsHighlighted
        {
            get { return isHighlighted; }
            set
            {
                if (value != isHighlighted)
                {
                    isHighlighted = value;
                    OnPropertyChanged("IsHighlighted");
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    OnPropertyChanged("IsValid");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}