using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IItem
{
    void saysomething();
    IList<Itemdata> FakeData1();
    IList<AtkWData> FakeData2();
}


public class S1Item : IItem
{
    public void saysomething()
    {
        Debug.Log("我是第一個廠景");
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