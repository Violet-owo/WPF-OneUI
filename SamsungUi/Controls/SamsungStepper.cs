using System.Windows;
using System.Windows.Controls;

namespace SamsungUi.Controls
{
    public class SamsungStepper : ItemsControl
    {
        static SamsungStepper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungStepper), new FrameworkPropertyMetadata(typeof(SamsungStepper)));
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SamsungStepper), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SamsungStepperItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SamsungStepperItem;
        }
    }

    public class SamsungStepperItem : ContentControl
    {
        static SamsungStepperItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamsungStepperItem), new FrameworkPropertyMetadata(typeof(SamsungStepperItem)));
        }

        public static readonly DependencyProperty IsCompletedProperty =
            DependencyProperty.Register("IsCompleted", typeof(bool), typeof(SamsungStepperItem), new PropertyMetadata(false));

        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, value); }
        }

        public static readonly DependencyProperty IsCurrentProperty =
            DependencyProperty.Register("IsCurrent", typeof(bool), typeof(SamsungStepperItem), new PropertyMetadata(false));

        public bool IsCurrent
        {
            get { return (bool)GetValue(IsCurrentProperty); }
            set { SetValue(IsCurrentProperty, value); }
        }

        public static readonly DependencyProperty StepIndexProperty =
            DependencyProperty.Register("StepIndex", typeof(string), typeof(SamsungStepperItem), new PropertyMetadata("1"));

        public string StepIndex
        {
            get { return (string)GetValue(StepIndexProperty); }
            set { SetValue(StepIndexProperty, value); }
        }

        public static readonly DependencyProperty IsLastStepProperty =
            DependencyProperty.Register("IsLastStep", typeof(bool), typeof(SamsungStepperItem), new PropertyMetadata(false));

        public bool IsLastStep
        {
            get { return (bool)GetValue(IsLastStepProperty); }
            set { SetValue(IsLastStepProperty, value); }
        }
    }
}
