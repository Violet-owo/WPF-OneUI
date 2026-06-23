using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SamsungUi.Controls
{
    /// <summary>
    /// A custom versatile edit box that supports Text, Password, Number, and Email input types.
    /// Features a floating hint and bottom border in a Material-like style.
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(PasswordBox))]
    public class SamsungEditBox : Control
    {
        // --- Fields ---
        private TextBox _textBox;
        private PasswordBox _passwordBox;
        private bool _isUpdatingText;

        // --- Dependency Properties ---
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(SamsungEditBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(SamsungEditBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SamsungEditBox),
                new PropertyMetadata(new CornerRadius(16)));

        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register(
                nameof(InputType),
                typeof(InputType),
                typeof(SamsungEditBox),
                new PropertyMetadata(InputType.Text, OnInputTypeChanged));

        public static readonly DependencyProperty IsPasswordRevealedProperty =
            DependencyProperty.Register(
                nameof(IsPasswordRevealed),
                typeof(bool),
                typeof(SamsungEditBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register(
                nameof(HasText),
                typeof(bool),
                typeof(SamsungEditBox),
                new PropertyMetadata(false));

        // --- Properties ---

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public InputType InputType
        {
            get => (InputType)GetValue(InputTypeProperty);
            set => SetValue(InputTypeProperty, value);
        }

        public bool IsPasswordRevealed
        {
            get => (bool)GetValue(IsPasswordRevealedProperty);
            set => SetValue(IsPasswordRevealedProperty, value);
        }

        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            private set => SetValue(HasTextProperty, value);
        }

        // --- Initialization ---

        static SamsungEditBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungEditBox), new FrameworkPropertyMetadata(typeof(SamsungEditBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_textBox != null)
            {
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.PreviewTextInput -= TextBox_PreviewTextInput;
            }

            if (_passwordBox != null)
            {
                _passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            }

            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _passwordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;

            if (_textBox != null)
            {
                _textBox.TextChanged += TextBox_TextChanged;
                _textBox.PreviewTextInput += TextBox_PreviewTextInput;
                _textBox.Text = Text;
            }

            if (_passwordBox != null)
            {
                _passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                _passwordBox.Password = Text;
            }

            UpdateVisualState();
        }

        // --- Event Handlers & Callbacks ---

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungEditBox box)
            {
                if (box._isUpdatingText) return;

                box._isUpdatingText = true;
                string newText = e.NewValue as string ?? string.Empty;

                if (box._textBox != null && box._textBox.Text != newText)
                {
                    box._textBox.Text = newText;
                }

                if (box._passwordBox != null && box._passwordBox.Password != newText)
                {
                    box._passwordBox.Password = newText;
                }
                
                box.HasText = !string.IsNullOrEmpty(newText);
                box._isUpdatingText = false;
            }
        }

        private static void OnInputTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SamsungEditBox box)
            {
                box.UpdateVisualState();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;

            _isUpdatingText = true;
            Text = _textBox.Text;
            HasText = !string.IsNullOrEmpty(Text);
            if (_passwordBox != null && _passwordBox.Password != Text)
            {
                 _passwordBox.Password = Text;
            }
            _isUpdatingText = false;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdatingText) return;

            _isUpdatingText = true;
            Text = _passwordBox.Password;
            HasText = !string.IsNullOrEmpty(Text);
            if (_textBox != null && _textBox.Text != Text)
            {
                 _textBox.Text = Text;
            }
            _isUpdatingText = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (InputType == InputType.Number)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        // --- Helper Methods ---

        private void UpdateVisualState()
        {
            // Internal state updates can be handled here if needed
        }
    }
}
