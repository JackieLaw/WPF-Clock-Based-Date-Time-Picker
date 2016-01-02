using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     An auto completion suggestion.
    /// </summary>
    public struct Suggestion
    {
        #region Private Member Variables

        private readonly string prefix;
        private readonly string match;
        private readonly string suffix;
        private readonly int probability;
        private readonly object value;

        #endregion Private Member Variables

        #region Constructor

        public Suggestion(string prefix, string match, string suffix, int probability, object value)
        {
            this.prefix = prefix;
            this.match = match;
            this.suffix = suffix;
            this.probability = probability;
            this.value = value;
        }

        #endregion Constructor

        #region Public Properties

        public string Prefix
        {
            get { return prefix; }
        }

        public string Match
        {
            get { return match; }
        }

        public string Suffix
        {
            get { return suffix; }
        }

        public int Probability
        {
            get { return probability; }
        }

        public object Value
        {
            get { return value; }
        }

        public TextBlock DisplayItem
        {
            get
            {
                var tb = new TextBlock();
                if (!string.IsNullOrEmpty(Prefix))
                {
                    tb.Inlines
                        .Add(new Run
                        {
                            Text = Prefix,
                            FontWeight = FontWeights.Bold
                        });
                }
                tb.Inlines
                    .Add(new Run
                    {
                        Text = Match
                    });
                if (!string.IsNullOrEmpty(Suffix))
                {
                    tb.Inlines
                        .Add(new Run
                        {
                            Text = Suffix,
                            FontWeight = FontWeights.Bold
                        });
                }
                return tb;
            }
        }

        #endregion Public Properties
    }
}