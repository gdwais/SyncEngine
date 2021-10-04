using System;

namespace SyncEngine.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PositionAttribute : System.Attribute 
    {
        public int Position;
        public string DataTransform = string.Empty;

        public PositionAttribute(int position, 
                            string dataTransform)
        {
            Position = position;
            DataTransform = dataTransform;
        }

        public PositionAttribute(int position)
        {
            Position = position;
        }

        public PositionAttribute()
        {
        }
    }
}
    