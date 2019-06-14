// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.OneWaitTimer
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Threading;

namespace Framework.Stuff
{
  public class OneWaitTimer : IDisposable
  {
    private readonly object _countDownLock = new object();
    private readonly AutoResetEvent _evt = new AutoResetEvent(false);
    private readonly object _isWaitingLock = new object();
    private int _countDownMilliseconds = 1500;
    private readonly Thread _thread;
    private bool _isWaiting;

    public OneWaitTimer()
    {
      this._thread = new Thread(new ThreadStart(this.ThreadProc));
      this._thread.IsBackground = true;
      this._thread.Start();
    }

    public void Dispose()
    {
      try
      {
        this._thread.Abort((object) 200);
      }
      catch
      {
      }
    }

    private void ThreadProc()
    {
      DateTime now = DateTime.Now;
      while (true)
      {
        lock (this._isWaitingLock)
        {
          if (this._isWaiting)
          {
            TimeSpan timeSpan = DateTime.Now - now;
            lock (this._countDownLock)
            {
              this._countDownMilliseconds -= (int) timeSpan.TotalMilliseconds;
              if (this._countDownMilliseconds <= 0)
              {
                this._evt.Set();
                this._isWaiting = false;
              }
            }
          }
        }
        now = DateTime.Now;
        Thread.Sleep(50);
      }
    }

    public bool WaitFor(int milliseconds)
    {
      if (this._isWaiting)
      {
        lock (this._countDownLock)
          this._countDownMilliseconds = milliseconds;
        return false;
      }
      lock (this._isWaitingLock)
      {
        this._isWaiting = true;
        lock (this._countDownLock)
          this._countDownMilliseconds = milliseconds;
      }
      this._evt.WaitOne();
      return true;
    }
  }
}
