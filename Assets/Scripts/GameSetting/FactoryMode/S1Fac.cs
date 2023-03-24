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
        PlayerPrefs.SetString("S2Enter", "false");
        PlayerPrefs.SetString("S3Enter", "false");
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

        result.Add(new Itemdata() { Name = "D1-1", States = false });
        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW1-1", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW1-2", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW1-3", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW1-4", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW1-5", AWStates = false });

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
        IList<Itemdata> result = new List<Itemdata>();

        result.Add(new Itemdata() { Name = "D2-1", States = false });
        result.Add(new Itemdata() { Name = "D2-2", States = false });
        result.Add(new Itemdata() { Name = "D2-3", States = false });
        result.Add(new Itemdata() { Name = "D2-4", States = false });
        result.Add(new Itemdata() { Name = "D2-5", States = false });
        result.Add(new Itemdata() { Name = "D2-6", States = false });
        result.Add(new Itemdata() { Name = "D2-7", States = false });
        result.Add(new Itemdata() { Name = "D2-8", States = false });

        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW2-1", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW2-2", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW2-3", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW2-4", AWStates = false });

        return result;
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
        IList<Itemdata> result = new List<Itemdata>();
        
        result.Add(new Itemdata() { Name = "D3-1", States = false });
        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AW3-1", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-2", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-3", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-4", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-5", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-6", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-7", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-8", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-9", AWStates = false });
        result.Add(new AtkWData() { AWName = "AW3-10", AWStates = false });

        return result;
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

        result.Add(new Itemdata() { Name = "DN-1", States = false });
        result.Add(new Itemdata() { Name = "DN-2", States = false });
        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        result.Add(new AtkWData() { AWName = "AWN-1", AWStates = false });
        result.Add(new AtkWData() { AWName = "AWN-2", AWStates = false });

        return result;
    }
}