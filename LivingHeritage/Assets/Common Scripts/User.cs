[System.Serializable]
public class User
{
    public string name;
    public int age;
    public bool male;
    public string exp;

    public double overAllTime;

    public User() { }

    public User(string name, int age, bool male, string exp)
    {
        this.name = name;
        this.age = age;
        this.male = male;
        this.exp = exp;
        this.overAllTime = 0;
    }
}
