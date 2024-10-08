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
        // TODO: check images because when copied from pdf image it is crashing, not because of images, because of some other shit
        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                base.OnSourceInitialized(e);

                // Initialize the clipboard now that we have a window source to use
                ClipboardManager windowClipboardManager = new(this);
                windowClipboardManager.ClipboardChanged += ClipboardChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("OnSourceInitialized: " + ex);
            }
        }

        private void ClipboardChanged(object sender, EventArgs e)
        {
            try
            {
                // Handle your clipboard update here:
                if (Clipboard.ContainsText())
                {
                    int tryTime = 5;
                    string text = String.Empty;

                    // Create small loop to get text like pool
                    for (int i = 0; i <= tryTime; i++)
                    {
                        Thread.Sleep(100);
                        text = Clipboard.GetText();

                        if (text != String.Empty)
                        {
                            break;
                        }
                    }

                    RegexValues.Children.Clear();
                    Keywords.Children.Clear();
                    ClipboardText.Text = text;

                    CheckRegexValues(text);
                    CheckKeywords(text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ClipboardChanged: " + ex);
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
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.VerticalContentAlignment = VerticalAlignment.Center;
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
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.VerticalContentAlignment = VerticalAlignment.Center;
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
                " zorg",
                " zend",
                "zivver",
                "doczend",
                "undeliverable",
                "report"
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
                    textBlock.Foreground = Brushes.WhiteSmoke;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Padding = new Thickness(10);
                    textBlock.Margin = new Thickness(5, 0, 5, 0);
                    Keywords.Children.Add(textBlock);
                }
            }
        }
    }
}