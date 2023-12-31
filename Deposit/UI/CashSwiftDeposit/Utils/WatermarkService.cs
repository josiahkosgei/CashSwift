﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

namespace CashSwiftDeposit.Utils
{
    public static class WatermarkService
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(object), typeof(WatermarkService), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnWatermarkChanged)));
        private static readonly Dictionary<object, ItemsControl> itemsControls = new Dictionary<object, ItemsControl>();

        public static object GetWatermark(DependencyObject d) => d.GetValue(WatermarkProperty);

        public static void SetWatermark(DependencyObject d, object value) => d.SetValue(WatermarkProperty, value);

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Control control = (Control)d;
            control.Loaded += new RoutedEventHandler(Control_Loaded);
            switch (d)
            {
                case ComboBox _:
                    control.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(Control_GotKeyboardFocus);
                    control.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(Control_Loaded);
                    break;
                case TextBox _:
                    control.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(Control_GotKeyboardFocus);
                    control.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(Control_Loaded);
                    ((TextBoxBase)control).TextChanged += new TextChangedEventHandler(Control_GotKeyboardFocus);
                    break;
            }
            if (!(d is ItemsControl) || d is ComboBox)
                return;
            ItemsControl component = (ItemsControl)d;
            component.ItemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(ItemsChanged);
            itemsControls.Add(component.ItemContainerGenerator, component);
            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, component.GetType()).AddValueChanged(component, new EventHandler(ItemsSourceChanged));
        }

        private static void Control_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            if (ShouldShowWatermark(control))
                ShowWatermark(control);
            else
                RemoveWatermark(control);
        }

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            if (!ShouldShowWatermark(control))
                return;
            ShowWatermark(control);
        }

        private static void ItemsSourceChanged(object sender, EventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (itemsControl.ItemsSource != null)
            {
                if (ShouldShowWatermark(itemsControl))
                    ShowWatermark(itemsControl);
                else
                    RemoveWatermark(itemsControl);
            }
            else
                ShowWatermark(itemsControl);
        }

        private static void ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            ItemsControl itemsControl;
            if (!itemsControls.TryGetValue(sender, out itemsControl))
                return;
            if (ShouldShowWatermark(itemsControl))
                ShowWatermark(itemsControl);
            else
                RemoveWatermark(itemsControl);
        }

        private static void RemoveWatermark(UIElement control)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(control);
            if (adornerLayer == null)
                return;
            Adorner[] adorners = adornerLayer.GetAdorners(control);
            if (adorners == null)
                return;
            foreach (Adorner adorner in adorners)
            {
                if (adorner is WatermarkAdorner)
                {
                    adorner.Visibility = Visibility.Hidden;
                    adornerLayer.Remove(adorner);
                }
            }
        }

        private static void ShowWatermark(Control control) => AdornerLayer.GetAdornerLayer(control)?.Add(new WatermarkAdorner(control, GetWatermark(control)));

        private static bool ShouldShowWatermark(Control c)
        {
            switch (c)
            {
                case ComboBox _:
                    return (c as ComboBox).Text == string.Empty;
                case TextBoxBase _:
                    return (c as TextBox).Text == string.Empty;
                case ItemsControl _:
                    return (c as ItemsControl).Items.Count == 0;
                default:
                    return false;
            }
        }
    }
}
