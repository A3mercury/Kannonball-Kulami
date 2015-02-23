using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ThreadedJob
{
    private bool m_Done = false;
    private object m_Handle = new object();
    private System.Threading.Thread m_Thread = null;
    public bool Done
    {
        get
        {
            bool tmp;
            lock (m_Handle)
            {
                tmp = m_Done;
            }
            return tmp;
        }
        set
        {
            lock (m_Handle)
            {
                m_Done = value;
            }
        }
    }
    public virtual void Start()
    {
        m_Thread = new System.Threading.Thread(Run);
        m_Thread.Start();
    }
    public virtual void Abort()
    {
        m_Thread.Abort();
    }
    protected virtual void ThreadFunction()
    { }

    protected virtual void OnFinished()
    {
    }
    public virtual bool Update()
    {
        if (m_Done)
        {
            OnFinished();
            return true;
        }
        return false;
    }
    private void Run()
    {
        ThreadFunction();
        m_Done = true;
    }
}
