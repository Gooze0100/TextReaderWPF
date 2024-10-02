using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TextReaderWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RegexValues.Children.Clear();
            Keywords.Children.Clear();
            ClipboardText.Text = String.Empty;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Initialize the clipboard now that we have a window soruce to use
            var windowClipboardManager = new ClipboardManager(this);
            windowClipboardManager.ClipboardChanged += ClipboardChanged;
        }

        private void ClipboardChanged(object sender, EventArgs e)
        {
            // Handle your clipboard update here, debug logging example:
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();

                RegexValues.Children.Clear();
                Keywords.Children.Clear();
                ClipboardText.Text = text;

                CheckRegexValues(text);
                CheckKeywords(text);
            }
        }

        public void OnReset(object sender, RoutedEventArgs e)
        {
            RegexValues.Children.Clear();
            Keywords.Children.Clear();
            ClipboardText.Text = String.Empty;
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Clipboard.SetText(button.Content.ToString());
        }

        private void CheckRegexValues(string clipboardText)
        {
            RegexManager regexManager = new();
            var regexMatchesForSolveIT = regexManager.CheckRegexInText(clipboardText, SystemEnum.SolveIT).Distinct();
            var regexMatchesForAptic = regexManager.CheckRegexInText(clipboardText, SystemEnum.Aptic).Distinct();

            foreach (string item in regexMatchesForSolveIT)
            {
                Button btn = new();
                btn.Click += CopyToClipboard;
                btn.Margin = new Thickness(5, 0, 5, 5);
                btn.Content = item;
                btn.FontSize = 16;
                btn.FontWeight = FontWeights.Bold;
                RegexValues.Children.Add(btn);
            }

            foreach (string item in regexMatchesForAptic)
            {
                Button btn = new();
                btn.Click += CopyToClipboard;
                btn.Margin = new Thickness(5, 0, 5, 5);
                btn.Content = item;
                btn.FontSize = 16;
                btn.FontWeight = FontWeights.Bold;
                RegexValues.Children.Add(btn);
            }
        }

        private void CheckKeywords(string clipboardText)
        {
            List<string> keywords = new()
            {
                "steunvordering",
                "nyhetsbrev",
                "nieuws",
                "zorg",
                "zend",
                "zivver",
                "doczend"
            };

            foreach (string keyword in keywords)
            {
                if (clipboardText.ToLower().Contains(keyword))
                {
                    TextBlock textBlock = new();
                    textBlock.Text = keyword.ToUpper();
                    textBlock.FontSize = 16;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.Background = Brushes.Crimson;
                    textBlock.Foreground = Brushes.DarkSlateBlue;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Padding = new Thickness(10);
                    textBlock.Margin = new Thickness(5, 0, 5, 0);
                    Keywords.Children.Add(textBlock);
                }
            }
        }
    }
}