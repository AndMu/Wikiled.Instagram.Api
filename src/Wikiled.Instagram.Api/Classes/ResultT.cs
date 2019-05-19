namespace Wikiled.Instagram.Api.Classes
{
    public class Result<T> : IResult<T>
    {
        public Result(bool succeeded, T value, ResultInfo info)
        {
            Succeeded = succeeded;
            Value = value;
            Info = info;
        }

        public Result(bool succeeded, ResultInfo info)
        {
            Succeeded = succeeded;
            Info = info;
        }

        public Result(bool succeeded, T value)
        {
            Succeeded = succeeded;
            Value = value;
        }

        public ResultInfo Info { get; } = new ResultInfo("");

        public bool Succeeded { get; }

        public T Value { get; }
    }
}