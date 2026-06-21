using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SamsungUi.Controls
{
    /// <summary>
    /// Rappresenta un campo di inserimento password che supporta la funzionalità
    /// di mostrare/nascondere il testo in chiaro (reveal).
    /// </summary>
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(PasswordBox))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_RevealButton", Type = typeof(ToggleButton))]
    public class SamsungPasswordBox : Control
    {
        private PasswordBox? _passwordBox;
        private TextBox? _textBox;
        private ToggleButton? _revealButton;
        private bool _isUpdating;

        static SamsungPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungPasswordBox), new FrameworkPropertyMetadata(typeof(SamsungPasswordBox)));
        }

        /// <summary>
        /// Identifica la dependency property per la password.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(SamsungPasswordBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));

        /// <summary>
        /// Ottiene o imposta il testo della password. Può essere associato in binding (TwoWay).
        /// </summary>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Identifica la dependency property per lo stato di visibilità della password.
        /// </summary>
        public static readonly DependencyProperty IsPasswordRevealedProperty =
            DependencyProperty.Register(nameof(IsPasswordRevealed), typeof(bool), typeof(SamsungPasswordBox), new PropertyMetadata(false, OnIsPasswordRevealedChanged));

        /// <summary>
        /// Ottiene o imposta un valore che indica se la password è attualmente visibile in chiaro.
        /// </summary>
        public bool IsPasswordRevealed
        {
            get => (bool)GetValue(IsPasswordRevealedProperty);
            set => SetValue(IsPasswordRevealedProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_passwordBox != null) _passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            if (_textBox != null) _textBox.TextChanged -= TextBox_TextChanged;

            _passwordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _revealButton = GetTemplateChild("PART_RevealButton") as ToggleButton;

            if (_passwordBox != null)
            {
                _passwordBox.Password = Password ?? string.Empty;
                _passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }

            if (_textBox != null)
            {
                _textBox.Text = Password ?? string.Empty;
                _textBox.TextChanged += TextBox_TextChanged;
            }

            UpdateVisibility();
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungPasswordBox pb)
            {
                pb.SyncPassword((string)e.NewValue ?? string.Empty);
            }
        }

        private static void OnIsPasswordRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungPasswordBox pb)
            {
                pb.UpdateVisibility();
            }
        }

        private void SyncPassword(string newPassword)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            if (_passwordBox != null && _passwordBox.Password != newPassword)
            {
                _passwordBox.Password = newPassword;
            }
            if (_textBox != null && _textBox.Text != newPassword)
            {
                _textBox.Text = newPassword;
            }

            _isUpdating = false;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;
            Password = _passwordBox.Password;
            if (_textBox != null) _textBox.Text = Password;
            _isUpdating = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;
            Password = _textBox.Text;
            if (_passwordBox != null) _passwordBox.Password = Password;
            _isUpdating = false;
        }

        private void UpdateVisibility()
        {
            if (_passwordBox == null || _textBox == null) return;

            if (IsPasswordRevealed)
            {
                _passwordBox.Visibility = Visibility.Collapsed;
                _textBox.Visibility = Visibility.Visible;
                _textBox.Focus();
                _textBox.CaretIndex = _textBox.Text.Length;
            }
            else
            {
                _passwordBox.Visibility = Visibility.Visible;
                _textBox.Visibility = Visibility.Collapsed;
                _passwordBox.Focus();
            }
        }
    }
}
