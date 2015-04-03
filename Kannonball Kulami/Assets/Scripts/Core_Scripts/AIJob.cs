using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AIJob : ThreadedJob
{
    public KeyValuePair<int, int>[] AIMoveArray;
    public KeyValuePair<int, int> AIChosenMove;
	public bool UseEasyAI;
	public int AIBoard;
    protected override void ThreadFunction()
    {
       AIChosenMove =  KulamiCSharpLibrary.KulamiCSharpLibrary.AIMove(UseEasyAI, AIMoveArray, AIBoard);
    }
    protected override void OnFinished()
    {
        base.OnFinished();
    }
}

