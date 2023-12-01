using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


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

        //最大場景數量為4(關卡1~3，Boss為一關)，0為遊戲開始場景所以無須擔心。
        //將全部入場證明關閉
        for(int i = 1; i < SceneManager.GetActiveScene().buildIndex; i++) 
        {
            //開場動畫和結尾動畫是5和6
            if(i < 5)
                PlayerPrefs.SetString($"S{i}Enter", "false");
        }
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

        //門的最大數量為5，編號從1開始算
        for(int i = 1; i < 6; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW1-{i}", AWStates = false });
        }

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

        //門的最大數量為8，編號從1開始算
        for ( int i = 1; i < 9; i++)
        {
            result.Add(new Itemdata() { Name = $"D2-{i}", States = false });
        }

        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        //牆的最大數量為4，編號從1開始算
        for ( int i = 1; i < 5; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW2-{i}", AWStates = false });
        }

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

        //牆的最大數量為10，編號從1開始算
        for(int i = 1; i < 11; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW3-{i}", AWStates = false });
        }

        return result;
    }
}

public class S4Item : IItem
{
    public void saysomething()
    {
        Debug.Log("我是第四個場景");
    }

    public IList<Itemdata> FakeData1()
    {
        IList<Itemdata> result = new List<Itemdata>();
        
        result.Add(new Itemdata() { Name = "D4-1", States = false });
        return result;
    }

    public IList<AtkWData> FakeData2()
    {
        IList<AtkWData> result = new List<AtkWData>();

        //牆的最大數量為6，編號從1開始算
        for(int i = 1; i < 7; i++)
        {
            result.Add(new AtkWData() { AWName = $"AW4-{i}", AWStates = false });
        }

        return result;
    }
}

#region 這是關於測試場景的類別，實際遊玩不會碰到
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
#endregion
