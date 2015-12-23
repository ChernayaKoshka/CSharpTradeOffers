using System;
using System.Windows;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for AnimatedCloseButton.xaml
    /// </summary>
    public partial class AnimatedCloseButton
    {
        private readonly EButtonStyle _style;
        public AnimatedCloseButton()
        {
            InitializeComponent();
        }

        public AnimatedCloseButton(EButtonStyle style)
        {
            _style = style;
            Loaded += CloseButton_Load;
            InitializeComponent();
        }

        void CloseButton_Load(object sender, RoutedEventArgs e)
        {
            switch (_style)
            {
                case EButtonStyle.Close:
                    closeLines.Visibility = Visibility.Visible;
                    break;
                case EButtonStyle.Maximize:
                    maximizeLines.Visibility = Visibility.Visible;
                    break;
                case EButtonStyle.Minimize:
                    minimizeLines.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_style), _style, null);
            }
        }
    }

    public enum EButtonStyle
    {
        Close = 0,
        Maximize = 1,
        Minimize = 2
    }
}
