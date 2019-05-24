using System;

namespace UnitTests
{
    internal class ExceptionUtils
    {
        public static Exception GetThrownException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }
    }
}