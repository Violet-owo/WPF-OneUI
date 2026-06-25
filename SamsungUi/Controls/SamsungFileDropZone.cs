#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8622, CS8625
using System;
using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungFileDropZone : Control
    {
        public event EventHandler<string[]> FilesDropped;

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SamsungFileDropZone), new PropertyMetadata("Drag and drop files here"));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(SamsungFileDropZone), new PropertyMetadata("\uE8B7")); // Cloud icon

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SamsungFileDropZone), new PropertyMetadata(new CornerRadius(16)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty IsDragOverProperty =
            DependencyProperty.Register("IsDragOver", typeof(bool), typeof(SamsungFileDropZone), new PropertyMetadata(false));

        public bool IsDragOver
        {
            get { return (bool)GetValue(IsDragOverProperty); }
            private set { SetValue(IsDragOverProperty, value); }
        }

        static SamsungFileDropZone()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungFileDropZone), new FrameworkPropertyMetadata(typeof(SamsungFileDropZone)));
        }

        public SamsungFileDropZone()
        {
            AllowDrop = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                IsDragOver = true;
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            IsDragOver = false;
            e.Handled = true;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            IsDragOver = false;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    FilesDropped?.Invoke(this, files);
                }
            }
            e.Handled = true;
        }
    }
}
