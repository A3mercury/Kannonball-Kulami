using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AIJob : ThreadedJob
{
    public KeyValuePair<int, int>[] AIMoveArray;
    public KeyValuePair<int, int> AIChosenMove;
    protected override void ThreadFunction()
    {
       AIChosenMove =  KulamiCSharpLibrary.KulamiCSharpLibrary.AIMove(true, AIMoveArray, 1);
    }
    protected override void OnFinished()
    {
        base.OnFinished();
    }
}

