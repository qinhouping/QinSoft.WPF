using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace QinSoft.WPF.Control
{
    public class PopupExt : Popup
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetActiveWindow(IntPtr hWnd);

        static PopupExt()
        {
            EventManager.RegisterClassHandler(typeof(PopupExt), Popup.PreviewGotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnPreviewGotKeyboardFocus), true);
        }

        private static void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.NewFocus as TextBoxBase;
            if (textBox != null)
            {
                var hwndSource = PresentationSource.FromVisual(textBox) as HwndSource;
                if (hwndSource != null)
                {
                    SetActiveWindow(hwndSource.Handle);
                }
            }
        }
    }
}
