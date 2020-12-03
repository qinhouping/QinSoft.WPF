using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.PipeFilter
{
    /// <summary>
    /// 管道过滤器
    /// </summary>
    public abstract class PipeFilterBase
    {
        private PipeFilterBase nextPipeFilter;

        public abstract void Action(PipeFilterEventArgs arg);

        public virtual PipeFilterBase SetNextPipeFilter(PipeFilterBase pipeFilter)
        {
            this.nextPipeFilter = pipeFilter;
            return pipeFilter;
        }

        public virtual PipeFilterBase GetNexttPipeFilter(PipeFilterEventArgs arg)
        {
            return this.nextPipeFilter;
        }

        public virtual void Run(PipeFilterEventArgs arg)
        {
            Action(arg);
            if (arg.Cancel == true) return;
            PipeFilterBase nextFilter = GetNexttPipeFilter(arg);
            if (nextFilter == null) return;
            nextFilter.Run(new PipeFilterEventArgs() { InArg = arg.OutArg });
        }
    }
}
