using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IItem
{
    void saysomething();
    IList<Itemdata> FakeData1();
    IList<AtkWData> FakeData2();
}

public class SSItem : IItem
{
    public void saysomething()
    {
        Debug.Log("我是開始廠景");
        PlayerPrefs.SetString("S1Enter", "false");
    }

    public IList<Itemdata> FakeData1()
    {
        return null;
    }
    public IList<AtkWData> FakeData2()
    {
        return null;
    }
}
public class S1Item : IItem
{
    public void saysomething()
    {
        Debug.Log("我是第一個廠景");
    }

    public IList<Itemdata> FakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = bool.Parse((PlayerPrefs.GetString("D1-1S"))) });
        return result;
    }
    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW1-1", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-1S"))) });
        result.Add(new AtkWData() { AWName = "AW1-2", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-2S"))) });
        result.Add(new AtkWData() { AWName = "AW1-3", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-3S"))) });
        result.Add(new AtkWData() { AWName = "AW1-4", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-4S"))) });
        result.Add(new AtkWData() { AWName = "AW1-5", AWStates = bool.Parse((PlayerPrefs.GetString("AW1-5S"))) });

        return result;
    }
}
public class S2Item : IItem
{
    public void saysomething()
    {
        Debug.Log("我是第二個廠景");
    }
    public IList<Itemdata> FakeData1()
    {
        return null;
    }
    public IList<AtkWData> FakeData2()
    {
        return null;
    }
}
public class S3Item : IItem
{
    public void saysomething()
    {
        Debug.Log("我是第三個場景");
    }
    public IList<Itemdata> FakeData1()
    {
        return null;
    }
    public IList<AtkWData> FakeData2()
    {
        return null;
    }
}
public class NewSItem : IItem
{
    public void saysomething()
    {
        Debug.Log("我是新的場景");
    }
    public IList<Itemdata> FakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D1-1", States = false });
        result.Add(new Itemdata() { Name = "D1-2", States = false });
        return result;
    }
    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();
        
        result.Add(new AtkWData() { AWName = "AW1-1", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW1-2", AWStates = false });

        return result;
    }
}