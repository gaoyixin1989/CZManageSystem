using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;

/*2.1.1  下面的command.CommandText就是捕获的sql语句。

从方法名我们可以看出大致就三类:读取类的sql,[Reader],非读取类的sql,[NonQuery],还有[Scalar],这类用的比较少,跟原始的ADO.NET命令类型基本一样,不多讲.每个sql语句类型的方法都有执行前Executing,执行后Executed

2.2 然后要在程序的入口（Application_Start）注册一条监听器

void Application_Start(object sender, EventArgs e)
{
    DbInterception.Add(new EFIntercepterLogging());
}
*/


class EFIntercepterLogging : DbCommandInterceptor
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public override void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
    {

        var sql = command.CommandText;//这里可以获得想要的sql语句
        base.ScalarExecuting(command, interceptionContext);
        _stopwatch.Restart();
    }
    public override void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
    {
        _stopwatch.Stop();
        if (interceptionContext.Exception != null)
        {
            Trace.TraceError("Exception:{1} rn --> Error executing command: {0}", command.CommandText, interceptionContext.Exception.ToString());
        }
        else
        {
            Trace.TraceInformation("rn执行时间:{0} 毫秒rn-->ScalarExecuted.Command:{1}rn", _stopwatch.ElapsedMilliseconds, command.CommandText);
        }
        base.ScalarExecuted(command, interceptionContext);
    }
    public override void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
    {
        base.NonQueryExecuting(command, interceptionContext);
        _stopwatch.Restart();
    }
    public override void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
    {
        _stopwatch.Stop();
        if (interceptionContext.Exception != null)
        {
            Trace.TraceError("Exception:{1} rn --> Error executing command:rn {0}", command.CommandText, interceptionContext.Exception.ToString());
        }
        else
        {
            Trace.TraceInformation("rn执行时间:{0} 毫秒rn-->NonQueryExecuted.Command:rn{1}", _stopwatch.ElapsedMilliseconds, command.CommandText);
        }
        base.NonQueryExecuted(command, interceptionContext);
    }
    public override void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
    {
        base.ReaderExecuting(command, interceptionContext);
        _stopwatch.Restart();
    }
    public override void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
    {
        _stopwatch.Stop();
        if (interceptionContext.Exception != null)
        {
            Trace.TraceError("Exception:{1} rn --> Error executing command:rn {0}", command.CommandText, interceptionContext.Exception.ToString());
        }
        else
        {
            Trace.TraceInformation("rn执行时间:{0} 毫秒 rn -->ReaderExecuted.Command:rn{1}", _stopwatch.ElapsedMilliseconds, command.CommandText);
        }
        base.ReaderExecuted(command, interceptionContext);
    }
}


