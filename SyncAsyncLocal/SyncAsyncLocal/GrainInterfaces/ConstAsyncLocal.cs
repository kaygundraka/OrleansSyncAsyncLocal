namespace Grains
{
    public class ValueForUnittest<T> where T : struct
    {
        private T defaultValue;
        private AsyncLocal<bool> isUnittest = new ();
        private AsyncLocal<T> unittestValue = new ();

        public ValueForUnittest(T defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                if (this.isUnittest.Value)
                {
                    return this.unittestValue.Value;
                }

                return this.defaultValue;
            }
        }

        public class UnittestSetter
        {
            public static void Set(ValueForUnittest<T> target, T value)
            {
                target.isUnittest.Value = true;
                target.unittestValue.Value = value; 
            }
        }
    }
}
