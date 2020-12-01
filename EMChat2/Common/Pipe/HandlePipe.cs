using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.Pipe
{
    /// <summary>
    /// 处理管道
    /// </summary>
    public abstract class HandlePipe
    {
        private HandlePipe nextHandlePipe;

        public HandlePipe(HandlePipe nextHandlePipe)
        {
            this.nextHandlePipe = nextHandlePipe;
        }

        public abstract void Action(HandlePipeEventArgs arg);

        public virtual void Run(HandlePipeEventArgs arg)
        {
            Action(arg);
            if (arg.Cancel == true) return;
            else nextHandlePipe?.Run(new HandlePipeEventArgs() { InArg = arg.OutArg });
        }
    }
}
