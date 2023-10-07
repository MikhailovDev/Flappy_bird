using System;
using System.Collections.Generic;

[Serializable]
public class SerialiazableScoreData
{
    public List<int> scoreNumbersInTabelList = new List<int>();

    public SerialiazableScoreData(List<int> scoreNumbersInTabelList)
    {
        this.scoreNumbersInTabelList = scoreNumbersInTabelList;
    }
}