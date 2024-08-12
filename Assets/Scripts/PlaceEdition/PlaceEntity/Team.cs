// 简单的Team类，用于表示队伍
[System.Serializable]
public class Team
{
    public int Id;
    public string Name;
    // 这里可以添加更多队伍相关的属性和方法
    public int MaxTeamNumber;

    public int score;

    public float ink;
    public float hisInk;
    public float hisExInk;

    public int currentTeamNumberCount;
    
    public Team(int id, string name, int max)
    {
        Id = id;
        Name = name;
        MaxTeamNumber = max;
        score = 0;
        ink = 0;
    }

    public void Reset() {
        score = 0;
        ink = 0;
        hisInk = 0;
        hisExInk = 0;
        currentTeamNumberCount = 0;
    }
}