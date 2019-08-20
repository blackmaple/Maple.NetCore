namespace Maple.NetCore
{
    public enum EnumQuartzJobType
    {
        None = 0 << 0,
        WithCronSchedule = 1 << 1,
        WithSimpleSchedule = 1 << 2,

    }
}
