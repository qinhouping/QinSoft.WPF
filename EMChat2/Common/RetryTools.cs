using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    /// <summary>
    /// 重试工具
    /// </summary>
    public static class RetryTools
    {

        public static int DefaultTryCount = 3;

        public static int DefaultSleep = 100;

        /// <summary>
        /// 判断异常是否属于重试异常类型
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="RetryExceptionTypes"></param>
        /// <returns></returns>
        private static bool IsRetryException(Exception exception, IEnumerable<Type> RetryExceptionTypes)
        {
            Type type = exception.GetType();
            foreach (Type t in RetryExceptionTypes)
            {
                if (t.IsAssignableFrom(type))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Retry(this Action Execute, int Count = 3, int Sleep = 100)
        {
            IEnumerable<Type> RetryExceptionTypes = new Type[] { typeof(Exception) };

            Retry(Execute, Count, Sleep, RetryExceptionTypes.ToArray());
        }

        public static void Retry(this Action Execute, params Type[] RetryExceptionTypes)
        {
            Retry(Execute, DefaultTryCount, DefaultSleep, RetryExceptionTypes.ToArray());
        }

        public static void Retry(this Action Execute, int Count, int Sleep, params Type[] RetryExceptionTypes)
        {
            for (int index = 0; index < Count; index++)
            {
                try
                {
                    Execute();
                    break;
                }
                catch (Exception e)
                {
                    if (index + 1 == Count || !IsRetryException(e, RetryExceptionTypes))
                    {
                        throw e;
                    }
                    Thread.Sleep(Sleep);
                }
            }
        }

        public static T1 Retry<T1>(this Func<T1> Execute, int Count = 3, int Sleep = 100)
        {
            IEnumerable<Type> RetryExceptionTypes = new Type[] { typeof(Exception) };
            return Retry(Execute, Count, Sleep, RetryExceptionTypes.ToArray());
        }

        public static T1 Retry<T1>(this Func<T1> Execute, params Type[] RetryExceptionTypes)
        {
            return Retry(Execute, DefaultTryCount, DefaultSleep, RetryExceptionTypes.ToArray());
        }

        public static T1 Retry<T1>(this Func<T1> Execute, int Count, int Sleep, params Type[] RetryExceptionTypes)
        {
            for (int index = 0; index < Count; index++)
            {
                try
                {
                    return Execute();
                }
                catch (Exception e)
                {
                    if (index + 1 == Count || !IsRetryException(e, RetryExceptionTypes))
                    {
                        throw e;
                    }
                    Thread.Sleep(Sleep);
                }
            }
            return default(T1);
        }

    }
}
