
public class PTime{
    public int hour;
    public int minute;
    public float totalTime;

    public PTime(string time)
    {
        if (time.Contains(":"))
        {
            string[] times = time.Split(':');
            hour = int.Parse(times[0]);
            minute = int.Parse(times[1]);
            totalTime = hour * 60 + minute;
        }
        else
        {
            totalTime = float.Parse(time);
            hour = (int)(totalTime / 60);
            minute = (int)(totalTime % 60);
        }

    }

    public PTime(int hour, int minute)
    {
        this.hour = hour;
        this.minute = minute;
        totalTime = hour * 60 + minute;
    }

    public void setTime(float totalTime)
    {
        this.totalTime = totalTime;
        hour = (int)(totalTime / 60);
        minute = (int)(totalTime % 60);
    }

    public override string ToString()
    {
        return hour + ":" + minute;
    }
}
