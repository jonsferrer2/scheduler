namespace Scheduler.API.Common;

public class Result<T>
{
    public bool IsSucces { get; set; } = false;
    public string Message { get; set; } = "";
    public int ErrorCode { get; set; }
    public T? Data { get; set; }
}