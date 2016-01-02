using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Interaction logic for AutoCompletionComboBox.xaml
    /// </summary>
    public partial class AutoCompletionComboBox : UserControl
    {
        #region Constructor

        public AutoCompletionComboBox()
        {
            InitializeComponent();

            EditableTextBox.AutoWordSelection = false;
            EditableTextBox.TextChanged += OnTextChanged;
            MainGrid.DataContext = this;
        }

        #endregion Constructor

        #region Private Types

        private class BackgroundWorkerArguments
        {
            public ISuggestionProvider Provider { get; set; }
            public string SearchText { get; set; }
            public object CurrentValue { get; set; }
        }

        private class BackgroundWorkerSuggestion
        {
            public BackgroundWorker Worker { get; set; }
            public IEnumerable<Suggestion> Suggestions { get; set; }
        }

        #endregion Private Types

        #region Private Member Variables

        private readonly List<Suggestion> suggestions = new List<Suggestion>();
        private readonly List<BackgroundWorker> workers = new List<BackgroundWorker>();

        private bool ignoreUpdate;
        private bool ignoreSelectionChange;
        private bool ignoreFocus;

        #endregion Private Member Variables

        #region Public Properties

        #region Dependency Properties

        #region SourceItems

        public static readonly DependencyProperty SourceItemsProperty
            = DependencyProperty.Register("SourceItems",
                typeof (ObservableCollection<Suggestion>),
                typeof (AutoCompletionComboBox),
                new FrameworkPropertyMetadata(new ObservableCollection<Suggestion>()));

        public ObservableCollection<Suggestion> SourceItems
        {
            get { return (ObservableCollection<Suggestion>) GetValue(SourceItemsProperty); }
            set { SetValue(SourceItemsProperty, value); }
        }

        #endregion SourceItems

        #region IsDropDownOpen

        public static readonly DependencyProperty IsDropDownOpenProperty
            = DependencyProperty.Register("IsDropDownOpen",
                typeof (bool),
                typeof (AutoCompletionComboBox),
                new FrameworkPropertyMetadata(false));

        public bool IsDropDownOpen
        {
            get { return (bool) GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        #endregion IsDropDownOpen

        #region SelectedValue

        public static readonly DependencyProperty SelectedValueProperty
            = DependencyProperty.Register("SelectedValue",
                typeof (object),
                typeof (AutoCompletionComboBox),
                new FrameworkPropertyMetadata(null,
                    OnSelectedValuePropertyChanged));

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void OnSelectedValuePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as AutoCompletionComboBox;
            // Display the formatted string
            control.ignoreUpdate = true;
            control.IsDropDownOpen = false;
            control.SetEditableTextBox(e.NewValue);
        }

        #endregion SelectedValue

        #region Format

        public static readonly DependencyProperty FormatProperty
            = DependencyProperty.Register("Format",
                typeof (string),
                typeof (AutoCompletionComboBox),
                new FrameworkPropertyMetadata(string.Empty,
                    OnFormatPropertyChanged));

        public string Format
        {
            get { return (string) GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        private static void OnFormatPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as AutoCompletionComboBox;
            // Redisplay the formatted string
            control.ignoreUpdate = true;
            control.IsDropDownOpen = false;
            control.EditableTextBox.Text = string.Format("{0" + (string) e.NewValue + "}", control.SelectedValue);
        }

        #endregion Format

        #region SuggestionProvidersCollection

        public static readonly DependencyProperty SuggestionProvidersCollectionProperty
            = DependencyProperty.Register("SuggestionProvidersCollection",
                typeof (List<ISuggestionProvider>),
                typeof (AutoCompletionComboBox),
                new FrameworkPropertyMetadata(null));

        public List<ISuggestionProvider> SuggestionProvidersCollection
        {
            get { return (List<ISuggestionProvider>) GetValue(SuggestionProvidersCollectionProperty); }
            set { SetValue(SuggestionProvidersCollectionProperty, value); }
        }

        #endregion SuggestionProvidersCollection

        #endregion Dependency Properties

        #endregion Public Properties

        #region Private Methods

        #region Focus Handling

        private void EditableTextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (ignoreFocus)
            {
                ignoreFocus = false;
            }
            else
            {
                EditableTextBox.SelectAll();
            }
        }

        private void EditableTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                switch (e.ClickCount)
                {
                    case 1:
                        // Stop keyboard focus handling (i.e. when not current focus)
                        if (!tb.IsKeyboardFocused)
                        {
                            tb.SelectionLength = 0;
                            ignoreFocus = true;
                        }
                        break;
                    case 2:
                        // Double click to focus all
                        tb.SelectAll();
                        break;
                }
            }
        }

        private void MainGrid_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ignoreUpdate = true;
            SetEditableTextBox(SelectedValue);
            ignoreUpdate = false;
            IsDropDownOpen = false;
        }

        #endregion Focus Handling

        private void SetEditableTextBox(object newValue)
        {
            EditableTextBox.Text = string.Format("{0" + Format + "}", newValue);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!IsDropDownOpen) return;

            int i;
            switch (e.Key)
            {
                // Capture Up and down keys to move the selected value
                case Key.Down:
                    i = ItemsPresenter.SelectedIndex + 1;
                    ignoreSelectionChange = true;
                    ItemsPresenter.SelectedIndex = i < ItemsPresenter.Items.Count ? i : 0;
                    ignoreSelectionChange = false;
                    break;

                case Key.Up:
                    i = ItemsPresenter.SelectedIndex - 1;
                    ignoreSelectionChange = true;
                    ItemsPresenter.SelectedIndex = i >= 0 ? i : ItemsPresenter.Items.Count - 1;
                    ignoreSelectionChange = false;
                    break;

                // Capture Enter to use the drop down selected value
                case Key.Enter:
                    if (IsDropDownOpen)
                    {
                        if (ItemsPresenter.SelectedIndex != -1)
                        {
                            SelectedValue = ComposeSelectedValue(((Suggestion) ItemsPresenter.SelectedItem).Value);
                            IsDropDownOpen = false;
                        }
                        else if (ItemsPresenter.Items.Count == 1)
                        {
                            // Only one item so use it
                            SelectedValue = ComposeSelectedValue(((Suggestion) ItemsPresenter.Items[0]).Value);
                            IsDropDownOpen = false;
                        }
                        e.Handled = true;
                    }
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        private void ItemsPresenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreSelectionChange && ItemsPresenter.SelectedIndex != -1)
            {
                SelectedValue = ComposeSelectedValue(((Suggestion) ItemsPresenter.SelectedItem).Value);
                IsDropDownOpen = false;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ignoreUpdate)
            {
                ignoreUpdate = false;
                return;
            }

            var tb = sender as TextBox;

            // Don't change selection when text is selected or no text 
            if (!string.IsNullOrEmpty(tb.Text) && string.IsNullOrEmpty(tb.SelectedText))
            {
                var currentText = tb.Text;

                // Clear the existing selections and cancel any background work
                SourceItems.Clear();
                suggestions.Clear();
                foreach (var worker in workers)
                {
                    worker.CancelAsync();
                }
                workers.Clear();

                // Start udpate process
                foreach (var suggestionProvider in SuggestionProvidersCollection)
                {
                    // Set up a background worker
                    var bw = new BackgroundWorker
                    {
                        WorkerReportsProgress = true,
                        WorkerSupportsCancellation = true
                    };

                    workers.Add(bw);

                    bw.DoWork += GetSuggestions_DoWork;
                    bw.ProgressChanged += GetSuggestions_ProgressChanged;
                    bw.RunWorkerCompleted += GetSuggestions_RunWorkerCompleted;
                    bw.RunWorkerAsync(new BackgroundWorkerArguments
                    {
                        Provider = suggestionProvider,
                        SearchText = currentText,
                        CurrentValue = SelectedValue
                    });
                }
            }

            // Set focus on the text box
            tb.Focus();
        }

        private void GetSuggestions_DoWork(object sender, DoWorkEventArgs eventArgs)
        {
            var thisWorker = sender as BackgroundWorker;
            eventArgs.Result = thisWorker; // Ensure result is just the worker

            var args = eventArgs.Argument as BackgroundWorkerArguments;
            var provider = args.Provider;
            IEnumerable<Suggestion> suggestions;

            // Start the search
            suggestions = provider.GetFirstSuggestion(args.SearchText, args.CurrentValue);

            while (suggestions != null && suggestions.Any())
            {
                if (thisWorker.CancellationPending)
                {
                    thisWorker.CancelAsync();
                    return;
                }
                thisWorker.ReportProgress(0, new BackgroundWorkerSuggestion
                {
                    Worker = thisWorker,
                    Suggestions = suggestions
                });
                suggestions = provider.GetNextSuggestion();
            }
        }

        private void GetSuggestions_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var workerSuggestion = e.UserState as BackgroundWorkerSuggestion;
            if (workers.Contains(workerSuggestion.Worker))
            {
                var newSuggestions = workerSuggestion.Suggestions;
                foreach (var suggestion in newSuggestions)
                {
                    suggestions.Add(suggestion);
                }
                // Update suggestions
                foreach (var s in suggestions.OrderBy(s => s.Probability).Reverse())
                {
                    SourceItems.Add(s);
                }
                // Open or close dropdown of the ComboBox according to whether there are sourceItems in the  
                // filtered result. 
                IsDropDownOpen = suggestions.Any();
                EditableTextBox.SelectionStart = EditableTextBox.Text.Length;
            }
        }

        private void GetSuggestions_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var bw = e.Result as BackgroundWorker;
            workers.Remove(bw);
            bw.Dispose();
        }

        protected virtual object ComposeSelectedValue(object newValue)
        {
            return newValue;
        }

        #endregion Private Methods
    }
}