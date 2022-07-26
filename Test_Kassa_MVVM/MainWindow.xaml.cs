using Test_Kiosk_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Test_Kiosk_MVVM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                ApplicationViewModel.logger.Info("Старт программы");
                InitializeComponent();
                var VM = new ApplicationViewModel();
                VM.AddScroll(CategoryPanel);
                VM.onButtonDownClicked += StartScrollDownAnimation;
                VM.onButtonUpClicked += StartScrollUpAnimation;
                
                DataContext = VM;
            }
            catch (Exception e)
            {
                ApplicationViewModel.logger.Error("Ошибка инициализации программы \n" + e.Message);
            }

            OrderBox.Width = SystemParameters.WorkArea.Width;
            ItemButtonWidth = SystemParameters.WorkArea.Width - CategoryPanel.Width;
        }

        public double ItemButtonWidth { get; set; }
        private Button previousButton;

        private void StartScrollUpAnimation()
        {
            float stepper = 0;
            var  scrollTimer = new DispatcherTimer();

            scrollTimer.Start();

            scrollTimer.Interval = TimeSpan.FromSeconds(0.01f);

            scrollTimer.Tick += (s, e) =>
            {
                CategoryPanel.ScrollToVerticalOffset(CategoryPanel.VerticalOffset - 1);
                stepper += 0.01f;
                
                if (stepper >= 1)
                {
                    scrollTimer.Stop();                
                }     
            }; 
        }
        private void StartScrollDownAnimation()
        {
            float stepper = 0;
            var scrollTimer = new DispatcherTimer();

            scrollTimer.Start();

            scrollTimer.Interval = TimeSpan.FromSeconds(0.01f);

            scrollTimer.Tick += (s, e) =>
            {
                CategoryPanel.ScrollToVerticalOffset(CategoryPanel.VerticalOffset + 1);
                stepper += 0.01f;
               
                if (stepper >= 1)
                {
                    scrollTimer.Stop();
                }
            };
        }
        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;  
        }
        private void CategoryButtonAnimation(object sender, RoutedEventArgs e)
        {
            var but = (Button)sender;

            but.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ffcbbb");
            if (previousButton != null)
            {
                previousButton.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f6ead4");
                DoubleAnimation animPrevious = new DoubleAnimation();
                animPrevious.From = previousButton.Height;
                animPrevious.To = previousButton.Height - 10;
                animPrevious.Duration = TimeSpan.FromSeconds(1);
                previousButton.BeginAnimation(Button.HeightProperty, animPrevious);
            }
            previousButton = but;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = but.Height;
            anim.To = but.Height + 10;
            anim.Duration = TimeSpan.FromSeconds(1);
            but.BeginAnimation(Button.HeightProperty, anim);
        }
    }
}
