// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.DelayCaller
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Threading;

namespace Framework.Stuff
{
  public class DelayCaller
  {
    private readonly object _threadLock = new object();
    private readonly int _cycleTimeMS = 100;
    private readonly Action _action;
    private Thread _thread;
    private double _countDownMS;

    public DelayCaller(Action action)
    {
      this._action = action;
    }

    public int InitialCountdownMS { get; set; }

    public void InvokeOrReset()
    {
      lock (this._threadLock)
      {
        if (this._thread == null)
        {
          this._thread = new Thread((ThreadStart) (() =>
          {
            this._countDownMS = (double) this.InitialCountdownMS;
            while (this._countDownMS > 0.0)
            {
              this._countDownMS -= (double) this._cycleTimeMS;
              Thread.Sleep(this._cycleTimeMS);
            }
            try
            {
              this._action();
            }
            finally
            {
              lock (this._threadLock)
                this._thread = (Thread) null;
            }
          }));
          this._thread.IsBackground = true;
          this._thread.Start();
        }
        else
          this._countDownMS = (double) this.InitialCountdownMS;
      }
    }

    public void Stop()
    {
      lock (this._threadLock)
      {
        if (this._thread == null)
          return;
        this._thread.Abort();
        this._thread = (Thread) null;
      }
    }
  }
}
