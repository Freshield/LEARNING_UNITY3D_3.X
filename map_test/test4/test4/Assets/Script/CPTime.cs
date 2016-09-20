
public class CPTime{
    public float beginTime;
    public float endTime;

    public CPTime(float beginTime, float endTime)
    {
        this.beginTime = beginTime;
        if (endTime == beginTime)
        {
            this.endTime = beginTime + 60;
        }
        else
        {
            this.endTime = endTime + 60;
        }
        
    }

    public CPTime(int beginTime, int endTime)
    {
        this.beginTime = beginTime * 60;
        if (endTime == beginTime)
        {
            this.endTime = this.beginTime + 60;
        }
        else
        {
            this.endTime = (endTime + 1) * 60;
        }

    }

    public CPTime(int beginTime)
    {
        this.beginTime = beginTime * 60;
        endTime = (beginTime + 1) * 60;
        
    }

    public CPTime()
    {

    }
}
