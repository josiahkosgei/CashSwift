// Decompiled with JetBrains decompiler
// Type: CashSwiftDeposit.ViewModels.TimedScreenBase
// Assembly: CashSwiftDeposit, Version=6.6.5.7, Culture=neutral, PublicKeyToken=null
// MVID: 7BC9C8AC-6829-47FD-BEA6-5232B37B7616
// Assembly location: C:\DEV\maniwa\bak\New folder\6.0 - Demo\CashSwiftDeposit.exe

using System;
using System.Windows.Threading;

namespace CashSwiftDeposit.ViewModels
{
    public abstract class TimedScreenBase
    {
        protected DispatcherTimer idleTimer = new DispatcherTimer();

        protected TimeSpan TimeSpan { get; }

        public TimedScreenBase(int timeSpan)
        {
            TimeSpan = TimeSpan.FromSeconds(timeSpan);
            if (timeSpan <= 0)
                return;
            idleTimer.Interval = TimeSpan;
            idleTimer.Tick += new EventHandler(IdleTimer_Tick);
            idleTimer.IsEnabled = true;
        }

        protected abstract void IdleTimer_Tick(object sender, EventArgs e);

        protected abstract void IdleTimer_Reset(object sender, EventArgs e);
    }
}
